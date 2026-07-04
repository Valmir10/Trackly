import AppShell from '@/components/AppShell'
import StatsRow from '@/components/StatsRow'
import MyTasksCard from '@/components/MyTasksCard'
import RecentActivityCard from '@/components/RecentActivityCard'
import AiSummaryCard from '@/components/AiSummaryCard'

export default function DashboardPage() {
  return (
    <AppShell>
      <div className="p-6">
        <div className="mx-auto max-w-6xl flex flex-col gap-6">

          <div>
            <h1 className="text-xl font-semibold text-foreground">Good morning, Valmir</h1>
            <p className="mt-0.5 text-sm text-muted-foreground">
              Here is what is happening across your workspace today.
            </p>
          </div>

          <StatsRow />

          <AiSummaryCard />

          <div className="grid gap-6 xl:grid-cols-5">
            <div className="xl:col-span-3">
              <MyTasksCard />
            </div>
            <div className="xl:col-span-2">
              <RecentActivityCard />
            </div>
          </div>

        </div>
      </div>
    </AppShell>
  )
}
