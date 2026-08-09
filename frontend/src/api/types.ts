// Shapes returned by the real backend (Trackly.Api) — mirrors the DTOs
// defined in Trackly.Application's Features/*/Queries folders.

export interface ProjectDto {
  id: string
  name: string
  color: string
  description: string | null
}

export type BackendTicketStatus = 'ToDo' | 'InProgress' | 'InReview' | 'Done'
export type BackendTicketPriority = 'Low' | 'Medium' | 'High'

export interface TicketDto {
  id: string
  projectId: string
  title: string
  description: string | null
  status: BackendTicketStatus
  priority: BackendTicketPriority
  assignedToId: string | null
  assignedToInitials: string | null
  dueDate: string | null
  position: number
  createdAt: string
  completedAt: string | null
}

export interface ChatMessageDto {
  id: string
  authorId: string
  authorInitials: string
  content: string
  ticketId: string | null
  createdAt: string
}

export interface MeetingDto {
  id: string
  projectId: string
  title: string
  scheduledAt: string
  notes: string
  createdById: string
  createdAt: string
  updatedAt: string
}

export interface DecisionDto {
  id: string
  meetingId: string
  projectId: string
  text: string
  createdById: string
  createdAt: string
}
