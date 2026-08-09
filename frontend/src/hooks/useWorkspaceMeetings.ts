import { useEffect } from 'react'
import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { MeetingSummaryDto } from '@/api/types'
import { useMeetingStore } from '@/store/useMeetingStore'

export function useWorkspaceMeetings() {
  const setMeetingsFromApi = useMeetingStore((s) => s.setMeetingsFromApi)

  const query = useQuery({
    queryKey: ['workspace-meetings'],
    queryFn: async () => {
      const response = await apiClient.get<MeetingSummaryDto[]>('/api/meetings')
      return response.data
    },
  })

  useEffect(() => {
    if (query.data) setMeetingsFromApi(query.data)
  }, [query.data, setMeetingsFromApi])

  return query
}
