import { create } from 'zustand'
import type { ChatMessage, ChatScope } from '@/chat/types'
import { scopeKey } from '@/chat/types'
import { mapChatMessageDtoToChatMessage } from '@/chat/adapters'
import type { ChatMessageDto } from '@/api/types'
import { apiClient } from '@/lib/apiClient'

// Messages keyed by scope, so the project stream and a ticket's comment
// thread are just two keys into the same store, not two systems. Real,
// fetched from the backend — sendMessage only ever POSTs; the message that
// actually appears in the thread arrives back through the SignalR broadcast
// (applyRemoteMessage), not as a locally-fabricated optimistic echo. In a
// same-session demo that broadcast is effectively instant, and it avoids
// ever having to guess the current user's own display name client-side.

interface ChatStoreState {
  messages: Record<string, ChatMessage[]>
  setMessagesFromApi: (scope: ChatScope, messages: ChatMessageDto[]) => void
  sendMessage: (scope: ChatScope, rawContent: string) => Promise<void>
  applyRemoteMessage: (scope: ChatScope, message: ChatMessage) => void
  promoteToCard: (scope: ChatScope, messageId: string, blockIndex: number) => void
}

export const useChatStore = create<ChatStoreState>((set) => ({
  messages: {},

  setMessagesFromApi: (scope, messages) => {
    const key = scopeKey(scope)
    set((state) => ({
      messages: { ...state.messages, [key]: messages.map(mapChatMessageDtoToChatMessage) },
    }))
  },

  sendMessage: async (scope, rawContent) => {
    await apiClient.post('/api/chat/messages', {
      projectId: scope.projectId,
      ticketId: scope.type === 'ticket' ? scope.ticketId : null,
      content: rawContent,
    })
  },

  applyRemoteMessage: (scope, message) => {
    const key = scopeKey(scope)
    set((state) => ({
      messages: { ...state.messages, [key]: [...(state.messages[key] ?? []), message] },
    }))
  },

  // Local-only: turns a static "#127" reference into the live card block —
  // no backend call, so it's scoped to this session same as the rest of
  // Move 5's "live" behavior.
  promoteToCard: (scope, messageId, blockIndex) => {
    const key = scopeKey(scope)
    set((state) => {
      const existing = state.messages[key]
      if (!existing) return state
      const messages = existing.map((message) => {
        if (message.id !== messageId) return message
        const block = message.blocks[blockIndex]
        if (block.type !== 'ticketRef') return message
        const blocks = message.blocks.map((b, i) =>
          i === blockIndex ? { type: 'card' as const, ticketId: block.ticketId } : b
        )
        return { ...message, blocks }
      })
      return { messages: { ...state.messages, [key]: messages } }
    })
  },
}))
