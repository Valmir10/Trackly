import AppSidebar from '@/components/dashboard/AppSidebar'
import AppTopBar from '@/components/dashboard/AppTopBar'
import StatsRow from '@/components/dashboard/StatsRow'
import MyTasksCard from '@/components/dashboard/MyTasksCard'
import RecentActivityCard from '@/components/dashboard/RecentActivityCard'
import AiSummaryCard from '@/components/dashboard/AiSummaryCard'

export default function DashboardPage() {
  return (
    <div className="flex h-screen overflow-hidden bg-background">
      <AppSidebar />

      <div className="flex flex-1 flex-col overflow-hidden">
        <AppTopBar />

        <main className="flex-1 overflow-y-auto p-6">
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
        </main>
      </div>
    </div>
  )
}
