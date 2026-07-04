import '@/styles/RecentActivityCard.css'

const activities = [
  { user: { name: 'Sarah K.', initials: 'SK' }, action: 'moved', target: 'Design dashboard layout', detail: 'to In Review', time: '5m ago' },
  { user: { name: 'John S.', initials: 'JS' }, action: 'completed', target: 'CI/CD pipeline setup', detail: '', time: '1h ago' },
  { user: { name: 'You', initials: 'VZ' }, action: 'commented on', target: 'Fix authentication bug', detail: '', time: '2h ago' },
  { user: { name: 'Sarah K.', initials: 'SK' }, action: 'created', target: 'Update onboarding copy', detail: 'in Marketing site', time: '3h ago' },
  { user: { name: 'John S.', initials: 'JS' }, action: 'assigned', target: 'Write API documentation', detail: 'to you', time: '5h ago' },
]

export default function RecentActivityCard() {
  return (
    <div className="tp-activity-card">
      <div className="tp-activity-card__head">
        <h2 className="tp-activity-card__title">Recent activity</h2>
      </div>

      <div className="tp-activity-card__list">
        {activities.map((activity, i) => (
          <div key={i} className="tp-activity-card__row">
            <span className="tp-avatar">{activity.user.initials}</span>
            <div className="tp-activity-card__text">
              <span className="tp-activity-card__actor">{activity.user.name}</span>{' '}
              <span className="tp-activity-card__action">{activity.action}</span>{' '}
              <span className="tp-activity-card__target">{activity.target}</span>
              {activity.detail && <span className="tp-activity-card__action"> {activity.detail}</span>}
            </div>
            <span className="tp-activity-card__time">{activity.time}</span>
          </div>
        ))}
      </div>
    </div>
  )
}
