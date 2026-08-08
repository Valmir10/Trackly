import { create } from 'zustand'
import { persist } from 'zustand/middleware'

// The access token is deliberately NEVER persisted (memory-only) — an XSS
// exploit reading localStorage would otherwise get a usable session token.
// tenantSlug/userId/tenantId aren't secrets (they're already visible in the
// URL/JWT), so persisting just those lets the app know "there was a session
// here" across a refresh without persisting anything sensitive; the actual
// access token gets re-established via the HttpOnly refresh cookie on load.
interface AuthState {
  accessToken: string | null
  userId: string | null
  tenantId: string | null
  tenantSlug: string | null
  isInitializing: boolean
  setSession: (session: { accessToken: string; userId: string; tenantId: string; tenantSlug: string }) => void
  setAccessToken: (accessToken: string) => void
  setInitializing: (isInitializing: boolean) => void
  clearSession: () => void
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      accessToken: null,
      userId: null,
      tenantId: null,
      tenantSlug: null,
      isInitializing: true,

      setSession: ({ accessToken, userId, tenantId, tenantSlug }) =>
        set({ accessToken, userId, tenantId, tenantSlug, isInitializing: false }),

      setAccessToken: (accessToken) => set({ accessToken, isInitializing: false }),

      setInitializing: (isInitializing) => set({ isInitializing }),

      clearSession: () =>
        set({ accessToken: null, userId: null, tenantId: null, tenantSlug: null, isInitializing: false }),
    }),
    {
      name: 'trackly.auth.v1',
      partialize: (state) => ({ userId: state.userId, tenantId: state.tenantId, tenantSlug: state.tenantSlug }),
    }
  )
)
