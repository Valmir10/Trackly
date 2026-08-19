import { useRegisterCommandSource } from './registry'
import { useMilestoneStore } from '@/store/useMilestoneStore'
import { useWorkspaceMilestones } from '@/hooks/useWorkspaceMilestones'
import type { CommandContext } from './types'

// A #-prefixed milestones source — CommandSources are keyed by their own
// id, not by prefix, so this coexists with source.tickets/source.decisions
// under the same '#' trigger with zero edits to those files. This is what
// closes the "palette's reach matches the product's actual surface count"
// gap: milestones existed since Move 8 but were never indexed anywhere.
export function useBuiltinMilestoneCommands() {
  useWorkspaceMilestones()

  useRegisterCommandSource(
    () => ({
      id: 'source.milestones',
      prefix: '#',
      getCommands: (query) => {
        if (query.trim().length === 0) return []
        const q = query.toLowerCase()
        const milestones = Object.values(useMilestoneStore.getState().milestones)
        return milestones
          .filter((m) => m.id.startsWith(query) || m.title.toLowerCase().includes(q))
          .map((m) => ({
            id: `open-milestone.${m.id}`,
            title: m.title,
            group: 'milestone' as const,
            mono: m.isApproved ? 'Approved' : `${m.ticketsDone}/${m.ticketsTotal}`,
            recencyKey: 'open-milestone',
            run: (ctx: CommandContext) =>
              ctx.navigate(`/${ctx.slug}/projects/${m.projectId}/contracts?milestone=${m.id}`),
          }))
      },
    }),
    []
  )
}
