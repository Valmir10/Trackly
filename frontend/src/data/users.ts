// Shared mock workspace member directory. ProjectHeader's avatar stack and
// the chat @-mention picker both read from here so initials can't drift.
// Mock now, real workspace-scoped query once the backend lands in Move 4.
export interface WorkspaceMember {
  id: string
  name: string
  initials: string
  handle: string
}

export const WORKSPACE_MEMBERS: WorkspaceMember[] = [
  { id: 'vz', name: 'Valmir Zogaj', initials: 'VZ', handle: 'valmir' },
  { id: 'sk', name: 'Sarah Kim', initials: 'SK', handle: 'sarah' },
  { id: 'js', name: 'James Sato', initials: 'JS', handle: 'james' },
  { id: 'am', name: 'Aisha Malik', initials: 'AM', handle: 'aisha' },
]

export const CURRENT_MEMBER = WORKSPACE_MEMBERS[0]
