import { useWorkspaceTickets } from '@/hooks/useWorkspaceTickets'
import { isOverdue, isStale } from '@/utils/staleness'
import '@/styles/SuggestedAgendaPanel.css'

interface SuggestedAgendaPanelProps {
  projectId: string
  onAddToNotes: (ticketId: string, reason: string) => void
}

// Self-populating suggestions driven by current board queries — overdue and
// stale tickets in this meeting's project. "Rolled forward from the
// previous meeting" is deliberately out of scope: it needs meeting-history
// tracking that isn't worth building until there's real history to roll
// forward from.
export default function SuggestedAgendaPanel({ projectId, onAddToNotes }: SuggestedAgendaPanelProps) {
  const { data: tickets = [] } = useWorkspaceTickets()
  const projectTickets = tickets.filter((t) => t.projectId === projectId)

  const suggestions = projectTickets
    .filter((t) => isOverdue(t) || isStale(t))
    .map((t) => ({ ticketId: t.id, title: t.title, reason: isOverdue(t) ? 'Overdue' : 'No update in 5+ days' }))
    .slice(0, 5)

  if (suggestions.length === 0) return null

  return (
    <div className="tp-suggested-agenda">
      <span className="tp-label">Suggested agenda</span>
      <div className="tp-suggested-agenda__list">
        {suggestions.map((s) => (
          <div key={s.ticketId} className="tp-suggested-agenda__row">
            <div className="tp-suggested-agenda__info">
              <span className="tp-suggested-agenda__title">{s.title}</span>
              <span className="tp-suggested-agenda__reason">{s.reason}</span>
            </div>
            <button
              type="button"
              className="tp-btn tp-btn--secondary tp-btn--sm"
              onClick={() => onAddToNotes(s.ticketId, s.reason)}
            >
              Add to notes
            </button>
          </div>
        ))}
      </div>
    </div>
  )
}
