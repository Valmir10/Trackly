import type { MessageBlock } from './types'

interface Resolved {
  id: string
  label: string
}

interface ParseContext {
  resolveUser: (handle: string) => Resolved | undefined
  resolveTicket: (id: string) => Resolved | undefined
}

const TOKEN_RE = /([@#])([a-zA-Z0-9_-]+)/g

// Composer stays plain text while typing (no rich contentEditable); this is
// the one place raw input becomes structured blocks, run on send. An
// unresolved handle/id (typo, or free text that happens to start with @/#)
// is left as literal text rather than turned into a broken chip.
export function parseMessage(raw: string, ctx: ParseContext): MessageBlock[] {
  const blocks: MessageBlock[] = []
  let lastIndex = 0
  let match: RegExpExecArray | null

  TOKEN_RE.lastIndex = 0
  while ((match = TOKEN_RE.exec(raw))) {
    const [full, marker, token] = match
    const resolved = marker === '@' ? ctx.resolveUser(token) : ctx.resolveTicket(token)
    if (!resolved) continue

    if (match.index > lastIndex) {
      blocks.push({ type: 'text', text: raw.slice(lastIndex, match.index) })
    }
    blocks.push(
      marker === '@'
        ? { type: 'mention', userId: resolved.id, label: resolved.label }
        : { type: 'ticketRef', ticketId: resolved.id, label: resolved.label }
    )
    lastIndex = match.index + full.length
  }

  if (lastIndex < raw.length) {
    blocks.push({ type: 'text', text: raw.slice(lastIndex) })
  }

  return blocks.length > 0 ? blocks : [{ type: 'text', text: raw }]
}
