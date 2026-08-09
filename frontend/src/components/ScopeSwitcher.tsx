import { useRef, useState } from 'react'
import { Check, ChevronsUpDown, Layers } from 'lucide-react'
import { useClickOutside } from '@/hooks/useClickOutside'
import { useProjects } from '@/hooks/useProjects'
import { useWorkspaceScope } from '@/store/useWorkspaceScope'
import '@/styles/ScopeSwitcher.css'

// Same tp-dropdown primitive as the sidebar's workspace switcher, but this
// one lives in the page header and picks a Scope (all projects, or one) —
// the lens every scope-aware surface (Pulse, Analytics, later ChatThread)
// reads from.
export default function ScopeSwitcher() {
  const { data: projects = [] } = useProjects()
  const scope = useWorkspaceScope((s) => s.scope)
  const setScope = useWorkspaceScope((s) => s.setScope)
  const [open, setOpen] = useState(false)
  const ref = useRef<HTMLDivElement>(null)
  useClickOutside(ref, () => setOpen(false), open)

  const activeProject = scope.type === 'project' ? projects.find((p) => p.id === scope.projectId) : undefined
  const label = scope.type === 'all' ? 'All Projects' : (activeProject?.name ?? 'All Projects')

  return (
    <div className="tp-dropdown tp-scope-switcher" ref={ref}>
      <button
        type="button"
        className="tp-btn tp-btn--secondary tp-btn--sm"
        onClick={() => setOpen((o) => !o)}
        aria-expanded={open}
      >
        <Layers size={13} />
        {label}
        <ChevronsUpDown size={13} />
      </button>

      {open && (
        <div className="tp-dropdown__panel tp-scope-switcher__panel">
          <p className="tp-menu-label">Scope</p>
          <button
            type="button"
            className={`tp-menu-item${scope.type === 'all' ? ' tp-menu-item--active' : ''}`}
            onClick={() => {
              setScope({ type: 'all' })
              setOpen(false)
            }}
          >
            {scope.type === 'all' && <Check size={14} />}
            All Projects
          </button>

          {projects.length > 0 && <div className="tp-menu-divider" />}

          {projects.map((project) => {
            const isActive = scope.type === 'project' && scope.projectId === project.id
            return (
              <button
                key={project.id}
                type="button"
                className={`tp-menu-item${isActive ? ' tp-menu-item--active' : ''}`}
                onClick={() => {
                  setScope({ type: 'project', projectId: project.id })
                  setOpen(false)
                }}
              >
                {isActive && <Check size={14} />}
                {project.name}
              </button>
            )
          })}
        </div>
      )}
    </div>
  )
}
