import { useEffect } from 'react'
import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { DecisionDto } from '@/api/types'
import { useDecisionStore } from '@/store/useDecisionStore'

// Feeds the #-picker's decision search — without this, only decisions from
// meetings already visited this session (via useMeetingDecisions) would be
// findable, which isn't "indexed inside the # picker framework."
export function useWorkspaceDecisions() {
  const setDecisionsFromApi = useDecisionStore((s) => s.setDecisionsFromApi)

  const query = useQuery({
    queryKey: ['workspace-decisions'],
    queryFn: async () => {
      const response = await apiClient.get<DecisionDto[]>('/api/decisions')
      return response.data
    },
  })

  useEffect(() => {
    if (query.data) setDecisionsFromApi(query.data)
  }, [query.data, setDecisionsFromApi])

  return query
}
