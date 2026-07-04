import AppShell from '@/components/AppShell'
import StatsRow from '@/components/StatsRow'
import MyTasksCard from '@/components/MyTasksCard'
import RecentActivityCard from '@/components/RecentActivityCard'
import WeeklySummaryCard from '@/components/WeeklySummaryCard'
import '@/styles/DashboardPage.css'

export default function DashboardPage() {
  return (
    <AppShell>
      <div className="tp-dashboard">
        <div className="tp-page-header">
          <h1 className="tp-page-header__title">Good morning, Valmir</h1>
          <p className="tp-page-header__subtitle">Here is what is happening across your workspace today.</p>
        </div>

        <StatsRow />

        <WeeklySummaryCard />

        <div className="tp-dashboard__grid">
          <MyTasksCard />
          <RecentActivityCard />
        </div>
      </div>
    </AppShell>
  )
}
