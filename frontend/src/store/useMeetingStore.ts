import { create } from 'zustand'
import type { MeetingDto, MeetingSummaryDto } from '@/api/types'

interface MeetingStoreState {
  // Single-meeting detail slice — the notes editor only ever shows one
  // meeting at a time.
  meetingId: string | null
  projectId: string | null
  title: string
  notes: string
  setMeetingFromApi: (meeting: MeetingDto) => void
  setNotes: (notes: string) => void

  // Flat, keyed by id — the meetings list page's data, independent of the
  // single-meeting slice above.
  meetings: Record<string, MeetingSummaryDto>
  setMeetingsFromApi: (meetings: MeetingSummaryDto[]) => void
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

  meetings: {},

  setMeetingsFromApi: (meetings) => {
    set((state) => ({
      meetings: { ...state.meetings, ...Object.fromEntries(meetings.map((m) => [m.id, m])) },
    }))
  },
}))
