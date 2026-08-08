import { useEffect } from 'react'
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { API_BASE_URL } from '@/lib/apiClient'
import { useAuthStore } from '@/store/useAuthStore'
import { useTaskStore } from '@/store/useTaskStore'
import { useChatStore } from '@/store/useChatStore'
import { mapChatMessageDtoToChatMessage } from '@/chat/adapters'
import type { BackendTicketStatus, ChatMessageDto } from '@/api/types'
import type { ChatScope } from '@/chat/types'

interface TicketStatusChangedPayload {
  ticketId: string
  status: BackendTicketStatus
}

interface ChatMessageSentPayload {
  messageId: string
  ticketId: string | null
  authorId: string
  authorInitials: string
  content: string
  createdAt: string
}

// One SignalR connection per mounted project page, joining that project's
// broadcast group — this is the actual "live" half of Move 5. Everything
// upstream (the real PATCH/POST calls, the domain events, the Api-layer
// broadcast handlers) exists so this hook has something real to listen for.
export function useProjectHub(projectId: string | undefined) {
  useEffect(() => {
    if (!projectId) return

    const connection = new HubConnectionBuilder()
      .withUrl(`${API_BASE_URL}/hubs/project`, {
        accessTokenFactory: () => useAuthStore.getState().accessToken ?? '',
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build()

    connection.on('TicketStatusChanged', (payload: TicketStatusChangedPayload) => {
      useTaskStore.getState().applyRemoteStatusChange(payload.ticketId, payload.status)
    })

    connection.on('ChatMessageSent', (payload: ChatMessageSentPayload) => {
      const scope: ChatScope = payload.ticketId
        ? { type: 'ticket', projectId, ticketId: payload.ticketId }
        : { type: 'project', projectId }

      const dto: ChatMessageDto = {
        id: payload.messageId,
        authorId: payload.authorId,
        authorInitials: payload.authorInitials,
        content: payload.content,
        ticketId: payload.ticketId,
        createdAt: payload.createdAt,
      }

      useChatStore.getState().applyRemoteMessage(scope, mapChatMessageDtoToChatMessage(dto))
    })

    connection
      .start()
      .then(() => connection.invoke('JoinProject', projectId))
      .catch((error: unknown) => console.error('SignalR connection failed', error))

    return () => {
      void connection.stop()
    }
  }, [projectId])
}
