// The block union now lives in blocks/types.ts (it's shared with meeting
// notes, not chat-only) — re-exported here under the same name so existing
// chat imports don't need to change.
import type { MessageBlock } from '@/blocks/types'
export type { MessageBlock } from '@/blocks/types'

export interface ChatMessage {
  id: string
  authorId: string
  authorInitials: string
  blocks: MessageBlock[]
  createdAt: string
}

// A project-scoped stream and a per-ticket comment thread are the same
// component, scoped differently — not two systems that look similar. The
// ticket variant still carries projectId (a ticket always belongs to
// exactly one project) because the real API's message list is scoped by
// project first.
export type ChatScope =
  | { type: 'project'; projectId: string }
  | { type: 'ticket'; projectId: string; ticketId: string }

export function scopeKey(scope: ChatScope): string {
  return scope.type === 'project' ? `project:${scope.projectId}` : `ticket:${scope.ticketId}`
}
