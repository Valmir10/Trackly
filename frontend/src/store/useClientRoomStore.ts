import { create } from 'zustand'

// Holds only the credential, mirroring useAuthStore's job — fetched data
// (the summary) stays in TanStack Query, not here. Not persisted: the raw
// token always arrives fresh from the /client-room/:token URL on load.
interface ClientRoomState {
  token: string | null
  setToken: (token: string) => void
  clear: () => void
}

export const useClientRoomStore = create<ClientRoomState>((set) => ({
  token: null,
  setToken: (token) => set({ token }),
  clear: () => set({ token: null }),
}))
