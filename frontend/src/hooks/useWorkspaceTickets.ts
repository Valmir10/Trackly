import { useQuery } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'
import type { TicketDto } from '@/api/types'

// All tickets across every project in the tenant — the single fetch Pulse
// and Analytics both compute their real signals from, instead of each
// surface firing its own per-project fan-out.
export function useWorkspaceTickets() {
  return useQuery({
    queryKey: ['workspace-tickets'],
    queryFn: async () => {
      const response = await apiClient.get<TicketDto[]>('/api/tickets')
      return response.data
    },
  })
}
