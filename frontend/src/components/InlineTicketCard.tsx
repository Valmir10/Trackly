import { useTaskStore, findTask } from '@/store/useTaskStore'
import '@/styles/InlineTicketCard.css'

interface InlineTicketCardProps {
  ticketId: string
  onOpenTicket: (ticketId: string) => void
}

// Reads straight from useTaskStore so it re-renders on its own whenever
// applyRemoteStatusChange fires — the same SignalR event the board reacts
// to, no separate wiring needed for this card to feel "live". Selecting
// `columns` (a stable reference until the store actually changes) and
// deriving `findTask` in the render body avoids the infinite-loop trap of
// a selector that returns a brand-new object every call.
export default function InlineTicketCard({ ticketId, onOpenTicket }: InlineTicketCardProps) {
  const columns = useTaskStore((s) => s.columns)
  const found = findTask(columns, ticketId)

  if (!found) {
    return <span className="tp-inline-card tp-inline-card--missing">#{ticketId}</span>
  }

  const { task, column } = found

  return (
    <button type="button" className="tp-inline-card" onClick={() => onOpenTicket(ticketId)}>
      <span className="tp-inline-card__id">#{ticketId}</span>
      <span className="tp-inline-card__title">{task.title}</span>
      <span className="tp-inline-card__status">
        <span className="tp-inline-card__status-dot" style={{ background: column.color }} />
        {column.title}
      </span>
    </button>
  )
}
