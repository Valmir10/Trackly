import {
  Kanban,
  Sparkles,
  Zap,
  Clock,
  BarChart2,
  Webhook,
} from 'lucide-react'

const features = [
  {
    icon: Kanban,
    title: 'Kanban boards',
    description:
      'Visual task management with drag-and-drop cards across customizable columns. Filter, search, and sort with ease.',
  },
  {
    icon: Sparkles,
    title: 'AI assistant',
    description:
      'Break down complex features into actionable tasks, get weekly summaries, and chat with your project in natural language.',
  },
  {
    icon: Zap,
    title: 'Real-time sync',
    description:
      "Every update is instantly reflected for all team members. See who's online, what they're working on, live.",
  },
  {
    icon: Clock,
    title: 'Time tracking',
    description:
      'Start and stop timers directly on any task. Generate time reports per user, project, or billing period.',
  },
  {
    icon: BarChart2,
    title: 'Analytics',
    description:
      'Velocity charts, burndown graphs, and status breakdowns give your team full visibility into progress.',
  },
  {
    icon: Webhook,
    title: 'API & Webhooks',
    description:
      'A versioned public REST API and HMAC-signed webhooks let you plug Trackly into any workflow or tool.',
  },
]

export default function FeaturesSection() {
  return (
    <section id="features" className="py-24">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">

        <div className="mb-16 text-center">
          <p className="mb-3 text-sm font-medium text-violet-400 uppercase tracking-wider">Features</p>
          <h2 className="text-3xl font-semibold tracking-tight text-foreground sm:text-4xl">
            Everything your team needs
          </h2>
          <p className="mt-4 text-muted-foreground max-w-xl mx-auto">
            From planning to delivery, Trackly covers the full lifecycle of your projects — without the bloat.
          </p>
        </div>

        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
          {features.map((feature) => {
            const Icon = feature.icon
            return (
              <div
                key={feature.title}
                className="group rounded-xl border border-border/60 bg-card p-6 hover:border-violet-500/40 hover:bg-card/80 transition-all duration-200"
              >
                <div className="mb-4 flex h-10 w-10 items-center justify-center rounded-lg bg-violet-500/10 text-violet-400 group-hover:bg-violet-500/15 transition-colors">
                  <Icon size={20} />
                </div>
                <h3 className="mb-2 text-sm font-semibold text-foreground">{feature.title}</h3>
                <p className="text-sm text-muted-foreground leading-relaxed">{feature.description}</p>
              </div>
            )
          })}
        </div>

      </div>
    </section>
  )
}
