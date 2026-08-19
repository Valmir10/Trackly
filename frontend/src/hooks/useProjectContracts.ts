import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { ContractDto } from '@/api/types'

export function useProjectContracts(projectId: string | undefined) {
  return useQuery({
    queryKey: ['contracts', projectId],
    queryFn: async () => {
      const response = await apiClient.get<ContractDto[]>(`/api/projects/${projectId}/contracts`)
      return response.data
    },
    enabled: !!projectId,
  })
}
