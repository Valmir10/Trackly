import { useMutation, useQueryClient } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { CreateClientRoomAccessResult } from '@/api/types'

export function useCreateClientRoomAccess(projectId: string | undefined) {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: async () => {
      const response = await apiClient.post<CreateClientRoomAccessResult>(
        `/api/projects/${projectId}/client-room-access`
      )
      return response.data
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['client-room-access', projectId] })
    },
  })
}
