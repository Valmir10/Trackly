import { Link, useParams } from 'react-router-dom'
import { ArrowRight, CheckCircle2 } from 'lucide-react'
import { useProjects } from '@/hooks/useProjects'
import { useWorkspaceTickets } from '@/hooks/useWorkspaceTickets'
import { useWorkspaceScope } from '@/store/useWorkspaceScope'
import { computePulseSignals } from '@/utils/pulse'
import '@/styles/PulseFeed.css'

// Replaces the old "Weekly summary" prose card. Each row is a real query
// result, not generated narrative — severity, one-line statement, and a
// link straight to the place that explains it.
export default function PulseFeed() {
  const { slug = '' } = useParams()
  const { data: projects = [] } = useProjects()
  const { data: tickets = [] } = useWorkspaceTickets()
  const scope = useWorkspaceScope((s) => s.scope)

  const signals = computePulseSignals(tickets, projects, scope, slug)

  return (
    <div className="tp-pulse">
      <div className="tp-pulse__head">
        <span className="tp-pulse__title">Pulse</span>
      </div>

      {signals.length === 0 ? (
        <div className="tp-pulse__empty">
          <CheckCircle2 size={18} />
          <p>Nothing needs your attention right now.</p>
        </div>
      ) : (
        <div className="tp-pulse__list">
          {signals.map((signal) => (
            <Link key={signal.id} to={signal.to} className="tp-pulse__row">
              <span className={`tp-pulse__dot tp-pulse__dot--${signal.severity}`} />
              <span className="tp-pulse__message">{signal.message}</span>
              <ArrowRight size={13} className="tp-pulse__arrow" />
            </Link>
          ))}
        </div>
      )}
    </div>
  )
}
