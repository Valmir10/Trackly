import { useRegisterCommands, useRegisterCommandSource } from './registry'
import { useMeetingStore } from '@/store/useMeetingStore'
import { useDecisionStore } from '@/store/useDecisionStore'
import { useWorkspaceMeetings } from '@/hooks/useWorkspaceMeetings'
import { useWorkspaceDecisions } from '@/hooks/useWorkspaceDecisions'
import type { MeetingSummaryDto } from '@/api/types'
import type { Command, CommandContext } from './types'

const NAV_COMMAND: Command = {
  id: 'nav.meetings',
  title: 'Go to Meetings',
  group: 'navigate',
  keywords: ['minutes', 'notes', 'decisions'],
  keybinding: { steps: [{ key: 'g' }, { key: 'm' }], label: 'G M' },
  run: (ctx) => ctx.navigate(`/${ctx.slug}/meetings`),
}

function meetingJumpCommands(meetings: Record<string, MeetingSummaryDto>): Command[] {
  return Object.values(meetings).map((meeting) => ({
    id: `nav.meeting.${meeting.id}`,
    title: `Go to ${meeting.title}`,
    group: 'meeting' as const,
    keywords: ['meeting', 'minutes'],
    run: (ctx: CommandContext) => ctx.openMeeting(meeting.id),
  }))
}

// Registers "Go to Meetings" + per-meeting jump commands, plus a
// #-prefixed decisions source — CommandSources are keyed by their own id,
// not by prefix, so this coexists with builtins.tsx's source.tickets under
// the same '#' trigger with zero edits to that file. This is the literal
// "Decisions indexed inside the # picker framework alongside tickets"
// requirement, not a separate lookalike system.
export function useBuiltinMeetingCommands() {
  useWorkspaceMeetings()
  useWorkspaceDecisions()
  const meetings = useMeetingStore((s) => s.meetings)

  useRegisterCommands(() => [NAV_COMMAND, ...meetingJumpCommands(meetings)], [meetings])

  useRegisterCommandSource(
    () => ({
      id: 'source.decisions',
      prefix: '#',
      getCommands: (query) => {
        if (query.trim().length === 0) return []
        const q = query.toLowerCase()
        const decisions = Object.values(useDecisionStore.getState().decisions)
        return decisions
          .filter((d) => d.id.startsWith(query) || d.text.toLowerCase().includes(q))
          .map((d) => ({
            id: `open-decision.${d.id}`,
            title: d.text,
            group: 'meeting' as const,
            mono: `#${d.id}`,
            recencyKey: 'open-decision',
            run: (ctx: CommandContext) => ctx.navigate(`/${ctx.slug}/meetings/${d.meetingId}?decision=${d.id}`),
          }))
      },
    }),
    []
  )
}
