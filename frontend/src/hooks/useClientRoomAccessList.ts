import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { ClientRoomAccessDto } from '@/api/types'

export function useClientRoomAccessList(projectId: string | undefined) {
  return useQuery({
    queryKey: ['client-room-access', projectId],
    queryFn: async () => {
      const response = await apiClient.get<ClientRoomAccessDto[]>(`/api/projects/${projectId}/client-room-access`)
      return response.data
    },
    enabled: !!projectId,
  })
}
