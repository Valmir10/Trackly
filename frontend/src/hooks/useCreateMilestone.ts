import { useMutation, useQueryClient } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'

export function useCreateMilestone(projectId: string | undefined) {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: async ({ contractId, title }: { contractId: string; title: string }) => {
      const response = await apiClient.post<{ id: string }>(`/api/contracts/${contractId}/milestones`, { title })
      return response.data
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['milestones', projectId] })
    },
  })
}
