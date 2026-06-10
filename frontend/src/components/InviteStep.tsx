import { UserPlus, X } from 'lucide-react'
import { Input } from '@/components/ui/input'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'

const mockInvites = ['sarah@acme.com', 'john@acme.com']

export default function InviteStep() {
  return (
    <div className="flex flex-col gap-5">
      <div className="flex items-center gap-3">
        <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-rose-500/10">
          <UserPlus size={20} className="text-rose-400" />
        </div>
        <div>
          <h2 className="text-base font-semibold text-foreground">Invite your team</h2>
          <p className="text-sm text-muted-foreground">Add teammates to your workspace. You can do this later too.</p>
        </div>
      </div>

      <div className="flex flex-col gap-3">
        <div className="flex gap-2">
          <Input placeholder="colleague@company.com" className="flex-1" />
          <Button variant="outline" size="sm" className="shrink-0">
            Add
          </Button>
        </div>

        {mockInvites.length > 0 && (
          <div className="flex flex-col gap-2">
            <p className="text-xs font-medium text-muted-foreground uppercase tracking-wider">Pending invites</p>
            {mockInvites.map((email) => (
              <div key={email} className="flex items-center justify-between rounded-lg border border-border/60 bg-muted/30 px-3 py-2.5">
                <div className="flex items-center gap-2.5">
                  <div className="flex h-7 w-7 items-center justify-center rounded-full bg-rose-500/15 text-xs font-semibold text-rose-400">
                    {email[0].toUpperCase()}
                  </div>
                  <span className="text-sm text-foreground">{email}</span>
                </div>
                <div className="flex items-center gap-2">
                  <Badge variant="outline" className="text-[10px]">Member</Badge>
                  <button className="text-muted-foreground hover:text-foreground transition-colors">
                    <X size={14} />
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}
