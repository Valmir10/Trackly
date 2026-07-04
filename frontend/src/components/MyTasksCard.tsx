import { Link } from 'react-router-dom'
import { ArrowRight, Circle, CheckCircle2 } from 'lucide-react'
import '@/styles/MyTasksCard.css'

const tasks = [
  {
    title: 'Design dashboard layout',
    project: 'Frontend redesign',
    projectDot: 'var(--tp-cat-1)',
    priority: 'high' as const,
    due: 'Today',
    dueSoon: true,
    status: 'In Progress',
  },
  {
    title: 'Write API documentation',
    project: 'API v2',
    projectDot: 'var(--tp-cat-3)',
    priority: 'medium' as const,
    due: 'Tomorrow',
    dueSoon: false,
    status: 'To Do',
  },
  {
    title: 'Fix authentication bug',
    project: 'API v2',
    projectDot: 'var(--tp-cat-3)',
    priority: 'high' as const,
    due: 'Jun 12',
    dueSoon: false,
    status: 'In Progress',
  },
  {
    title: 'Update onboarding copy',
    project: 'Marketing site',
    projectDot: 'var(--tp-cat-2)',
    priority: 'low' as const,
    due: 'Jun 15',
    dueSoon: false,
    status: 'To Do',
  },
]

const priorityLabel = { high: 'High', medium: 'Medium', low: 'Low' }

export default function MyTasksCard() {
  return (
    <div className="tp-tasks-card">
      <div className="tp-tasks-card__head">
        <h2 className="tp-tasks-card__title">My tasks</h2>
        <Link to="/acme-corp/projects" className="tp-tasks-card__view-all">
          View all
          <ArrowRight size={12} />
        </Link>
      </div>

      {tasks.length === 0 ? (
        <div className="tp-tasks-card__empty">
          <CheckCircle2 size={24} />
          <p>All caught up</p>
          <span>New tasks assigned to you will show up here.</span>
        </div>
      ) : (
        <div className="tp-tasks-card__list">
          {tasks.map((task) => (
            <div key={task.title} className="tp-tasks-card__row">
              <Circle size={14} className="tp-tasks-card__checkbox" />
              <div className="tp-tasks-card__info">
                <span className="tp-tasks-card__title-text">{task.title}</span>
                <div className="tp-tasks-card__project">
                  <span className="tp-tasks-card__project-dot" style={{ background: task.projectDot }} />
                  {task.project}
                </div>
              </div>
              <div className="tp-tasks-card__meta">
                <span className={`tp-priority tp-priority--${task.priority}`}>
                  {priorityLabel[task.priority]}
                </span>
                <span className={task.dueSoon ? 'tp-tasks-card__due tp-tasks-card__due--soon' : 'tp-tasks-card__due'}>
                  {task.due}
                </span>
                <span className="tp-tasks-card__status">{task.status}</span>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}
