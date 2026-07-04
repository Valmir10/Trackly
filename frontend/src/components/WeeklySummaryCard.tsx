import { RefreshCw } from 'lucide-react'
import '@/styles/WeeklySummaryCard.css'

const tags = [
  { label: '8 tasks done', tone: 'neutral' as const },
  { label: '3 in review', tone: 'neutral' as const },
  { label: '2 overdue risk', tone: 'danger' as const },
]

export default function WeeklySummaryCard() {
  return (
    <div className="tp-summary-card">
      <div className="tp-summary-card__head">
        <span className="tp-summary-card__title">Weekly summary</span>
        <div className="tp-summary-card__head-actions">
          <span className="tp-summary-card__range">Jun 3 – Jun 9</span>
          <button type="button" className="tp-summary-card__refresh" aria-label="Refresh summary">
            <RefreshCw size={12} />
          </button>
        </div>
      </div>

      <p className="tp-summary-card__body">
        Your team had a productive week. <strong>8 tasks completed</strong>, up from 5 last week.
        The frontend redesign project is on track with the dashboard layout moving to review.
        However, <strong>2 high-priority tasks</strong> in the API v2 project are approaching their deadlines.
        Consider reviewing the authentication bug fix before end of day.
      </p>

      <div className="tp-summary-card__tags">
        {tags.map((tag) => (
          <span key={tag.label} className={`tp-summary-card__tag tp-summary-card__tag--${tag.tone}`}>
            {tag.label}
          </span>
        ))}
      </div>
    </div>
  )
}
