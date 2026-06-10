import { useState } from 'react'
import { Circle, CheckCircle2 } from 'lucide-react'
import { Badge } from '@/components/ui/badge'
import { Avatar, AvatarFallback } from '@/components/ui/avatar'
import AppSidebar from '@/components/AppSidebar'
import AppTopBar from '@/components/AppTopBar'

const statusTabs = ['All', 'To Do', 'In Progress', 'In Review', 'Done']

const tasks = [
  { title: 'Design dashboard layout and stat cards', project: 'Frontend redesign', projectColor: 'bg-rose-500', priority: 'High', priorityColor: 'text-red-400', status: 'In Progress', due: 'Jun 11', dueSoon: true },
  { title: 'Write API documentation for auth endpoints', project: 'API v2', projectColor: 'bg-blue-500', priority: 'Medium', priorityColor: 'text-yellow-400', status: 'To Do', due: 'Jun 14', dueSoon: false },
  { title: 'Fix authentication bug in refresh token flow', project: 'API v2', projectColor: 'bg-blue-500', priority: 'High', priorityColor: 'text-red-400', status: 'In Progress', due: 'Jun 13', dueSoon: true },
  { title: 'Update onboarding copy and CTA text', project: 'Marketing site', projectColor: 'bg-orange-500', priority: 'Low', priorityColor: 'text-green-400', status: 'To Do', due: 'Jun 20', dueSoon: false },
  { title: 'Implement dark mode toggle in settings', project: 'Frontend redesign', projectColor: 'bg-rose-500', priority: 'Medium', priorityColor: 'text-yellow-400', status: 'In Review', due: 'Jun 12', dueSoon: true },
  { title: 'Set up Storybook documentation', project: 'Frontend redesign', projectColor: 'bg-rose-500', priority: 'Low', priorityColor: 'text-green-400', status: 'To Do', due: 'Jun 22', dueSoon: false },
  { title: 'Landing page hero section redesign', project: 'Marketing site', projectColor: 'bg-orange-500', priority: 'High', priorityColor: 'text-red-400', status: 'In Review', due: 'Jun 10', dueSoon: true },
  { title: 'CI/CD pipeline setup', project: 'Frontend redesign', projectColor: 'bg-rose-500', priority: 'High', priorityColor: 'text-red-400', status: 'Done', due: 'Jun 1', dueSoon: false },
  { title: 'Configure Tailwind and shadcn', project: 'Frontend redesign', projectColor: 'bg-rose-500', priority: 'Medium', priorityColor: 'text-yellow-400', status: 'Done', due: 'Jun 3', dueSoon: false },
]

export default function MyTasksPage() {
  const [activeTab, setActiveTab] = useState('All')

  const filtered = activeTab === 'All' ? tasks : tasks.filter(t => t.status === activeTab)

  return (
    <div className="flex h-screen overflow-hidden bg-background">
      <AppSidebar />
      <div className="flex flex-1 flex-col overflow-hidden">
        <AppTopBar />
        <main className="flex-1 overflow-y-auto p-6">
          <div className="mx-auto max-w-4xl">

            <div className="mb-6">
              <h1 className="text-xl font-semibold text-foreground">My Tasks</h1>
              <p className="mt-0.5 text-sm text-muted-foreground">{tasks.length} tasks assigned to you</p>
            </div>

            <div className="mb-6 flex gap-1 border-b border-border/60">
              {statusTabs.map((tab) => {
                const count = tab === 'All' ? tasks.length : tasks.filter(t => t.status === tab).length
                return (
                  <button
                    key={tab}
                    onClick={() => setActiveTab(tab)}
                    className={`flex items-center gap-1.5 px-4 py-2 text-sm transition-colors ${
                      activeTab === tab
                        ? 'border-b-2 border-rose-500 font-medium text-foreground'
                        : 'text-muted-foreground hover:text-foreground'
                    }`}
                  >
                    {tab}
                    <span className={`rounded-full px-1.5 py-0.5 text-[10px] ${
                      activeTab === tab ? 'bg-rose-500/15 text-rose-400' : 'bg-muted text-muted-foreground'
                    }`}>{count}</span>
                  </button>
                )
              })}
            </div>

            <div className="flex flex-col divide-y divide-border/40 rounded-xl border border-border/60 bg-card overflow-hidden">
              {filtered.length === 0 ? (
                <div className="flex flex-col items-center gap-2 py-16 text-center">
                  <CheckCircle2 size={32} className="text-muted-foreground/40" />
                  <p className="text-sm font-medium text-foreground">No tasks here</p>
                  <p className="text-xs text-muted-foreground">Tasks in this status will appear here.</p>
                </div>
              ) : filtered.map((task) => (
                <div key={task.title} className="flex items-center gap-4 px-5 py-3.5 hover:bg-muted/20 transition-colors">
                  <Circle size={16} className="shrink-0 text-border" />
                  <div className="flex flex-1 flex-col gap-0.5 min-w-0">
                    <span className="truncate text-sm text-foreground">{task.title}</span>
                    <div className="flex items-center gap-2">
                      <div className={`h-1.5 w-1.5 rounded-full ${task.projectColor}`} />
                      <span className="text-xs text-muted-foreground">{task.project}</span>
                    </div>
                  </div>
                  <div className="flex shrink-0 items-center gap-4">
                    <span className={`text-xs font-medium ${task.priorityColor}`}>{task.priority}</span>
                    <Badge variant="outline" className="text-[10px]">{task.status}</Badge>
                    <span className={`text-xs ${task.dueSoon ? 'text-red-400' : 'text-muted-foreground'}`}>{task.due}</span>
                    <Avatar className="h-6 w-6">
                      <AvatarFallback className="bg-rose-500/15 text-[9px] font-semibold text-rose-400">VZ</AvatarFallback>
                    </Avatar>
                  </div>
                </div>
              ))}
            </div>

          </div>
        </main>
      </div>
    </div>
  )
}
