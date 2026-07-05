import { useRef, useState } from 'react'
import { Link, useLocation } from 'react-router-dom'
import {
  LayoutDashboard,
  FolderKanban,
  CheckSquare,
  BarChart2,
  Settings,
  Plus,
  ChevronsUpDown,
  Check,
  X,
} from 'lucide-react'
import { useClickOutside } from '@/hooks/useClickOutside'
import { PROJECTS } from '@/data/projects'
import '@/styles/AppSidebar.css'

const slug = 'acme-corp'

const navItems = [
  { label: 'Dashboard', icon: LayoutDashboard, to: `/${slug}/dashboard` },
  { label: 'Projects', icon: FolderKanban, to: `/${slug}/projects` },
  { label: 'My Tasks', icon: CheckSquare, to: `/${slug}/tasks` },
  { label: 'Analytics', icon: BarChart2, to: `/${slug}/analytics` },
]

interface AppSidebarProps {
  mobileOpen: boolean
  onCloseMobile: () => void
}

export default function AppSidebar({ mobileOpen, onCloseMobile }: AppSidebarProps) {
  const location = useLocation()
  const [switcherOpen, setSwitcherOpen] = useState(false)
  const switcherRef = useRef<HTMLDivElement>(null)
  useClickOutside(switcherRef, () => setSwitcherOpen(false), switcherOpen)

  const content = (
    <>
      <div className="tp-sidebar__header">
        <div className="tp-dropdown" ref={switcherRef}>
          <button
            type="button"
            className="tp-sidebar__workspace"
            onClick={() => setSwitcherOpen((open) => !open)}
            aria-expanded={switcherOpen}
          >
            <span className="tp-sidebar__workspace-mark">A</span>
            <span className="tp-sidebar__workspace-info">
              <span className="tp-sidebar__workspace-name">Acme Corp</span>
              <span className="tp-sidebar__workspace-tier">Starter</span>
            </span>
            <ChevronsUpDown size={14} />
          </button>

          {switcherOpen && (
            <div className="tp-dropdown__panel tp-sidebar__workspace-panel">
              <p className="tp-menu-label">Workspace</p>
              <div className="tp-menu-item tp-menu-item--active">
                <Check size={14} />
                Acme Corp
              </div>
              <div className="tp-menu-divider" />
              <Link
                to={`/${slug}/settings`}
                className="tp-menu-item"
                onClick={() => setSwitcherOpen(false)}
              >
                <Settings size={14} />
                Workspace settings
              </Link>
            </div>
          )}
        </div>
      </div>

      <nav className="tp-sidebar__nav">
        {navItems.map((item) => {
          const Icon = item.icon
          const isActive = location.pathname === item.to
          return (
            <Link
              key={item.label}
              to={item.to}
              onClick={onCloseMobile}
              className={`tp-sidebar__nav-item${isActive ? ' tp-sidebar__nav-item--active' : ''}`}
            >
              <Icon size={16} />
              {item.label}
            </Link>
          )
        })}
      </nav>

      <div className="tp-sidebar__projects">
        <div className="tp-sidebar__projects-head">
          <span>Projects</span>
          <button type="button" className="tp-sidebar__projects-add" aria-label="New project">
            <Plus size={13} />
          </button>
        </div>
        <div className="tp-sidebar__projects-list">
          {PROJECTS.map((project) => (
            <Link
              key={project.name}
              to={`/${slug}/projects/${project.id}`}
              onClick={onCloseMobile}
              className="tp-sidebar__project"
            >
              <span className="tp-sidebar__project-dot" style={{ background: project.dot }} />
              <span className="tp-sidebar__project-name">{project.name}</span>
            </Link>
          ))}
        </div>
      </div>

      <div className="tp-sidebar__footer">
        <Link to={`/${slug}/settings`} onClick={onCloseMobile} className="tp-sidebar__settings-link">
          <Settings size={14} />
          Settings
        </Link>
        <div className="tp-sidebar__user">
          <span className="tp-sidebar__user-avatar">VZ</span>
          <span className="tp-sidebar__user-info">
            <span className="tp-sidebar__user-name">Valmir Zogaj</span>
            <span className="tp-sidebar__user-role">Owner</span>
          </span>
        </div>
      </div>
    </>
  )

  return (
    <>
      <aside className="tp-sidebar tp-sidebar--desktop">{content}</aside>

      {mobileOpen && (
        <div className="tp-sidebar__backdrop" onClick={onCloseMobile}>
          <aside
            className="tp-sidebar tp-sidebar--mobile"
            onClick={(event) => event.stopPropagation()}
          >
            <button
              type="button"
              className="tp-sidebar__close"
              onClick={onCloseMobile}
              aria-label="Close menu"
            >
              <X size={18} />
            </button>
            {content}
          </aside>
        </div>
      )}
    </>
  )
}
