import type { Command, RankedCommand } from './types'

// Ranking priorities, in order:
// 1. Exact datum match ("#127" or "127" against command.mono) — the
//    power-user fast path must always win.
// 2. Exact title match, then title prefix.
// 3. Word-boundary subsequence ("nk b" -> "Northlight Kanban Board").
// 4. Loose in-order subsequence (classic fuzzy).
// Recency adds a bounded bonus — it reorders near-ties, it never lets a
// stale habit beat a better literal match (see SCORE constants below:
// subsequence + max recency bonus is well under a title-prefix hit).
//
// Complexity: O(commands x query length). A workspace has a few hundred
// commands; this runs comfortably per keystroke with no memoization.

const SCORE = {
  monoExact: 1000,
  titleExact: 900,
  titlePrefix: 600,
  wordBoundary: 400,
  subsequence: 150,
  keywordHit: 120,
  recencyMax: 100,
} as const

function normalize(s: string): string {
  return s.toLowerCase().trim()
}

function stripHash(s: string): string {
  return s.startsWith('#') ? s.slice(1) : s
}

/** In-order subsequence with word-boundary bonus. Returns a score fragment, or -1 for no match. */
function fuzzy(query: string, target: string): number {
  let qi = 0
  let boundaryHits = 0
  let prevWasBoundary = true // string start counts as a boundary
  for (let ti = 0; ti < target.length && qi < query.length; ti++) {
    const ch = target[ti]
    const isBoundary = prevWasBoundary
    prevWasBoundary = ch === ' ' || ch === '-' || ch === '/' || ch === '.'
    if (ch === query[qi]) {
      if (isBoundary) boundaryHits++
      qi++
    }
  }
  if (qi < query.length) return -1
  return boundaryHits >= query.replace(/\s/g, '').length
    ? SCORE.wordBoundary
    : SCORE.subsequence + boundaryHits * 20
}

export function scoreCommand(rawQuery: string, command: Command): number {
  const query = normalize(rawQuery)
  if (query.length === 0) return 1

  const title = normalize(command.title)
  const mono = command.mono ? normalize(command.mono) : null

  // Datum fast path: "#127" and "127" are equivalent on both sides,
  // regardless of whether the command's mono field carries the "#" or not.
  if (mono && stripHash(mono) === stripHash(query)) {
    return SCORE.monoExact
  }

  if (title === query) return SCORE.titleExact
  if (title.startsWith(query)) return SCORE.titlePrefix

  let best = fuzzy(query.replace(/\s/g, ''), title)

  if (command.keywords) {
    for (const kw of command.keywords) {
      const k = normalize(kw)
      if (k === query) best = Math.max(best, SCORE.titlePrefix)
      else if (k.startsWith(query)) best = Math.max(best, SCORE.keywordHit + 100)
      else {
        const f = fuzzy(query.replace(/\s/g, ''), k)
        if (f > 0) best = Math.max(best, Math.min(f, SCORE.keywordHit))
      }
    }
  }

  return best
}

export function rank(rawQuery: string, commands: Command[], recency: string[], limit = 12): RankedCommand[] {
  const out: RankedCommand[] = []
  for (const command of commands) {
    const base = scoreCommand(rawQuery, command)
    if (base <= 0) continue
    const rKey = command.recencyKey ?? command.id
    const rIndex = recency.indexOf(rKey)
    const recencyBonus = rIndex === -1 ? 0 : Math.round(SCORE.recencyMax * (1 - rIndex / recency.length))
    out.push({ command, score: base + recencyBonus })
  }
  out.sort((a, b) => b.score - a.score)
  return out.slice(0, limit)
}
