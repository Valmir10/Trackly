import { useEffect, useRef, useState } from 'react'
import { X, Calendar, ChevronsUpDown, Check, Clock } from 'lucide-react'
import { useClickOutside } from '@/hooks/useClickOutside'
import ChatThread from '@/components/ChatThread'
import type { Task } from '@/components/TaskCard'
import '@/styles/TicketModal.css'

interface TimeEntry {
  id: string
  hours: string
  note: string
}

interface TicketModalProps {
  task: Task
  projectId: string
  statuses: string[]
  currentStatus: string
  onStatusChange: (status: string) => void
  onClose: () => void
  onOpenTicket: (ticketId: string) => void
}

const priorityLabel = { high: 'High', medium: 'Medium', low: 'Low' }

export default function TicketModal({
  task,
  projectId,
  statuses,
  currentStatus,
  onStatusChange,
  onClose,
  onOpenTicket,
}: TicketModalProps) {
  const [statusMenuOpen, setStatusMenuOpen] = useState(false)
  const statusRef = useRef<HTMLDivElement>(null)
  useClickOutside(statusRef, () => setStatusMenuOpen(false), statusMenuOpen)

  const [entries, setEntries] = useState<TimeEntry[]>([])
  const [hours, setHours] = useState('')
  const [note, setNote] = useState('')

  useEffect(() => {
    function onKeyDown(e: KeyboardEvent) {
      if (e.key === 'Escape') onClose()
    }
    document.addEventListener('keydown', onKeyDown)
    return () => document.removeEventListener('keydown', onKeyDown)
  }, [onClose])

  function handleLogTime(e: React.FormEvent) {
    e.preventDefault()
    if (!hours.trim()) return
    setEntries((prev) => [...prev, { id: `${Date.now()}`, hours: hours.trim(), note: note.trim() }])
    setHours('')
    setNote('')
  }

  return (
    <div className="tp-modal-overlay" onClick={onClose}>
      <div className="tp-modal" onClick={(e) => e.stopPropagation()}>
        <div className="tp-modal__header">
          <div>
            <span className="tp-ticket-modal__id">#{task.id}</span>
            <h2 className="tp-modal__title">{task.title}</h2>
          </div>
          <button type="button" className="tp-modal__close" onClick={onClose} aria-label="Close">
            <X size={16} />
          </button>
        </div>

        <div className="tp-modal__body">
          <div className="tp-ticket-modal__meta">
            <span className={`tp-priority tp-priority--${task.priority}`}>{priorityLabel[task.priority]}</span>
            {task.tag && <span className="tp-ticket-modal__tag">{task.tag}</span>}
            {task.dueDate && (
              <span className={`tp-ticket-modal__due${task.dueSoon ? ' tp-ticket-modal__due--soon' : ''}`}>
                <Calendar size={12} />
                {task.dueDate}
              </span>
            )}
          </div>

          {task.description && <p className="tp-ticket-modal__description">{task.description}</p>}

          <div className="tp-ticket-modal__row">
            <span className="tp-label">Assignee</span>
            <div className="tp-ticket-modal__assignee">
              {task.assignee ? (
                <>
                  <span className="tp-avatar tp-avatar--sm">{task.assignee.initials}</span>
                  {task.assignee.initials}
                </>
              ) : (
                <span className="tp-ticket-modal__unassigned">Unassigned</span>
              )}
            </div>
          </div>

          <div className="tp-ticket-modal__row">
            <span className="tp-label">Status</span>
            <div className="tp-dropdown" ref={statusRef}>
              <button
                type="button"
                className="tp-ticket-modal__status-trigger"
                onClick={() => setStatusMenuOpen((open) => !open)}
                aria-expanded={statusMenuOpen}
              >
                {currentStatus}
                <ChevronsUpDown size={13} />
              </button>
              {statusMenuOpen && (
                <div className="tp-dropdown__panel tp-ticket-modal__status-panel">
                  {statuses.map((status) => (
                    <button
                      key={status}
                      type="button"
                      className={`tp-menu-item${status === currentStatus ? ' tp-menu-item--active' : ''}`}
                      onClick={() => {
                        onStatusChange(status)
                        setStatusMenuOpen(false)
                      }}
                    >
                      {status === currentStatus && <Check size={14} />}
                      {status}
                    </button>
                  ))}
                </div>
              )}
            </div>
          </div>

          <div className="tp-ticket-modal__timelog">
            <span className="tp-label">Time log</span>

            {entries.length > 0 && (
              <ul className="tp-ticket-modal__entries">
                {entries.map((entry) => (
                  <li key={entry.id}>
                    <Clock size={12} />
                    <span className="tp-ticket-modal__entry-hours">{entry.hours}h</span>
                    {entry.note && <span className="tp-ticket-modal__entry-note">{entry.note}</span>}
                  </li>
                ))}
              </ul>
            )}

            <form className="tp-ticket-modal__log-form" onSubmit={handleLogTime}>
              <input
                type="number"
                min="0"
                step="0.5"
                inputMode="decimal"
                placeholder="Hours"
                className="tp-input tp-ticket-modal__hours-input"
                value={hours}
                onChange={(e) => setHours(e.target.value)}
              />
              <input
                type="text"
                placeholder="What did you work on? (optional)"
                className="tp-input"
                value={note}
                onChange={(e) => setNote(e.target.value)}
              />
              <button type="submit" className="tp-btn tp-btn--secondary tp-btn--sm">
                Log time
              </button>
            </form>
          </div>

          <div className="tp-ticket-modal__comments">
            <span className="tp-label">Comments</span>
            <div className="tp-ticket-modal__comments-thread">
              <ChatThread scope={{ type: 'ticket', projectId, ticketId: task.id }} onOpenTicket={onOpenTicket} />
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
