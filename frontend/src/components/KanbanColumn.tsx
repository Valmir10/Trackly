import { Plus } from 'lucide-react'
import type { ReactNode } from 'react'
import '@/styles/KanbanColumn.css'

interface KanbanColumnProps {
  title: string
  color: string
  count: number
  children: ReactNode
}

export default function KanbanColumn({ title, color, count, children }: KanbanColumnProps) {
  return (
    <div className="tp-kanban-column">
      <div className="tp-kanban-column__head">
        <div className="tp-kanban-column__label">
          <span className="tp-kanban-column__dot" style={{ background: color }} />
          <span>{title}</span>
          <span className="tp-kanban-column__count">{count}</span>
        </div>
        <button type="button" className="tp-kanban-column__add" aria-label={`Add task to ${title}`}>
          <Plus size={14} />
        </button>
      </div>

      <div className="tp-kanban-column__cards">
        {children}
        <button type="button" className="tp-kanban-column__add-task">
          <Plus size={12} />
          Add task
        </button>
      </div>
    </div>
  )
}
