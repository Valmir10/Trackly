import { Link } from 'react-router-dom'
import { ArrowRight, Circle } from 'lucide-react'
import { Badge } from '@/components/ui/badge'

const tasks = [
  {
    title: 'Design dashboard layout',
    project: 'Frontend redesign',
    projectColor: 'bg-violet-500',
    priority: 'High',
    priorityColor: 'text-red-400',
    due: 'Today',
    dueColor: 'text-red-400',
    status: 'In Progress',
  },
  {
    title: 'Write API documentation',
    project: 'API v2',
    projectColor: 'bg-blue-500',
    priority: 'Medium',
    priorityColor: 'text-yellow-400',
    due: 'Tomorrow',
    dueColor: 'text-muted-foreground',
    status: 'To Do',
  },
  {
    title: 'Fix authentication bug',
    project: 'API v2',
    projectColor: 'bg-blue-500',
    priority: 'High',
    priorityColor: 'text-red-400',
    due: 'Jun 12',
    dueColor: 'text-muted-foreground',
    status: 'In Progress',
  },
  {
    title: 'Update onboarding copy',
    project: 'Marketing site',
    projectColor: 'bg-orange-500',
    priority: 'Low',
    priorityColor: 'text-green-400',
    due: 'Jun 15',
    dueColor: 'text-muted-foreground',
    status: 'To Do',
  },
]

export default function MyTasksCard() {
  return (
    <div className="flex flex-col rounded-xl border border-border/60 bg-card">
      <div className="flex items-center justify-between border-b border-border/40 px-5 py-4">
        <h2 className="text-sm font-semibold text-foreground">My tasks</h2>
        <Link
          to="/acme-corp/projects"
          className="flex items-center gap-1 text-xs text-muted-foreground hover:text-foreground transition-colors"
        >
          View all
          <ArrowRight size={12} />
        </Link>
      </div>

      <div className="flex flex-col divide-y divide-border/40">
        {tasks.map((task) => (
          <div key={task.title} className="flex items-center gap-3 px-5 py-3.5 hover:bg-muted/20 transition-colors">
            <Circle size={15} className="shrink-0 text-border" />
            <div className="flex flex-1 flex-col gap-0.5 min-w-0">
              <span className="truncate text-sm text-foreground">{task.title}</span>
              <div className="flex items-center gap-2">
                <div className={`h-1.5 w-1.5 rounded-full ${task.projectColor}`} />
                <span className="text-xs text-muted-foreground">{task.project}</span>
              </div>
            </div>
            <div className="flex shrink-0 items-center gap-3">
              <span className={`text-xs font-medium ${task.priorityColor}`}>{task.priority}</span>
              <span className={`text-xs ${task.dueColor}`}>{task.due}</span>
              <Badge variant="outline" className="text-[10px]">{task.status}</Badge>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}
