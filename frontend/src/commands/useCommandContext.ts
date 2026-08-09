import { useCallback } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { useCommandRegistry } from './registry'
import type { CommandContext } from './types'

// Shared by AppShell (the global keyboard layer needs a stable getter that
// still reads fresh state on every call) and CommandPalette (which just
// needs the context object itself). One construction path, not two.
export function useCommandContextGetter(): () => CommandContext {
  const navigate = useNavigate()
  const { slug = 'acme-corp' } = useParams()

  return useCallback(
    (): CommandContext => ({
      navigate,
      slug,
      openTicket: (ticketId) => navigate(`/${slug}/projects/1?ticket=${ticketId}`),
      openMeeting: (meetingId) => navigate(`/${slug}/meetings/${meetingId}`),
      close: () => useCommandRegistry.getState().close(),
      push: (page) => useCommandRegistry.getState().pushPage(page),
    }),
    [navigate, slug]
  )
}
