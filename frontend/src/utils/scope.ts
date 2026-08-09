import type { MeetingSummaryDto, ProjectDto, TicketDto } from '@/api/types'
import type { Scope } from '@/store/useWorkspaceScope'

export function filterTicketsByScope(tickets: TicketDto[], scope: Scope): TicketDto[] {
  return scope.type === 'project' ? tickets.filter((t) => t.projectId === scope.projectId) : tickets
}

export function filterProjectsByScope(projects: ProjectDto[], scope: Scope): ProjectDto[] {
  return scope.type === 'project' ? projects.filter((p) => p.id === scope.projectId) : projects
}

export function filterMeetingsByScope(meetings: MeetingSummaryDto[], scope: Scope): MeetingSummaryDto[] {
  return scope.type === 'project' ? meetings.filter((m) => m.projectId === scope.projectId) : meetings
}
