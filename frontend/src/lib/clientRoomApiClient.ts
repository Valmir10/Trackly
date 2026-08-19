import axios from 'axios'
import { API_BASE_URL } from '@/lib/apiClient'
import { useClientRoomStore } from '@/store/useClientRoomStore'

// Separate axios instance for the Client Room — no withCredentials (no
// cookies involved) and no retry-with-refresh (nothing to refresh against;
// a client-room token is validated fresh on every request, not exchanged
// for a session).
export const clientRoomApiClient = axios.create({ baseURL: API_BASE_URL })

clientRoomApiClient.interceptors.request.use((config) => {
  const token = useClientRoomStore.getState().token
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

clientRoomApiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      useClientRoomStore.getState().clear()
    }
    return Promise.reject(error)
  }
)
