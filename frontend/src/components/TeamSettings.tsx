import { Mail, MoreHorizontal } from 'lucide-react'
import { Avatar, AvatarFallback } from '@/components/ui/avatar'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'

const members = [
  { name: 'Valmir Zogaj', email: 'valmir@acme.com', role: 'Owner', initials: 'VZ', color: 'bg-rose-500/15 text-rose-400', joined: 'Jun 1, 2025' },
  { name: 'Sarah Kim', email: 'sarah@acme.com', role: 'Admin', initials: 'SK', color: 'bg-blue-500/15 text-blue-400', joined: 'Jun 3, 2025' },
  { name: 'John Smith', email: 'john@acme.com', role: 'Member', initials: 'JS', color: 'bg-green-500/15 text-green-400', joined: 'Jun 5, 2025' },
  { name: 'Ana Martinez', email: 'ana@acme.com', role: 'Member', initials: 'AM', color: 'bg-orange-500/15 text-orange-400', joined: 'Jun 8, 2025' },
]

const roleStyle: Record<string, string> = {
  Owner: 'border-rose-500/30 text-rose-400',
  Admin: 'border-blue-500/30 text-blue-400',
  Member: 'border-border text-muted-foreground',
}

export default function TeamSettings() {
  return (
    <div className="flex flex-col gap-8 max-w-2xl">
      <div>
        <h2 className="text-base font-semibold text-foreground">Team members</h2>
        <p className="mt-1 text-sm text-muted-foreground">Manage who has access to your workspace.</p>
      </div>

      <div className="flex gap-2">
        <Input placeholder="colleague@company.com" className="max-w-xs" />
        <Button className="gap-2 bg-rose-600 hover:bg-rose-700 text-white shrink-0">
          <Mail size={14} />
          Invite
        </Button>
      </div>

      <div className="flex flex-col divide-y divide-border/40 rounded-xl border border-border/60 overflow-hidden">
        {members.map((member) => (
          <div key={member.email} className="flex items-center gap-4 bg-card px-4 py-3">
            <Avatar className="h-8 w-8 shrink-0">
              <AvatarFallback className={`text-xs font-semibold ${member.color}`}>{member.initials}</AvatarFallback>
            </Avatar>
            <div className="flex-1 min-w-0">
              <p className="text-sm font-medium text-foreground">{member.name}</p>
              <p className="text-xs text-muted-foreground">{member.email}</p>
            </div>
            <span className="text-xs text-muted-foreground hidden sm:block">Joined {member.joined}</span>
            <Badge variant="outline" className={`text-[10px] shrink-0 ${roleStyle[member.role]}`}>
              {member.role}
            </Badge>
            <button className="flex h-7 w-7 items-center justify-center rounded-md text-muted-foreground hover:bg-muted hover:text-foreground transition-colors">
              <MoreHorizontal size={14} />
            </button>
          </div>
        ))}
      </div>
    </div>
  )
}
