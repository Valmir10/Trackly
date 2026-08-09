import AppShell from '@/components/AppShell'
import StatsRow from '@/components/StatsRow'
import MyTasksCard from '@/components/MyTasksCard'
import RecentActivityCard from '@/components/RecentActivityCard'
import PulseFeed from '@/components/PulseFeed'
import ScopeSwitcher from '@/components/ScopeSwitcher'
import '@/styles/DashboardPage.css'

export default function DashboardPage() {
  return (
    <AppShell>
      <div className="tp-dashboard">
        <div className="tp-page-header tp-page-header--row">
          <div>
            <h1 className="tp-page-header__title">Good morning, Valmir</h1>
            <p className="tp-page-header__subtitle">Here is what is happening across your workspace today.</p>
          </div>
          <ScopeSwitcher />
        </div>

        <StatsRow />

        <PulseFeed />

        <div className="tp-dashboard__grid">
          <MyTasksCard />
          <RecentActivityCard />
        </div>
      </div>
    </AppShell>
  )
}
