// Block-model messages: message content is MessageBlock[], not a plain
// string. This is what lets a meeting's notes (Move 7) reuse the same
// renderer at document scale later — a document is a Block[] with a
// different container, not a different rendering system.
export type MessageBlock =
  | { type: 'text'; text: string }
  | { type: 'mention'; userId: string; label: string }
  | { type: 'ticketRef'; ticketId: string; label: string }
  // Typed now, not rendered yet — Move 5's live inline ticket card lands in
  // this same block slot once it exists.
  | { type: 'card'; ticketId: string }

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
