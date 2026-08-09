export interface AuthoringTrigger {
  marker: '>' | '+'
  start: number
}

const ARM_RE = /(?:^|\s)([>+])$/

// Unlike detectMentionTrigger (chat/mentionTrigger.ts) — stateless,
// space-terminated, backed by a dropdown of existing entities — '>' and '+'
// are arm-and-capture triggers: free text with no picker, since there's
// nothing to search. Once armed, the span survives spaces and newlines;
// only the caller's own submit (Enter) / cancel (Escape) or the user
// backspacing past the marker ends it.
export function detectAuthoringTrigger(
  value: string,
  caret: number,
  armed: AuthoringTrigger | null
): AuthoringTrigger | null {
  if (armed) {
    return value[armed.start] === armed.marker ? armed : null
  }

  const upToCaret = value.slice(0, caret)
  const match = ARM_RE.exec(upToCaret)
  if (!match) return null

  const marker = match[1] as '>' | '+'
  return { marker, start: caret - marker.length }
}
