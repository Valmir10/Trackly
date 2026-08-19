import { useMutation, useQueryClient } from '@tanstack/react-query'
import { clientRoomApiClient } from '@/lib/clientRoomApiClient'

// Refetch-after-approve IS the live-update mechanism here — the Client Room
// never opens a SignalR connection (see PRODUCT.md/the Move 8 plan), so
// invalidating the summary query is what makes the progress bar/Approved
// state update without a page reload.
export function useApproveMilestone() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: async (milestoneId: string) => {
      const response = await clientRoomApiClient.post<{ approvalId: string }>(
        `/api/client-room/milestones/${milestoneId}/approve`
      )
      return response.data
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['client-room-summary'] })
    },
  })
}
