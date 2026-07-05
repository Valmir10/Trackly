// Shared mock project list for the current single-tenant demo workspace.
// AppSidebar and the command palette both read from here so they can't drift.
// All projects currently route to the same project id ("1") since only one
// project has real Kanban data behind it — a pre-existing simplification,
// not something this module introduces.
export interface ProjectSummary {
  id: string
  name: string
  dot: string
}

export const PROJECTS: ProjectSummary[] = [
  { id: '1', name: 'Frontend redesign', dot: 'var(--tp-cat-1)' },
  { id: '1', name: 'API v2', dot: 'var(--tp-cat-3)' },
  { id: '1', name: 'Mobile app', dot: 'var(--tp-cat-4)' },
  { id: '1', name: 'Marketing site', dot: 'var(--tp-cat-2)' },
]
