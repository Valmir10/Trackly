import { Link, useLocation } from 'react-router-dom'
import {
  LayoutDashboard,
  FolderKanban,
  CheckSquare,
  BarChart2,
  Settings,
  Plus,
} from 'lucide-react'
import { Avatar, AvatarFallback } from '@/components/ui/avatar'
import { cn } from '@/lib/utils'

const slug = 'acme-corp'

const navItems = [
  { label: 'Dashboard', icon: LayoutDashboard, to: `/${slug}/dashboard` },
  { label: 'Projects', icon: FolderKanban, to: `/${slug}/projects` },
  { label: 'My Tasks', icon: CheckSquare, to: `/${slug}/tasks` },
  { label: 'Analytics', icon: BarChart2, to: `/${slug}/analytics` },
]

const projects = [
  { name: 'Frontend redesign', color: 'bg-rose-500' },
  { name: 'API v2', color: 'bg-blue-500' },
  { name: 'Mobile app', color: 'bg-green-500' },
  { name: 'Marketing site', color: 'bg-orange-500' },
]

export default function AppSidebar() {
  const location = useLocation()

  return (
    <aside className="flex h-screen w-60 shrink-0 flex-col border-r border-sidebar-border bg-sidebar">
      <div className="flex h-14 items-center gap-2.5 border-b border-sidebar-border px-4">
        <div className="flex h-6 w-6 items-center justify-center rounded-md bg-rose-600">
          <span className="text-[10px] font-bold text-white">T</span>
        </div>
        <span className="text-sm font-semibold text-sidebar-foreground tracking-tight">Trackly</span>
        <span className="ml-auto rounded-md bg-rose-500/15 px-1.5 py-0.5 text-[10px] font-medium text-rose-400">
          Pro
        </span>
      </div>

      <nav className="flex flex-col gap-0.5 p-2">
        {navItems.map((item) => {
          const Icon = item.icon
          const isActive = location.pathname === item.to
          return (
            <Link
              key={item.label}
              to={item.to}
              className={cn(
                'flex items-center gap-2.5 rounded-md px-3 py-2 text-sm transition-colors',
                isActive
                  ? 'bg-accent text-foreground font-medium'
                  : 'text-muted-foreground hover:bg-accent/50 hover:text-foreground'
              )}
            >
              <Icon size={15} />
              {item.label}
            </Link>
          )
        })}
      </nav>

      <div className="mx-2 mt-4 border-t border-sidebar-border pt-4">
        <div className="mb-1.5 flex items-center justify-between px-3">
          <span className="text-[11px] font-semibold uppercase tracking-wider text-muted-foreground">
            Projects
          </span>
          <button className="rounded-md p-0.5 text-muted-foreground hover:text-foreground transition-colors">
            <Plus size={13} />
          </button>
        </div>
        <div className="flex flex-col gap-0.5">
          {projects.map((project) => (
            <Link
              key={project.name}
              to={`/${slug}/projects/1`}
              className="flex items-center gap-2.5 rounded-md px-3 py-1.5 text-sm text-muted-foreground hover:bg-accent/50 hover:text-foreground transition-colors"
            >
              <div className={`h-2 w-2 shrink-0 rounded-full ${project.color}`} />
              <span className="truncate">{project.name}</span>
            </Link>
          ))}
        </div>
      </div>

      <div className="mt-auto border-t border-sidebar-border p-3">
        <Link
          to={`/${slug}/settings`}
          className="flex items-center gap-2 rounded-md px-2 py-1.5 text-sm text-muted-foreground hover:bg-accent/50 hover:text-foreground transition-colors"
        >
          <Settings size={14} />
          Settings
        </Link>
        <div className="mt-2 flex items-center gap-2.5 rounded-md px-2 py-1.5">
          <Avatar className="h-7 w-7">
            <AvatarFallback className="bg-rose-500/15 text-xs font-medium text-rose-400">VZ</AvatarFallback>
          </Avatar>
          <div className="flex flex-col">
            <span className="text-xs font-medium text-foreground">Valmir Zogaj</span>
            <span className="text-[10px] text-muted-foreground">Owner</span>
          </div>
        </div>
      </div>
    </aside>
  )
}
