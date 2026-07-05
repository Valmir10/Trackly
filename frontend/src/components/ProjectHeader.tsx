import { SlidersHorizontal, LayoutList, Kanban, Plus, MessageSquare } from 'lucide-react'
import { WORKSPACE_MEMBERS } from '@/data/users'
import '@/styles/ProjectHeader.css'

interface ProjectHeaderProps {
  name: string
  dotColor: string
  chatOpen: boolean
  onToggleChat: () => void
}

export default function ProjectHeader({ name, dotColor, chatOpen, onToggleChat }: ProjectHeaderProps) {
  return (
    <div className="tp-project-header">
      <div className="tp-project-header__left">
        <div className="tp-project-header__name">
          <span className="tp-project-header__dot" style={{ background: dotColor }} />
          <h1>{name}</h1>
        </div>

        <span className="tp-project-header__divider" />

        <div className="tp-project-header__members">
          {WORKSPACE_MEMBERS.map((m) => (
            <span key={m.id} className="tp-avatar tp-avatar--stacked">
              {m.initials}
            </span>
          ))}
          <button type="button" className="tp-project-header__add-member" aria-label="Add member">
            +
          </button>
        </div>
      </div>

      <div className="tp-project-header__right">
        <div className="tp-segmented">
          <button type="button" className="tp-segmented__item tp-segmented__item--active">
            <Kanban size={13} />
            Board
          </button>
          <button type="button" className="tp-segmented__item">
            <LayoutList size={13} />
            List
          </button>
        </div>

        <button type="button" className="tp-btn tp-btn--secondary tp-btn--sm">
          <SlidersHorizontal size={13} />
          Filter
        </button>

        <button type="button" className="tp-btn tp-btn--primary tp-btn--sm">
          <Plus size={13} />
          Add task
        </button>

        <button
          type="button"
          className={`tp-btn tp-btn--secondary tp-btn--sm${chatOpen ? ' tp-btn--active' : ''}`}
          onClick={onToggleChat}
          aria-pressed={chatOpen}
        >
          <MessageSquare size={13} />
          Chat
        </button>
      </div>
    </div>
  )
}
