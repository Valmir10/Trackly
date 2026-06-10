import { Plus } from 'lucide-react'
import type { ReactNode } from 'react'

interface KanbanColumnProps {
  title: string
  color: string
  count: number
  children: ReactNode
}

export default function KanbanColumn({ title, color, count, children }: KanbanColumnProps) {
  return (
    <div className="flex w-72 shrink-0 flex-col rounded-xl border border-border/60 bg-muted/20">
      <div className="flex items-center justify-between px-4 py-3">
        <div className="flex items-center gap-2">
          <div className={`h-2 w-2 rounded-full ${color}`} />
          <span className="text-xs font-semibold text-foreground">{title}</span>
          <span className="rounded-md bg-muted px-1.5 py-0.5 text-[10px] font-medium text-muted-foreground">
            {count}
          </span>
        </div>
        <button className="flex h-6 w-6 items-center justify-center rounded-md text-muted-foreground hover:bg-muted hover:text-foreground transition-colors">
          <Plus size={14} />
        </button>
      </div>

      <div className="flex flex-col gap-2 overflow-y-auto px-3 pb-3" style={{ maxHeight: 'calc(100vh - 220px)' }}>
        {children}
        <button className="mt-1 flex items-center gap-1.5 rounded-lg border border-dashed border-border/60 px-3 py-2.5 text-xs text-muted-foreground hover:border-border hover:text-foreground transition-colors">
          <Plus size={12} />
          Add task
        </button>
      </div>
    </div>
  )
}
