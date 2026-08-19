import { useEffect } from 'react'
import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { MilestoneDto } from '@/api/types'
import { useMilestoneStore } from '@/store/useMilestoneStore'

// Feeds the #-picker's milestone search — mirrors useWorkspaceDecisions:
// without this, only milestones from a project whose Contracts page has
// already been visited this session would be findable.
export function useWorkspaceMilestones() {
  const setMilestonesFromApi = useMilestoneStore((s) => s.setMilestonesFromApi)

  const query = useQuery({
    queryKey: ['workspace-milestones'],
    queryFn: async () => {
      const response = await apiClient.get<MilestoneDto[]>('/api/milestones')
      return response.data
    },
  })

  useEffect(() => {
    if (query.data) setMilestonesFromApi(query.data)
  }, [query.data, setMilestonesFromApi])

  return query
}
