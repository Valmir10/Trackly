import axios from 'axios'
import { useAuthStore } from '@/store/useAuthStore'

export const API_BASE_URL = 'http://localhost:5080'

// withCredentials so the HttpOnly refresh-token cookie actually travels with
// every request (and the /auth/refresh call in particular).
export const apiClient = axios.create({ baseURL: API_BASE_URL, withCredentials: true })

apiClient.interceptors.request.use((config) => {
  const token = useAuthStore.getState().accessToken
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

let refreshPromise: Promise<string | null> | null = null

async function refreshAccessToken(): Promise<string | null> {
  refreshPromise ??= axios
    .post(`${API_BASE_URL}/api/auth/refresh`, null, { withCredentials: true })
    .then((response) => {
      const token = response.data.accessToken as string
      useAuthStore.getState().setAccessToken(token)
      return token
    })
    .catch(() => {
      useAuthStore.getState().clearSession()
      return null
    })
    .finally(() => {
      refreshPromise = null
    })

  return refreshPromise
}

// A 401 mid-session almost always just means the 15-minute access token
// expired — retry the exact same request once with a freshly refreshed
// token before giving up, so an expired token isn't a user-visible error.
apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config
    if (error.response?.status === 401 && !originalRequest._retried) {
      originalRequest._retried = true
      const token = await refreshAccessToken()
      if (token) {
        originalRequest.headers.Authorization = `Bearer ${token}`
        return apiClient(originalRequest)
      }
    }
    return Promise.reject(error)
  }
)
