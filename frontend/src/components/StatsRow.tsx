import { ListTodo, Clock, CheckCircle2, Users } from 'lucide-react'
import '@/styles/StatsRow.css'

const stats = [
  {
    label: 'Open tasks',
    value: '12',
    change: '+2 this week',
    positive: false,
    icon: ListTodo,
  },
  {
    label: 'In progress',
    value: '5',
    change: '3 due soon',
    positive: false,
    icon: Clock,
  },
  {
    label: 'Completed this week',
    value: '8',
    change: '+3 vs last week',
    positive: true,
    icon: CheckCircle2,
  },
  {
    label: 'Team members',
    value: '6',
    change: '4 online now',
    positive: true,
    icon: Users,
  },
]

export default function StatsRow() {
  return (
    <div className="tp-stats">
      {stats.map((stat) => {
        const Icon = stat.icon
        return (
          <div key={stat.label} className="tp-stat-card">
            <div className="tp-stat-card__head">
              <div>
                <p className="tp-stat-card__label">{stat.label}</p>
                <p className="tp-stat-card__value">{stat.value}</p>
              </div>
              <div className="tp-stat-card__icon">
                <Icon size={16} />
              </div>
            </div>
            <p className={`tp-stat-card__change${stat.positive ? ' tp-stat-card__change--positive' : ''}`}>
              {stat.change}
            </p>
          </div>
        )
      })}
    </div>
  )
}
