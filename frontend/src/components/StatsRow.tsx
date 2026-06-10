import { CheckCircle2, Clock, ListTodo, Users } from 'lucide-react'

const stats = [
  {
    label: 'Open tasks',
    value: '12',
    change: '+2 this week',
    changePositive: false,
    icon: ListTodo,
    iconColor: 'text-blue-400',
    iconBg: 'bg-blue-500/10',
  },
  {
    label: 'In progress',
    value: '5',
    change: '3 due soon',
    changePositive: false,
    icon: Clock,
    iconColor: 'text-yellow-400',
    iconBg: 'bg-yellow-500/10',
  },
  {
    label: 'Completed this week',
    value: '8',
    change: '+3 vs last week',
    changePositive: true,
    icon: CheckCircle2,
    iconColor: 'text-green-400',
    iconBg: 'bg-green-500/10',
  },
  {
    label: 'Team members',
    value: '6',
    change: '4 online now',
    changePositive: true,
    icon: Users,
    iconColor: 'text-rose-400',
    iconBg: 'bg-rose-500/10',
  },
]

export default function StatsRow() {
  return (
    <div className="grid grid-cols-2 gap-4 xl:grid-cols-4">
      {stats.map((stat) => {
        const Icon = stat.icon
        return (
          <div key={stat.label} className="rounded-xl border border-border/60 bg-card p-5">
            <div className="flex items-start justify-between">
              <div>
                <p className="text-xs text-muted-foreground">{stat.label}</p>
                <p className="mt-1.5 text-2xl font-semibold text-foreground">{stat.value}</p>
              </div>
              <div className={`flex h-9 w-9 items-center justify-center rounded-lg ${stat.iconBg}`}>
                <Icon size={17} className={stat.iconColor} />
              </div>
            </div>
            <p className={`mt-3 text-xs ${stat.changePositive ? 'text-green-400' : 'text-muted-foreground'}`}>
              {stat.change}
            </p>
          </div>
        )
      })}
    </div>
  )
}
