import { create } from 'zustand'

// One discriminated union, consumed by every surface that needs a "lens" —
// dashboard, Analytics, and (later) ChatThread/the command palette. Not a
// dashboard-only concern, hence its own store rather than a page-local one.
export type Scope = { type: 'all' } | { type: 'project'; projectId: string }

interface WorkspaceScopeState {
  scope: Scope
  setScope: (scope: Scope) => void
}

export const useWorkspaceScope = create<WorkspaceScopeState>((set) => ({
  scope: { type: 'all' },
  setScope: (scope) => set({ scope }),
}))
