import { Building2, Users, CreditCard, Key, Webhook, Bell, UserCircle, Shield } from 'lucide-react'
import { cn } from '@/lib/utils'

const sections = [
  { label: 'Workspace', icon: Building2 },
  { label: 'Team members', icon: Users },
  { label: 'Billing', icon: CreditCard },
  { label: 'API keys', icon: Key },
  { label: 'Webhooks', icon: Webhook },
  { label: 'Notifications', icon: Bell },
  { label: 'Profile', icon: UserCircle },
  { label: 'Security', icon: Shield },
]

interface SettingsSidebarProps {
  active: string
  onSelect: (label: string) => void
}

export default function SettingsSidebar({ active, onSelect }: SettingsSidebarProps) {
  return (
    <nav className="w-52 shrink-0 border-r border-border/60 py-4">
      <p className="mb-1 px-4 text-[11px] font-semibold uppercase tracking-wider text-muted-foreground">
        Settings
      </p>
      <div className="flex flex-col gap-0.5 px-2">
        {sections.map(({ label, icon: Icon }) => (
          <button
            key={label}
            onClick={() => onSelect(label)}
            className={cn(
              'flex items-center gap-2.5 rounded-md px-3 py-2 text-sm transition-colors text-left',
              active === label
                ? 'bg-accent text-foreground font-medium'
                : 'text-muted-foreground hover:bg-accent/50 hover:text-foreground'
            )}
          >
            <Icon size={15} />
            {label}
          </button>
        ))}
      </div>
    </nav>
  )
}
