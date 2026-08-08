import { useEffect } from 'react'
import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { ChatMessageDto } from '@/api/types'
import type { ChatScope } from '@/chat/types'
import { useChatStore } from '@/store/useChatStore'

export function useChatMessages(scope: ChatScope) {
  const setMessagesFromApi = useChatStore((s) => s.setMessagesFromApi)
  const ticketId = scope.type === 'ticket' ? scope.ticketId : undefined

  const query = useQuery({
    queryKey: ['chat-messages', scope.projectId, ticketId ?? null],
    queryFn: async () => {
      const response = await apiClient.get<ChatMessageDto[]>('/api/chat/messages', {
        params: { projectId: scope.projectId, ticketId },
      })
      return response.data
    },
  })

  useEffect(() => {
    if (query.data) setMessagesFromApi(scope, query.data)
    // Re-running on every `scope` identity change would refire needlessly —
    // the query key above already governs when new data actually arrives.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [query.data, setMessagesFromApi])

  return query
}
