import { Link } from 'react-router-dom'
import { ArrowRight, Zap } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'

function KanbanPreview() {
  const columns = [
    {
      title: 'To Do',
      dot: 'bg-slate-400',
      tasks: [
        { title: 'Design system tokens', tag: 'Design', priority: 'High', priorityColor: 'text-red-400' },
        { title: 'Write API docs', tag: 'Backend', priority: 'Medium', priorityColor: 'text-yellow-400' },
      ],
    },
    {
      title: 'In Progress',
      dot: 'bg-blue-400',
      tasks: [
        { title: 'User onboarding flow', tag: 'Frontend', priority: 'High', priorityColor: 'text-red-400' },
        { title: 'Database migrations', tag: 'Backend', priority: 'Low', priorityColor: 'text-green-400' },
      ],
    },
    {
      title: 'Done',
      dot: 'bg-green-400',
      tasks: [
        { title: 'CI/CD pipeline setup', tag: 'DevOps', priority: 'Medium', priorityColor: 'text-yellow-400' },
        { title: 'Auth flow implemented', tag: 'Backend', priority: 'High', priorityColor: 'text-red-400' },
      ],
    },
  ]

  return (
    <div className="relative mx-auto mt-16 max-w-5xl">
      <div className="absolute inset-0 -z-10 rounded-2xl bg-violet-500/10 blur-3xl" />
      <div className="overflow-hidden rounded-xl border border-border/60 bg-card shadow-2xl">
        <div className="flex items-center gap-1.5 border-b border-border/60 bg-muted/30 px-4 py-3">
          <div className="h-2.5 w-2.5 rounded-full bg-red-500/70" />
          <div className="h-2.5 w-2.5 rounded-full bg-yellow-500/70" />
          <div className="h-2.5 w-2.5 rounded-full bg-green-500/70" />
          <div className="ml-4 flex h-5 w-56 items-center rounded-md bg-muted/60 px-2">
            <span className="text-[10px] text-muted-foreground">trackly.app/acme/projects/frontend</span>
          </div>
        </div>

        <div className="flex gap-3 overflow-x-auto p-4">
          {columns.map((col) => (
            <div key={col.title} className="w-60 shrink-0">
              <div className="mb-2 flex items-center gap-2 px-1">
                <div className={`h-2 w-2 rounded-full ${col.dot}`} />
                <span className="text-xs font-medium text-foreground">{col.title}</span>
                <span className="ml-auto text-xs text-muted-foreground">{col.tasks.length}</span>
              </div>
              <div className="flex flex-col gap-2">
                {col.tasks.map((task) => (
                  <div
                    key={task.title}
                    className="rounded-lg border border-border/60 bg-background/60 p-3 hover:border-border transition-colors"
                  >
                    <p className="text-xs font-medium text-foreground leading-snug">{task.title}</p>
                    <div className="mt-2 flex items-center gap-2">
                      <span className="rounded-md bg-muted px-1.5 py-0.5 text-[10px] font-medium text-muted-foreground">
                        {task.tag}
                      </span>
                      <span className={`text-[10px] font-medium ${task.priorityColor}`}>
                        {task.priority}
                      </span>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}

export default function HeroSection() {
  return (
    <section className="relative overflow-hidden pt-32 pb-20">
      <div className="absolute inset-0 -z-10">
        <div className="absolute left-1/2 top-0 h-[500px] w-[800px] -translate-x-1/2 rounded-full bg-violet-600/10 blur-3xl" />
      </div>

      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
        <div className="flex flex-col items-center text-center">

          <Badge
            variant="outline"
            className="mb-6 gap-1.5 border-violet-500/30 bg-violet-500/10 text-violet-400 hover:bg-violet-500/15"
          >
            <Zap size={11} />
            Now with AI-powered task management
          </Badge>

          <h1 className="max-w-3xl text-5xl font-semibold tracking-tight text-foreground sm:text-6xl lg:text-7xl">
            Project management,{' '}
            <span className="bg-gradient-to-r from-violet-400 to-violet-600 bg-clip-text text-transparent">
              reimagined with AI
            </span>
          </h1>

          <p className="mt-6 max-w-2xl text-lg text-muted-foreground leading-relaxed">
            Trackly brings your team's work together — real-time kanban boards,
            AI-powered task breakdowns, and deep analytics. Ship faster, together.
          </p>

          <div className="mt-10 flex flex-wrap items-center justify-center gap-4">
            <Link to="/register">
              <Button size="lg" className="bg-violet-600 hover:bg-violet-700 text-white gap-2 px-6">
                Get started for free
                <ArrowRight size={16} />
              </Button>
            </Link>
            <a href="#pricing">
              <Button size="lg" variant="outline">
                View pricing
              </Button>
            </a>
          </div>

          <p className="mt-4 text-xs text-muted-foreground">
            Free plan available · No credit card required
          </p>
        </div>

        <KanbanPreview />
      </div>
    </section>
  )
}
