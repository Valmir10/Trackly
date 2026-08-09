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
// the Suggested Agenda panel, Phase B) as a separate line-level format, not
// parsed by this regex.
export function parseNotes(raw: string, ctx: ParseContext): MessageBlock[] {
  const blocks: MessageBlock[] = []
  let lastIndex = 0
  let match: RegExpExecArray | null

  TOKEN_RE.lastIndex = 0
  while ((match = TOKEN_RE.exec(raw))) {
    const [full, marker, token] = match
    const resolved =
      marker === '@'
        ? ctx.resolveUser(token)
        : marker === '#'
          ? ctx.resolveTicket(token)
          : marker === '>'
            ? ctx.resolveDecision(token)
            : ctx.resolveTicket(token)
    if (!resolved) continue

    if (match.index > lastIndex) {
      blocks.push({ type: 'text', text: raw.slice(lastIndex, match.index) })
    }

    if (marker === '@') {
      blocks.push({ type: 'mention', userId: resolved.id, label: resolved.label })
    } else if (marker === '#') {
      blocks.push({ type: 'ticketRef', ticketId: resolved.id, label: resolved.label })
    } else if (marker === '>') {
      blocks.push({ type: 'decisionRef', decisionId: resolved.id, label: resolved.label })
    } else {
      blocks.push({ type: 'actionItem', ticketId: resolved.id })
    }

    lastIndex = match.index + full.length
  }

  if (lastIndex < raw.length) {
    blocks.push({ type: 'text', text: raw.slice(lastIndex) })
  }

  return blocks.length > 0 ? blocks : [{ type: 'text', text: raw }]
}
