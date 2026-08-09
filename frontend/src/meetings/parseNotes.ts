import type { MessageBlock } from '@/blocks/types'

interface Resolved {
  id: string
  label: string
}

interface ParseContext {
  resolveUser: (handle: string) => Resolved | undefined
  resolveTicket: (id: string) => Resolved | undefined
  resolveDecision: (id: string) => Resolved | undefined
}

// Same shape as chat's TOKEN_RE, widened to the two new authoring markers.
// '+' resolves via resolveTicket too, deliberately — an actionItem token's
// id points at a real Ticket, the same entity type a '#' reference points
// at, just rendered differently (a live card, not a plain chip).
const TOKEN_RE = /([@#>+])([a-zA-Z0-9_-]+)/g

// Phase A: inline tokens only. agendaItem blocks are machine-inserted (via
// the Suggested Agenda panel) as a separate line-level format, not parsed
// by this regex — see the pre-pass below.
const AGENDA_LINE_RE = /^~([a-zA-Z0-9_-]+)\|(.*)$/

export function parseNotes(raw: string, ctx: ParseContext): MessageBlock[] {
  const lines = raw.split('\n')
  const blocks: MessageBlock[] = []

  lines.forEach((line, i) => {
    const agendaMatch = AGENDA_LINE_RE.exec(line)
    if (agendaMatch) {
      const [, ticketId, reason] = agendaMatch
      blocks.push({ type: 'agendaItem', ticketId, reason })
    } else {
      blocks.push(...parseInline(line, ctx))
    }
    if (i < lines.length - 1) blocks.push({ type: 'text', text: '\n' })
  })

  return blocks
}

// # resolves against tickets first, decisions second — the same trigger
// surfaces both entity types (PRODUCT.md: "Decisions indexed inside the #
// picker framework alongside tickets"), and GUID collision between the two
// tables is not a real concern.
function parseInline(raw: string, ctx: ParseContext): MessageBlock[] {
  const blocks: MessageBlock[] = []
  let lastIndex = 0
  let match: RegExpExecArray | null

  TOKEN_RE.lastIndex = 0
  while ((match = TOKEN_RE.exec(raw))) {
    const [full, marker, token] = match
    let block: MessageBlock | undefined

    if (marker === '@') {
      const user = ctx.resolveUser(token)
      if (user) block = { type: 'mention', userId: user.id, label: user.label }
    } else if (marker === '#') {
      const ticket = ctx.resolveTicket(token)
      if (ticket) {
        block = { type: 'ticketRef', ticketId: ticket.id, label: ticket.label }
      } else {
        const decision = ctx.resolveDecision(token)
        if (decision) block = { type: 'decisionRef', decisionId: decision.id, label: decision.label }
      }
    } else if (marker === '>') {
      const decision = ctx.resolveDecision(token)
      if (decision) block = { type: 'decisionRef', decisionId: decision.id, label: decision.label }
    } else {
      const ticket = ctx.resolveTicket(token)
      if (ticket) block = { type: 'actionItem', ticketId: ticket.id }
    }

    if (!block) continue

    if (match.index > lastIndex) {
      blocks.push({ type: 'text', text: raw.slice(lastIndex, match.index) })
    }
    blocks.push(block)
    lastIndex = match.index + full.length
  }

  if (lastIndex < raw.length || blocks.length === 0) {
    blocks.push({ type: 'text', text: raw.slice(lastIndex) })
  }

  return blocks
}
