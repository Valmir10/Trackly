import { useMutation, useQueryClient } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'

export function useRevokeClientRoomAccess(projectId: string | undefined) {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: async (accessId: string) => {
      await apiClient.post(`/api/client-room-access/${accessId}/revoke`)
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['client-room-access', projectId] })
    },
  })
}
