import { useEffect } from 'react'
import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { TicketDto } from '@/api/types'
import { useTaskStore } from '@/store/useTaskStore'

export function useProjectTickets(projectId: string | undefined) {
  const setTicketsFromApi = useTaskStore((s) => s.setTicketsFromApi)

  const query = useQuery({
    queryKey: ['tickets', projectId],
    queryFn: async () => {
      const response = await apiClient.get<TicketDto[]>(`/api/projects/${projectId}/tickets`)
      return response.data
    },
    enabled: !!projectId,
  })

  useEffect(() => {
    if (query.data) setTicketsFromApi(query.data)
  }, [query.data, setTicketsFromApi])

  return query
}
