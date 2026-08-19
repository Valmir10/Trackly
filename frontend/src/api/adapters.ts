import type { Task } from '@/components/TaskCard'
import type { BackendTicketPriority, BackendTicketStatus, TicketDto } from '@/api/types'

// The Kanban board's column ids are a frontend-only concept; the backend
// only knows the TicketStatus enum. These two maps are the single place
// that translation happens, so the board and the status-change API call
// can never drift out of sync with each other.
export const STATUS_TO_COLUMN_ID: Record<BackendTicketStatus, string> = {
  ToDo: 'todo',
  InProgress: 'inprogress',
  InReview: 'review',
  Done: 'done',
}

export const COLUMN_ID_TO_STATUS: Record<string, BackendTicketStatus> = {
  todo: 'ToDo',
  inprogress: 'InProgress',
  review: 'InReview',
  done: 'Done',
}

// Same label/color pairing as the board's Kanban columns (useTaskStore's
// BASE_COLUMNS), so a status means the same color everywhere it appears.
export const STATUS_META: Record<BackendTicketStatus, { label: string; color: string }> = {
  ToDo: { label: 'To Do', color: 'var(--tp-text-muted)' },
  InProgress: { label: 'In Progress', color: 'var(--tp-cat-3)' },
  InReview: { label: 'In Review', color: 'var(--tp-warning)' },
  Done: { label: 'Done', color: 'var(--tp-success)' },
}

const PRIORITY_TO_FRONTEND: Record<BackendTicketPriority, Task['priority']> = {
  Low: 'low',
  Medium: 'medium',
  High: 'high',
}

function formatDueDate(iso: string | null): string | undefined {
  if (!iso) return undefined
  return new Date(iso).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })
}

function isDueSoon(iso: string | null): boolean {
  if (!iso) return false
  const daysUntilDue = (new Date(iso).getTime() - Date.now()) / (1000 * 60 * 60 * 24)
  return daysUntilDue <= 3
}

export function mapTicketDtoToTask(dto: TicketDto): Task {
  return {
    id: dto.id,
    projectId: dto.projectId,
    title: dto.title,
    priority: PRIORITY_TO_FRONTEND[dto.priority],
    assignee: dto.assignedToInitials ? { initials: dto.assignedToInitials } : undefined,
    dueDate: formatDueDate(dto.dueDate),
    dueSoon: isDueSoon(dto.dueDate),
    description: dto.description ?? undefined,
    milestoneId: dto.milestoneId ?? undefined,
    blockedByTicketId: dto.blockedByTicketId ?? undefined,
    blockedByMilestoneId: dto.blockedByMilestoneId ?? undefined,
    originMeetingId: dto.originMeetingId ?? undefined,
  }
}
