import { create } from 'zustand'
import type { MilestoneDto } from '@/api/types'

// Flat, keyed by id — mirrors useDecisionStore, so the milestone #-picker
// source can do a synchronous dictionary lookup regardless of which
// project/contract a milestone belongs to.
interface MilestoneStoreState {
  milestones: Record<string, MilestoneDto>
  setMilestonesFromApi: (milestones: MilestoneDto[]) => void
}

export const useMilestoneStore = create<MilestoneStoreState>((set) => ({
  milestones: {},

  setMilestonesFromApi: (milestones) => {
    set((state) => ({
      milestones: { ...state.milestones, ...Object.fromEntries(milestones.map((m) => [m.id, m])) },
    }))
  },
}))
