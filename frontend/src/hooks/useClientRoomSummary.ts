import { useQuery } from '@tanstack/react-query'
import { clientRoomApiClient } from '@/lib/clientRoomApiClient'
import type { ClientRoomSummaryDto } from '@/api/types'

export function useClientRoomSummary() {
  return useQuery({
    queryKey: ['client-room-summary'],
    queryFn: async () => {
      const response = await clientRoomApiClient.get<ClientRoomSummaryDto>('/api/client-room/summary')
      return response.data
    },
    retry: false,
  })
}
