import type { TicketDto } from '@/api/types'

const DAY_MS = 24 * 60 * 60 * 1000

export function isOverdue(ticket: TicketDto): boolean {
  return ticket.status !== 'Done' && !!ticket.dueDate && new Date(ticket.dueDate).getTime() < Date.now()
}

export function isStale(ticket: TicketDto, thresholdDays = 5): boolean {
  return ticket.status !== 'Done' && Date.now() - new Date(ticket.updatedAt).getTime() > thresholdDays * DAY_MS
}
