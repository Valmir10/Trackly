import { useMutation, useQueryClient } from '@tanstack/react-query'
import { apiClient } from '@/lib/apiClient'

export function useCreateContract(projectId: string | undefined) {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: async (title: string) => {
      const response = await apiClient.post<{ id: string }>(`/api/projects/${projectId}/contracts`, { title })
      return response.data
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['contracts', projectId] })
    },
  })
}
