import { useEffect } from 'react'
import type { ReactNode } from 'react'
import { Navigate, useLocation } from 'react-router-dom'
import { apiClient } from '@/lib/apiClient'
import { useAuthStore } from '@/store/useAuthStore'

interface RequireAuthProps {
  children: ReactNode
}

export default function RequireAuth({ children }: RequireAuthProps) {
  const accessToken = useAuthStore((s) => s.accessToken)
  const isInitializing = useAuthStore((s) => s.isInitializing)
  const tenantSlug = useAuthStore((s) => s.tenantSlug)
  const userId = useAuthStore((s) => s.userId)
  const tenantId = useAuthStore((s) => s.tenantId)
  const setSession = useAuthStore((s) => s.setSession)
  const setInitializing = useAuthStore((s) => s.setInitializing)
  const location = useLocation()

  useEffect(() => {
    if (!isInitializing) return

    // No prior session recorded — nothing to silently refresh, skip the
    // network round trip entirely.
    if (!tenantSlug || !userId || !tenantId) {
      setInitializing(false)
      return
    }

    apiClient
      .post('/api/auth/refresh')
      .then((response) => {
        setSession({ accessToken: response.data.accessToken, userId, tenantId, tenantSlug })
      })
      .catch(() => setInitializing(false))
    // Only ever needs to run once, on mount.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  if (isInitializing) return null

  if (!accessToken) {
    return <Navigate to="/login" state={{ from: location }} replace />
  }

  return children
}
