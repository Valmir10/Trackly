import { useMutation, useQueryClient } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'

export function useSetTicketBlockedByTicket(projectId: string | undefined) {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: async ({ ticketId, blockingTicketId }: { ticketId: string; blockingTicketId: string | null }) => {
      await apiClient.patch(`/api/tickets/${ticketId}/blocked-by-ticket`, { blockingTicketId })
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['tickets', projectId] })
    },
  })
}
