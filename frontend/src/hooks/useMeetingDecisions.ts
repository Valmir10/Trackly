import { useEffect } from 'react'
import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { DecisionDto } from '@/api/types'
import { useDecisionStore } from '@/store/useDecisionStore'

export function useMeetingDecisions(meetingId: string | undefined) {
  const setDecisionsFromApi = useDecisionStore((s) => s.setDecisionsFromApi)

  const query = useQuery({
    queryKey: ['meeting-decisions', meetingId],
    queryFn: async () => {
      const response = await apiClient.get<DecisionDto[]>(`/api/meetings/${meetingId}/decisions`)
      return response.data
    },
    enabled: !!meetingId,
  })

  useEffect(() => {
    if (query.data) setDecisionsFromApi(query.data)
  }, [query.data, setDecisionsFromApi])

  return query
}
