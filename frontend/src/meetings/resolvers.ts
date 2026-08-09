import { useDecisionStore } from '@/store/useDecisionStore'

export { resolveUser, resolveTicket } from '@/chat/resolvers'

// Decisions are immutable once created, so unlike resolveTicket there's no
// "live status" concern here — just a synchronous lookup against whatever's
// been fetched/pushed into the store so far.
export function resolveDecision(id: string) {
  const decision = useDecisionStore.getState().decisions[id]
  return decision ? { id: decision.id, label: decision.text } : undefined
}
