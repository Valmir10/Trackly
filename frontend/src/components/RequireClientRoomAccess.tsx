import { useEffect } from 'react'
import type { ReactNode } from 'react'
import { useParams } from 'react-router-dom'
import { useClientRoomStore } from '@/store/useClientRoomStore'

interface RequireClientRoomAccessProps {
  children: ReactNode
}

// No /login fallback — an invalid/expired/revoked token surfaces as a
// "this link is no longer valid" state inside ClientRoomPage, driven off
// the summary query's failure, not a redirect. Unlike RequireAuth, there's
// no session to restore: the token IS the URL, every load.
export default function RequireClientRoomAccess({ children }: RequireClientRoomAccessProps) {
  const { token = '' } = useParams()
  const setToken = useClientRoomStore((s) => s.setToken)

  useEffect(() => {
    setToken(token)
  }, [token, setToken])

  return children
}
