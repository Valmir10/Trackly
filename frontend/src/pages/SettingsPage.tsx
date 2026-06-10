import { useState } from 'react'
import AppSidebar from '@/components/dashboard/AppSidebar'
import AppTopBar from '@/components/dashboard/AppTopBar'
import SettingsSidebar from '@/components/settings/SettingsSidebar'
import WorkspaceSettings from '@/components/settings/WorkspaceSettings'
import TeamSettings from '@/components/settings/TeamSettings'

function SettingsContent({ active }: { active: string }) {
  if (active === 'Team members') return <TeamSettings />
  return <WorkspaceSettings />
}

export default function SettingsPage() {
  const [activeSection, setActiveSection] = useState('Workspace')

  return (
    <div className="flex h-screen overflow-hidden bg-background">
      <AppSidebar />

      <div className="flex flex-1 flex-col overflow-hidden">
        <AppTopBar />

        <div className="flex flex-1 overflow-hidden">
          <SettingsSidebar active={activeSection} onSelect={setActiveSection} />

          <main className="flex-1 overflow-y-auto p-8">
            <SettingsContent active={activeSection} />
          </main>
        </div>
      </div>
    </div>
  )
}
