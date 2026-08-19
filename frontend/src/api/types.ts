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
  updatedAt: string
  milestoneId: string | null
  blockedByTicketId: string | null
  blockedByMilestoneId: string | null
  originMeetingId: string | null
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

// Excludes `notes` — a meetings list shouldn't ship every meeting's full
// notes payload, mirroring the backend's MeetingSummaryDto.
export interface MeetingSummaryDto {
  id: string
  projectId: string
  title: string
  scheduledAt: string
  createdAt: string
}

export interface ContractDto {
  id: string
  projectId: string
  title: string
  createdAt: string
}

export interface MilestoneDto {
  id: string
  projectId: string
  contractId: string
  title: string
  ticketsTotal: number
  ticketsDone: number
  progressPercentage: number
  isApproved: boolean
  approvedAt: string | null
}

// Never includes the raw token or its hash — a management list, not a way
// to retrieve/re-derive a usable credential.
export interface ClientRoomAccessDto {
  id: string
  expiresAt: string
  createdAt: string
  revokedAt: string | null
  isActive: boolean
}

// Present only in the response of the create call — never retrievable
// again after that.
export interface CreateClientRoomAccessResult {
  accessId: string
  rawToken: string
}

// Aggregate-only — no individual ticket titles/assignees/statuses, matching
// what the backend's client-room-scoped DTO exposes.
export interface ClientRoomSummaryDto {
  projectName: string
  projectColor: string
  contracts: ClientRoomContractDto[]
}

export interface ClientRoomContractDto {
  id: string
  title: string
  milestones: ClientRoomMilestoneDto[]
}

export interface ClientRoomMilestoneDto {
  id: string
  title: string
  ticketsTotal: number
  ticketsDone: number
  progressPercentage: number
  isApproved: boolean
  approvedAt: string | null
}
