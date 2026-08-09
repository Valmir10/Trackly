import { create } from 'zustand'
import type { MeetingDto } from '@/api/types'

// Phase A only ever shows one meeting at a time (no list page yet), so this
// is a single-meeting slice rather than a keyed collection.
interface MeetingStoreState {
  meetingId: string | null
  projectId: string | null
  title: string
  notes: string
  setMeetingFromApi: (meeting: MeetingDto) => void
  setNotes: (notes: string) => void
}

export const useMeetingStore = create<MeetingStoreState>((set) => ({
  meetingId: null,
  projectId: null,
  title: '',
  notes: '',

  setMeetingFromApi: (meeting) => {
    set({ meetingId: meeting.id, projectId: meeting.projectId, title: meeting.title, notes: meeting.notes })
  },

  setNotes: (notes) => {
    set({ notes })
  },
}))
