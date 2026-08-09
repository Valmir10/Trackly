import { useEffect } from 'react'
import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { MeetingDto } from '@/api/types'
import { useMeetingStore } from '@/store/useMeetingStore'

export function useMeeting(meetingId: string | undefined) {
  const setMeetingFromApi = useMeetingStore((s) => s.setMeetingFromApi)

  const query = useQuery({
    queryKey: ['meeting', meetingId],
    queryFn: async () => {
      const response = await apiClient.get<MeetingDto>(`/api/meetings/${meetingId}`)
      return response.data
    },
    enabled: !!meetingId,
  })

  useEffect(() => {
    if (query.data) setMeetingFromApi(query.data)
  }, [query.data, setMeetingFromApi])

  return query
}
