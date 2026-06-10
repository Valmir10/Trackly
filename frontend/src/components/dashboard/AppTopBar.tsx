import { Search, Bell, HelpCircle } from 'lucide-react'
import { Avatar, AvatarFallback } from '@/components/ui/avatar'

export default function AppTopBar() {
  return (
    <header className="flex h-14 shrink-0 items-center justify-between border-b border-border/60 bg-background px-6">
      <div className="flex items-center gap-2 rounded-md border border-input bg-muted/40 px-3 py-1.5 w-64">
        <Search size={14} className="text-muted-foreground" />
        <span className="flex-1 text-sm text-muted-foreground">Search...</span>
        <kbd className="rounded border border-border bg-background px-1.5 py-0.5 text-[10px] text-muted-foreground">
          ⌘K
        </kbd>
      </div>

      <div className="flex items-center gap-1">
        <button className="relative flex h-8 w-8 items-center justify-center rounded-md text-muted-foreground hover:bg-muted hover:text-foreground transition-colors">
          <Bell size={15} />
          <span className="absolute right-1.5 top-1.5 h-1.5 w-1.5 rounded-full bg-violet-500" />
        </button>
        <button className="flex h-8 w-8 items-center justify-center rounded-md text-muted-foreground hover:bg-muted hover:text-foreground transition-colors">
          <HelpCircle size={15} />
        </button>
        <div className="ml-1">
          <Avatar className="h-7 w-7 cursor-pointer">
            <AvatarFallback className="bg-violet-500/15 text-xs font-medium text-violet-400">VZ</AvatarFallback>
          </Avatar>
        </div>
      </div>
    </header>
  )
}
