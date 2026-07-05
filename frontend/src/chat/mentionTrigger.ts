export interface MentionTrigger {
  marker: '@' | '#'
  query: string
  start: number
}

const TRIGGER_RE = /(?:^|\s)([@#])([a-zA-Z0-9_-]*)$/

// Looks backward from the caret for an unterminated @/# token so the picker
// only shows up mid-mention, not after a space or a completed one.
export function detectMentionTrigger(value: string, caret: number): MentionTrigger | null {
  const upToCaret = value.slice(0, caret)
  const match = TRIGGER_RE.exec(upToCaret)
  if (!match) return null

  const marker = match[1] as '@' | '#'
  const query = match[2]
  const start = caret - marker.length - query.length
  return { marker, query, start }
}
