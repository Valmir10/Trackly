import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { MilestoneDto } from '@/api/types'

export function useProjectMilestones(projectId: string | undefined) {
  return useQuery({
    queryKey: ['milestones', projectId],
    queryFn: async () => {
      const response = await apiClient.get<MilestoneDto[]>(`/api/projects/${projectId}/milestones`)
      return response.data
    },
    enabled: !!projectId,
  })
}
