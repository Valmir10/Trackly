import type { ProjectDto, TicketDto } from '@/api/types'
import type { Scope } from '@/store/useWorkspaceScope'
import { filterProjectsByScope, filterTicketsByScope } from '@/utils/scope'

export interface PulseSignal {
  id: string
  severity: 'danger' | 'warning' | 'info'
  message: string
  to: string
}

const WEEK_MS = 7 * 24 * 60 * 60 * 1000

// Discrete signals — (severity, one-line statement, link target) — computed
// from real ticket data, not narrative text. Overdue counts, completion
// rate, and week-over-week velocity are all queries over what's actually in
// the workspace, so the numbers are only as interesting as the real data.
export function computePulseSignals(
  tickets: TicketDto[],
  projects: ProjectDto[],
  scope: Scope,
  slug: string
): PulseSignal[] {
  const now = Date.now()
  const inScope = filterTicketsByScope(tickets, scope)
  const scopedProjects = filterProjectsByScope(projects, scope)

  const signals: PulseSignal[] = []

  for (const project of scopedProjects) {
    const overdue = inScope.filter(
      (t) => t.projectId === project.id && t.status !== 'Done' && t.dueDate && new Date(t.dueDate).getTime() < now
    )
    if (overdue.length > 0) {
      signals.push({
        id: `overdue-${project.id}`,
        severity: 'danger',
        message: `${overdue.length} task${overdue.length === 1 ? '' : 's'} in ${project.name} ${overdue.length === 1 ? 'is' : 'are'} overdue`,
        to: `/${slug}/projects/${project.id}`,
      })
    }
  }

  if (inScope.length > 0) {
    const done = inScope.filter((t) => t.status === 'Done').length
    const rate = Math.round((done / inScope.length) * 100)
    signals.push({
      id: 'completion-rate',
      severity: rate >= 70 ? 'info' : rate >= 40 ? 'warning' : 'danger',
      message: `Completion rate is ${rate}% ${scope.type === 'all' ? 'across the workspace' : 'in this project'}`,
      to: `/${slug}/analytics`,
    })

    const completedThisWeek = inScope.filter(
      (t) => t.completedAt && now - new Date(t.completedAt).getTime() < WEEK_MS
    ).length
    const completedLastWeek = inScope.filter((t) => {
      if (!t.completedAt) return false
      const age = now - new Date(t.completedAt).getTime()
      return age >= WEEK_MS && age < 2 * WEEK_MS
    }).length

    if (completedThisWeek > 0 || completedLastWeek > 0) {
      const delta = completedThisWeek - completedLastWeek
      const trend = delta > 0 ? `up ${delta}` : delta < 0 ? `down ${Math.abs(delta)}` : 'flat'
      signals.push({
        id: 'velocity',
        severity: delta < 0 ? 'warning' : 'info',
        message: `${completedThisWeek} task${completedThisWeek === 1 ? '' : 's'} completed this week, ${trend} vs last week`,
        to: `/${slug}/analytics`,
      })
    }
  }

  return signals
}
