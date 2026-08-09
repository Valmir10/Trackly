import { useEffect } from 'react'
import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { MeetingSummaryDto } from '@/api/types'
import { useMeetingStore } from '@/store/useMeetingStore'

export function useMeetings(projectId: string | undefined) {
  const setMeetingsFromApi = useMeetingStore((s) => s.setMeetingsFromApi)

  const query = useQuery({
    queryKey: ['meetings', projectId],
    queryFn: async () => {
      const response = await apiClient.get<MeetingSummaryDto[]>(`/api/projects/${projectId}/meetings`)
      return response.data
    },
    enabled: !!projectId,
  })

  useEffect(() => {
    if (query.data) setMeetingsFromApi(query.data)
  }, [query.data, setMeetingsFromApi])

  return query
}
