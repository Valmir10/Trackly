import { useState } from 'react'
import type { ReactNode } from 'react'
import AppSidebar from '@/components/AppSidebar'
import AppTopBar from '@/components/AppTopBar'
import CommandPalette from '@/components/CommandPalette'
import { useGlobalCommandKeys } from '@/commands/useGlobalKeys'
import { useCommandContextGetter } from '@/commands/useCommandContext'
import { useBuiltinCommands } from '@/commands/builtins'
import { useBuiltinMeetingCommands } from '@/commands/meetingCommands'
import '@/styles/tokens.css'
import '@/styles/tp-primitives.css'
import '@/styles/AppShell.css'

interface AppShellProps {
  children: ReactNode
}

export default function AppShell({ children }: AppShellProps) {
  const [mobileNavOpen, setMobileNavOpen] = useState(false)

  useBuiltinCommands()
  useBuiltinMeetingCommands()
  useGlobalCommandKeys(useCommandContextGetter())

  return (
    <div className="tp-shell tp-app">
      <div className="tp-dot-grid tp-dot-grid--shell" aria-hidden="true" />
      <AppSidebar mobileOpen={mobileNavOpen} onCloseMobile={() => setMobileNavOpen(false)} />

      <div className="tp-app__main">
        <AppTopBar onOpenMobileNav={() => setMobileNavOpen(true)} />
        <main className="tp-app__content">{children}</main>
      </div>

      <CommandPalette />
    </div>
  )
}
