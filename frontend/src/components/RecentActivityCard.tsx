import { Avatar, AvatarFallback } from '@/components/ui/avatar'

const activities = [
  {
    user: { name: 'Sarah K.', initials: 'SK', color: 'bg-blue-500/15 text-blue-400' },
    action: 'moved',
    target: 'Design dashboard layout',
    detail: 'to In Review',
    time: '5m ago',
  },
  {
    user: { name: 'John S.', initials: 'JS', color: 'bg-green-500/15 text-green-400' },
    action: 'completed',
    target: 'CI/CD pipeline setup',
    detail: '',
    time: '1h ago',
  },
  {
    user: { name: 'You', initials: 'VZ', color: 'bg-rose-500/15 text-rose-400' },
    action: 'commented on',
    target: 'Fix authentication bug',
    detail: '',
    time: '2h ago',
  },
  {
    user: { name: 'Sarah K.', initials: 'SK', color: 'bg-blue-500/15 text-blue-400' },
    action: 'created',
    target: 'Update onboarding copy',
    detail: 'in Marketing site',
    time: '3h ago',
  },
  {
    user: { name: 'John S.', initials: 'JS', color: 'bg-green-500/15 text-green-400' },
    action: 'assigned',
    target: 'Write API documentation',
    detail: 'to you',
    time: '5h ago',
  },
]

export default function RecentActivityCard() {
  return (
    <div className="flex flex-col rounded-xl border border-border/60 bg-card">
      <div className="border-b border-border/40 px-5 py-4">
        <h2 className="text-sm font-semibold text-foreground">Recent activity</h2>
      </div>

      <div className="flex flex-col gap-0 divide-y divide-border/40">
        {activities.map((activity, i) => (
          <div key={i} className="flex items-start gap-3 px-5 py-3.5">
            <Avatar className="mt-0.5 h-6 w-6 shrink-0">
              <AvatarFallback className={`text-[10px] font-medium ${activity.user.color}`}>
                {activity.user.initials}
              </AvatarFallback>
            </Avatar>
            <div className="flex flex-1 flex-wrap items-baseline gap-x-1 text-xs">
              <span className="font-medium text-foreground">{activity.user.name}</span>
              <span className="text-muted-foreground">{activity.action}</span>
              <span className="font-medium text-foreground">{activity.target}</span>
              {activity.detail && (
                <span className="text-muted-foreground">{activity.detail}</span>
              )}
            </div>
            <span className="shrink-0 text-[10px] text-muted-foreground">{activity.time}</span>
          </div>
        ))}
      </div>
    </div>
  )
}
