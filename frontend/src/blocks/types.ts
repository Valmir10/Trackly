// Block-model content: a message (or a meeting's notes) is a Block[], not a
// plain string — "a document is a Block[] with a different container, not a
// different rendering system." Lives here (not chat/) because it's no
// longer chat-only: meeting notes (Move 7) render the exact same blocks at
// document scale via the shared renderBlock below.
export type MessageBlock =
  | { type: 'text'; text: string }
  | { type: 'mention'; userId: string; label: string }
  | { type: 'ticketRef'; ticketId: string; label: string }
  // Local-only promotion target (useChatStore's promoteToCard) — never
  // produced by parsing raw text.
  | { type: 'card'; ticketId: string }
  // '>'-triggered in meeting notes — the id always points at a Decision
  // that was created synchronously the moment this block was authored.
  | { type: 'decisionRef'; decisionId: string; label: string }
  // '+'-triggered in meeting notes — same idea, but the id points at a real
  // Ticket. "Promoted to the real Board via one keystroke" because creating
  // the ticket IS what typing this block does, not a later export step.
  | { type: 'actionItem'; ticketId: string }
  // Machine-inserted only (via the Suggested Agenda panel's "Add to notes"
  // button), never user-typed. `reason` is frozen at insertion time so a
  // meeting's minutes don't silently reinterpret themselves later if the
  // referenced ticket's status changes.
  | { type: 'agendaItem'; ticketId: string; reason: string }
