import {
  LineChart, Line, BarChart, Bar, PieChart, Pie, Cell,
  XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Legend,
} from 'recharts'
import AppSidebar from '@/components/AppSidebar'
import AppTopBar from '@/components/AppTopBar'

const weeklyData = [
  { week: 'Apr 14', completed: 3, created: 5 },
  { week: 'Apr 21', completed: 5, created: 4 },
  { week: 'Apr 28', completed: 4, created: 7 },
  { week: 'May 5', completed: 7, created: 6 },
  { week: 'May 12', completed: 6, created: 8 },
  { week: 'May 19', completed: 9, created: 7 },
  { week: 'May 26', completed: 8, created: 5 },
  { week: 'Jun 2', completed: 11, created: 9 },
]

const projectData = [
  { name: 'Frontend redesign', open: 7, done: 11 },
  { name: 'API v2', open: 15, done: 9 },
  { name: 'Mobile app', open: 27, done: 4 },
  { name: 'Marketing site', open: 0, done: 12 },
]

const statusData = [
  { name: 'Done', value: 36, color: '#22c55e' },
  { name: 'In Progress', value: 14, color: '#f59e0b' },
  { name: 'In Review', value: 8, color: '#3b82f6' },
  { name: 'To Do', value: 27, color: '#6b7280' },
]

const stats = [
  { label: 'Tasks completed', value: '36', sub: '+11 this week' },
  { label: 'Completion rate', value: '72%', sub: '+8% vs last month' },
  { label: 'Avg. time to complete', value: '3.2d', sub: 'Per task' },
  { label: 'Overdue tasks', value: '4', sub: '2 high priority' },
]

export default function AnalyticsPage() {
  return (
    <div className="flex h-screen overflow-hidden bg-background">
      <AppSidebar />
      <div className="flex flex-1 flex-col overflow-hidden">
        <AppTopBar />
        <main className="flex-1 overflow-y-auto p-6">
          <div className="mx-auto max-w-6xl flex flex-col gap-6">

            <div>
              <h1 className="text-xl font-semibold text-foreground">Analytics</h1>
              <p className="mt-0.5 text-sm text-muted-foreground">Workspace performance overview</p>
            </div>

            <div className="grid grid-cols-2 gap-4 xl:grid-cols-4">
              {stats.map((s) => (
                <div key={s.label} className="rounded-xl border border-border/60 bg-card p-5">
                  <p className="text-xs text-muted-foreground">{s.label}</p>
                  <p className="mt-1.5 text-2xl font-semibold text-foreground">{s.value}</p>
                  <p className="mt-2 text-xs text-green-400">{s.sub}</p>
                </div>
              ))}
            </div>

            <div className="rounded-xl border border-border/60 bg-card p-6">
              <h2 className="mb-1 text-sm font-semibold text-foreground">Task completion over time</h2>
              <p className="mb-5 text-xs text-muted-foreground">Weekly tasks created vs completed</p>
              <ResponsiveContainer width="100%" height={220}>
                <LineChart data={weeklyData}>
                  <CartesianGrid strokeDasharray="3 3" stroke="oklch(1 0 0 / 8%)" />
                  <XAxis dataKey="week" tick={{ fontSize: 11, fill: 'oklch(0.556 0 0)' }} />
                  <YAxis tick={{ fontSize: 11, fill: 'oklch(0.556 0 0)' }} />
                  <Tooltip contentStyle={{ backgroundColor: 'oklch(0.205 0 0)', border: '1px solid oklch(1 0 0 / 10%)', borderRadius: '8px', fontSize: 12 }} />
                  <Legend />
                  <Line type="monotone" dataKey="completed" stroke="#f43f5e" strokeWidth={2} dot={false} name="Completed" />
                  <Line type="monotone" dataKey="created" stroke="#6b7280" strokeWidth={2} dot={false} name="Created" />
                </LineChart>
              </ResponsiveContainer>
            </div>

            <div className="grid gap-6 xl:grid-cols-5">
              <div className="xl:col-span-3 rounded-xl border border-border/60 bg-card p-6">
                <h2 className="mb-1 text-sm font-semibold text-foreground">Tasks by project</h2>
                <p className="mb-5 text-xs text-muted-foreground">Open vs completed per project</p>
                <ResponsiveContainer width="100%" height={200}>
                  <BarChart data={projectData} layout="vertical">
                    <CartesianGrid strokeDasharray="3 3" stroke="oklch(1 0 0 / 8%)" horizontal={false} />
                    <XAxis type="number" tick={{ fontSize: 11, fill: 'oklch(0.556 0 0)' }} />
                    <YAxis type="category" dataKey="name" width={120} tick={{ fontSize: 11, fill: 'oklch(0.556 0 0)' }} />
                    <Tooltip contentStyle={{ backgroundColor: 'oklch(0.205 0 0)', border: '1px solid oklch(1 0 0 / 10%)', borderRadius: '8px', fontSize: 12 }} />
                    <Legend />
                    <Bar dataKey="done" fill="#f43f5e" radius={[0, 4, 4, 0]} name="Done" />
                    <Bar dataKey="open" fill="oklch(0.269 0 0)" radius={[0, 4, 4, 0]} name="Open" />
                  </BarChart>
                </ResponsiveContainer>
              </div>

              <div className="xl:col-span-2 rounded-xl border border-border/60 bg-card p-6">
                <h2 className="mb-1 text-sm font-semibold text-foreground">Status breakdown</h2>
                <p className="mb-4 text-xs text-muted-foreground">All tasks by current status</p>
                <ResponsiveContainer width="100%" height={160}>
                  <PieChart>
                    <Pie data={statusData} cx="50%" cy="50%" innerRadius={50} outerRadius={75} dataKey="value" paddingAngle={3}>
                      {statusData.map((entry) => (
                        <Cell key={entry.name} fill={entry.color} />
                      ))}
                    </Pie>
                    <Tooltip contentStyle={{ backgroundColor: 'oklch(0.205 0 0)', border: '1px solid oklch(1 0 0 / 10%)', borderRadius: '8px', fontSize: 12 }} />
                  </PieChart>
                </ResponsiveContainer>
                <div className="mt-2 flex flex-col gap-1.5">
                  {statusData.map((s) => (
                    <div key={s.name} className="flex items-center justify-between text-xs">
                      <div className="flex items-center gap-1.5">
                        <div className="h-2 w-2 rounded-full" style={{ backgroundColor: s.color }} />
                        <span className="text-muted-foreground">{s.name}</span>
                      </div>
                      <span className="font-medium text-foreground">{s.value}</span>
                    </div>
                  ))}
                </div>
              </div>
            </div>

          </div>
        </main>
      </div>
    </div>
  )
}
