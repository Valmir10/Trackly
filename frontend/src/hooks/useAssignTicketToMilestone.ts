import { useMutation, useQueryClient } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'

export function useAssignTicketToMilestone(projectId: string | undefined) {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: async ({ ticketId, milestoneId }: { ticketId: string; milestoneId: string | null }) => {
      await apiClient.patch(`/api/tickets/${ticketId}/milestone`, { milestoneId })
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['tickets', projectId] })
      queryClient.invalidateQueries({ queryKey: ['milestones', projectId] })
    },
  })
}
