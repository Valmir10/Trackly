import type { ProjectDto, TicketDto } from '@/api/types'
import { STATUS_META } from '@/api/adapters'
import type { BackendTicketStatus } from '@/api/types'

const WEEK_MS = 7 * 24 * 60 * 60 * 1000

export interface StatCard {
  label: string
  value: string
  sub: string
}

export interface StatusSlice {
  name: string
  value: number
  color: string
}

export interface ProjectSlice {
  name: string
  open: number
  done: number
}

export interface WeekSlice {
  week: string
  completed: number
  created: number
}

// All computed straight from the fetched tickets — no narrative, no
// hardcoded deltas. Numbers are only as interesting as the real data.
export function computeStats(tickets: TicketDto[]): StatCard[] {
  const now = Date.now()
  const total = tickets.length
  const done = tickets.filter((t) => t.status === 'Done')
  const rate = total > 0 ? Math.round((done.length / total) * 100) : 0

  const completedThisWeek = tickets.filter(
    (t) => t.completedAt && now - new Date(t.completedAt).getTime() < WEEK_MS
  ).length

  const overdue = tickets.filter((t) => t.status !== 'Done' && t.dueDate && new Date(t.dueDate).getTime() < now)
  const overdueHighPriority = overdue.filter((t) => t.priority === 'High').length

  const completionDurationsDays = done
    .filter((t) => t.completedAt)
    .map((t) => (new Date(t.completedAt as string).getTime() - new Date(t.createdAt).getTime()) / (1000 * 60 * 60 * 24))
  const avgDays =
    completionDurationsDays.length > 0
      ? completionDurationsDays.reduce((sum, d) => sum + d, 0) / completionDurationsDays.length
      : null

  return [
    { label: 'Tasks completed', value: String(done.length), sub: `${completedThisWeek} this week` },
    { label: 'Completion rate', value: `${rate}%`, sub: `${done.length}/${total} tasks` },
    { label: 'Avg. time to complete', value: avgDays === null ? '—' : `${avgDays.toFixed(1)}d`, sub: 'Per completed task' },
    { label: 'Overdue tasks', value: String(overdue.length), sub: `${overdueHighPriority} high priority` },
  ]
}

export function computeStatusBreakdown(tickets: TicketDto[]): StatusSlice[] {
  return (Object.keys(STATUS_META) as BackendTicketStatus[]).map((status) => ({
    name: STATUS_META[status].label,
    value: tickets.filter((t) => t.status === status).length,
    color: STATUS_META[status].color,
  }))
}

export function computeProjectBreakdown(tickets: TicketDto[], projects: ProjectDto[]): ProjectSlice[] {
  return projects.map((project) => {
    const projectTickets = tickets.filter((t) => t.projectId === project.id)
    return {
      name: project.name,
      open: projectTickets.filter((t) => t.status !== 'Done').length,
      done: projectTickets.filter((t) => t.status === 'Done').length,
    }
  })
}

export function computeWeeklyTrend(tickets: TicketDto[], weeks = 8): WeekSlice[] {
  const now = Date.now()
  const buckets: WeekSlice[] = []

  for (let i = weeks - 1; i >= 0; i--) {
    const bucketEnd = now - i * WEEK_MS
    const bucketStart = bucketEnd - WEEK_MS
    const label = new Date(bucketStart).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })

    const created = tickets.filter((t) => {
      const ts = new Date(t.createdAt).getTime()
      return ts >= bucketStart && ts < bucketEnd
    }).length

    const completed = tickets.filter((t) => {
      if (!t.completedAt) return false
      const ts = new Date(t.completedAt).getTime()
      return ts >= bucketStart && ts < bucketEnd
    }).length

    buckets.push({ week: label, created, completed })
  }

  return buckets
}
