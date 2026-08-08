import { Calendar, MessageSquare, Paperclip } from 'lucide-react'
import '@/styles/TaskCard.css'

export interface Task {
  id: string
  title: string
  tag?: string
  priority: 'high' | 'medium' | 'low'
  assignee?: { initials: string }
  dueDate?: string
  dueSoon?: boolean
  commentCount?: number
  attachmentCount?: number
  description?: string
}

interface TaskCardProps {
  task: Task
  onOpen: (taskId: string) => void
}

const priorityLabel = { high: 'High', medium: 'Medium', low: 'Low' }

export default function TaskCard({ task, onOpen }: TaskCardProps) {
  return (
    <button type="button" className="tp-task-card" onClick={() => onOpen(task.id)}>
      <div className="tp-task-card__head">
        <p className="tp-task-card__title">{task.title}</p>
        <span
          className={`tp-task-card__priority-dot tp-priority--${task.priority}`}
          aria-label={`${priorityLabel[task.priority]} priority`}
        />
      </div>

      {task.tag && <span className="tp-task-card__tag">{task.tag}</span>}

      <div className="tp-task-card__footer">
        {task.assignee && <span className="tp-avatar tp-avatar--sm">{task.assignee.initials}</span>}

        <div className="tp-task-card__meta">
          {!!task.commentCount && (
            <span className="tp-task-card__meta-item">
              <MessageSquare size={10} />
              {task.commentCount}
            </span>
          )}
          {!!task.attachmentCount && (
            <span className="tp-task-card__meta-item">
              <Paperclip size={10} />
              {task.attachmentCount}
            </span>
          )}
          {task.dueDate && (
            <span className={`tp-task-card__meta-item${task.dueSoon ? ' tp-task-card__meta-item--due' : ''}`}>
              <Calendar size={10} />
              {task.dueDate}
            </span>
          )}
        </div>
      </div>
    </button>
  )
}
