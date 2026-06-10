import { useState } from 'react'
import AppSidebar from '@/components/AppSidebar'
import AppTopBar from '@/components/AppTopBar'
import SettingsSidebar from '@/components/SettingsSidebar'
import WorkspaceSettings from '@/components/WorkspaceSettings'
import TeamSettings from '@/components/TeamSettings'

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
