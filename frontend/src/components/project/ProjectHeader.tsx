import { SlidersHorizontal, LayoutList, Kanban, Plus } from 'lucide-react'
import { Avatar, AvatarFallback } from '@/components/ui/avatar'
import { Button } from '@/components/ui/button'

const members = [
  { initials: 'VZ', color: 'bg-violet-500/15 text-violet-400' },
  { initials: 'SK', color: 'bg-blue-500/15 text-blue-400' },
  { initials: 'JS', color: 'bg-green-500/15 text-green-400' },
  { initials: 'AM', color: 'bg-orange-500/15 text-orange-400' },
]

export default function ProjectHeader() {
  return (
    <div className="flex items-center justify-between border-b border-border/60 bg-background px-6 py-3">
      <div className="flex items-center gap-3">
        <div className="flex items-center gap-2">
          <div className="h-3 w-3 rounded-full bg-violet-500" />
          <h1 className="text-sm font-semibold text-foreground">Frontend redesign</h1>
        </div>

        <div className="h-4 w-px bg-border/60" />

        <div className="flex -space-x-1.5">
          {members.map((m, i) => (
            <Avatar key={i} className="h-6 w-6 border-2 border-background">
              <AvatarFallback className={`text-[9px] font-semibold ${m.color}`}>{m.initials}</AvatarFallback>
            </Avatar>
          ))}
          <button className="flex h-6 w-6 items-center justify-center rounded-full border-2 border-background bg-muted text-[10px] text-muted-foreground hover:bg-accent transition-colors">
            +
          </button>
        </div>
      </div>

      <div className="flex items-center gap-2">
        <div className="flex items-center rounded-md border border-border/60 p-0.5">
          <button className="flex items-center gap-1.5 rounded px-2.5 py-1 text-xs font-medium bg-background text-foreground shadow-sm">
            <Kanban size={13} />
            Board
          </button>
          <button className="flex items-center gap-1.5 rounded px-2.5 py-1 text-xs text-muted-foreground hover:text-foreground transition-colors">
            <LayoutList size={13} />
            List
          </button>
        </div>

        <Button variant="outline" size="sm" className="gap-1.5 text-xs">
          <SlidersHorizontal size={13} />
          Filter
        </Button>

        <Button size="sm" className="gap-1.5 bg-violet-600 hover:bg-violet-700 text-white text-xs">
          <Plus size={13} />
          Add task
        </Button>
      </div>
    </div>
  )
}
