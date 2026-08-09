import { create } from 'zustand'
import type { DecisionDto } from '@/api/types'

// Flat, keyed by id — not nested by meeting — so resolveDecision(id) can be
// a synchronous dictionary lookup the same way resolveTicket is against
// useTaskStore, regardless of which meeting a decision was created in.
interface DecisionStoreState {
  decisions: Record<string, DecisionDto>
  setDecisionsFromApi: (decisions: DecisionDto[]) => void
  applyRemoteDecision: (decision: DecisionDto) => void
}

export const useDecisionStore = create<DecisionStoreState>((set) => ({
  decisions: {},

  setDecisionsFromApi: (decisions) => {
    set((state) => ({
      decisions: { ...state.decisions, ...Object.fromEntries(decisions.map((d) => [d.id, d])) },
    }))
  },

  applyRemoteDecision: (decision) => {
    set((state) => ({ decisions: { ...state.decisions, [decision.id]: decision } }))
  },
}))
