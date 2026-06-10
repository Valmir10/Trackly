import { Plus, MoreHorizontal, Key } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'

const keys = [
  { name: 'Production API', scopes: ['read', 'write'], lastUsed: '2 hours ago', created: 'Jun 1, 2025', expires: 'Never' },
  { name: 'CI Integration', scopes: ['read'], lastUsed: '5 days ago', created: 'May 10, 2025', expires: 'Dec 31, 2025' },
]

export default function ApiKeysSettings() {
  return (
    <div className="flex flex-col gap-8 max-w-2xl">
      <div className="flex items-start justify-between">
        <div>
          <h2 className="text-base font-semibold text-foreground">API Keys</h2>
          <p className="mt-1 text-sm text-muted-foreground">Manage API keys for programmatic access to your workspace.</p>
        </div>
        <Button className="gap-2 bg-rose-500 hover:bg-rose-600 text-white shrink-0" size="sm">
          <Plus size={14} />
          New key
        </Button>
      </div>

      <div className="rounded-lg border border-rose-500/20 bg-rose-500/5 px-4 py-3 text-sm text-muted-foreground">
        API keys have full access to your workspace data. Keep them secret and never share them publicly.
      </div>

      <div className="flex flex-col divide-y divide-border/40 rounded-xl border border-border/60 overflow-hidden">
        {keys.map((key) => (
          <div key={key.name} className="flex items-center gap-4 bg-card px-4 py-3">
            <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-lg bg-muted">
              <Key size={14} className="text-muted-foreground" />
            </div>
            <div className="flex-1 min-w-0">
              <p className="text-sm font-medium text-foreground">{key.name}</p>
              <p className="text-xs text-muted-foreground">Last used {key.lastUsed} · Expires {key.expires}</p>
            </div>
            <div className="flex shrink-0 items-center gap-2">
              {key.scopes.map(s => (
                <Badge key={s} variant="outline" className="text-[10px]">{s}</Badge>
              ))}
            </div>
            <button className="flex h-7 w-7 items-center justify-center rounded-md text-muted-foreground hover:bg-muted hover:text-foreground transition-colors">
              <MoreHorizontal size={14} />
            </button>
          </div>
        ))}
      </div>
    </div>
  )
}
