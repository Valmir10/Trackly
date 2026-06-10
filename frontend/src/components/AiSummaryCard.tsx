import { Sparkles, RefreshCw } from 'lucide-react'

export default function AiSummaryCard() {
  return (
    <div className="rounded-xl border border-rose-500/20 bg-rose-500/5 p-5">
      <div className="mb-3 flex items-center justify-between">
        <div className="flex items-center gap-2">
          <Sparkles size={14} className="text-rose-400" />
          <span className="text-sm font-semibold text-foreground">Weekly AI summary</span>
        </div>
        <div className="flex items-center gap-2">
          <span className="text-[10px] text-muted-foreground">Jun 3 – Jun 9</span>
          <button className="flex h-6 w-6 items-center justify-center rounded-md text-muted-foreground hover:text-foreground transition-colors">
            <RefreshCw size={12} />
          </button>
        </div>
      </div>

      <p className="text-sm text-muted-foreground leading-relaxed">
        Your team had a productive week — <span className="text-foreground font-medium">8 tasks completed</span>, up from 5 last week.
        The frontend redesign project is on track with the dashboard layout moving to review.
        However, <span className="text-foreground font-medium">2 high-priority tasks</span> in the API v2 project are approaching their deadlines.
        Consider reviewing the authentication bug fix before end of day.
      </p>

      <div className="mt-4 flex flex-wrap gap-2">
        {['8 tasks done', '3 in review', '2 overdue risk'].map((tag, i) => (
          <span
            key={tag}
            className={`rounded-full px-2.5 py-0.5 text-[11px] font-medium ${
              i === 2
                ? 'bg-red-500/10 text-red-400'
                : 'bg-rose-500/10 text-rose-400'
            }`}
          >
            {tag}
          </span>
        ))}
      </div>
    </div>
  )
}
