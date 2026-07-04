import { useState } from 'react'
import AppShell from '@/components/AppShell'
import SettingsSidebar from '@/components/SettingsSidebar'
import WorkspaceSettings from '@/components/WorkspaceSettings'
import TeamSettings from '@/components/TeamSettings'
import BillingSettings from '@/components/BillingSettings'
import ApiKeysSettings from '@/components/ApiKeysSettings'
import WebhooksSettings from '@/components/WebhooksSettings'
import NotificationsSettings from '@/components/NotificationsSettings'
import ProfileSettings from '@/components/ProfileSettings'

function SettingsContent({ active }: { active: string }) {
  switch (active) {
    case 'Team members': return <TeamSettings />
    case 'Billing': return <BillingSettings />
    case 'API keys': return <ApiKeysSettings />
    case 'Webhooks': return <WebhooksSettings />
    case 'Notifications': return <NotificationsSettings />
    case 'Profile': return <ProfileSettings />
    default: return <WorkspaceSettings />
  }
}

export default function SettingsPage() {
  const [activeSection, setActiveSection] = useState('Workspace')

  return (
    <AppShell>
      <div className="flex h-full overflow-hidden">
        <SettingsSidebar active={activeSection} onSelect={setActiveSection} />
        <div className="flex-1 overflow-y-auto p-8">
          <SettingsContent active={activeSection} />
        </div>
      </div>
    </AppShell>
  )
}
