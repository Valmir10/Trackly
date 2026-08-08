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
    title: dto.title,
    priority: PRIORITY_TO_FRONTEND[dto.priority],
    assignee: dto.assignedToInitials ? { initials: dto.assignedToInitials } : undefined,
    dueDate: formatDueDate(dto.dueDate),
    dueSoon: isDueSoon(dto.dueDate),
    description: dto.description ?? undefined,
  }
}
