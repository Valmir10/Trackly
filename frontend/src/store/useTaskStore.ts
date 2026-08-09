import { create } from 'zustand'
import type { Task } from '@/components/TaskCard'
import type { BackendTicketStatus, TicketDto } from '@/api/types'
import { COLUMN_ID_TO_STATUS, mapTicketDtoToTask, STATUS_TO_COLUMN_ID } from '@/api/adapters'
import { apiClient } from '@/lib/apiClient'

// Real tickets, fetched from the backend and kept here so the command
// palette (and anything else) can look up a ticket by id from anywhere in
// the app, not just while the project page happens to be mounted.

export interface TaskColumn {
  id: string
  title: string
  color: string
  tasks: Task[]
}

interface RemoteNewTicket {
  id: string
  title: string
}

interface TaskStoreState {
  columns: TaskColumn[]
  setTicketsFromApi: (tickets: TicketDto[]) => void
  moveTask: (taskId: string, targetColumnId: string) => Promise<void>
  applyRemoteStatusChange: (taskId: string, status: BackendTicketStatus) => void
  applyRemoteNewTicket: (ticket: RemoteNewTicket) => void
}

const BASE_COLUMNS: Omit<TaskColumn, 'tasks'>[] = [
  { id: 'todo', title: 'To Do', color: 'var(--tp-text-muted)' },
  { id: 'inprogress', title: 'In Progress', color: 'var(--tp-cat-3)' },
  { id: 'review', title: 'In Review', color: 'var(--tp-warning)' },
  { id: 'done', title: 'Done', color: 'var(--tp-success)' },
]

function moveTaskInColumns(columns: TaskColumn[], taskId: string, targetColumnId: string): TaskColumn[] {
  let moved: Task | undefined
  const stripped = columns.map((col) => {
    const remaining = col.tasks.filter((t) => {
      if (t.id === taskId) {
        moved = t
        return false
      }
      return true
    })
    return { ...col, tasks: remaining }
  })
  if (!moved) return columns
  return stripped.map((col) => (col.id === targetColumnId ? { ...col, tasks: [...col.tasks, moved as Task] } : col))
}

export const useTaskStore = create<TaskStoreState>((set, get) => ({
  columns: BASE_COLUMNS.map((col) => ({ ...col, tasks: [] })),

  setTicketsFromApi: (tickets) => {
    const tasksByColumn: Record<string, Task[]> = { todo: [], inprogress: [], review: [], done: [] }
    for (const ticket of tickets) {
      const columnId = STATUS_TO_COLUMN_ID[ticket.status] ?? 'todo'
      tasksByColumn[columnId].push(mapTicketDtoToTask(ticket))
    }
    set({ columns: BASE_COLUMNS.map((col) => ({ ...col, tasks: tasksByColumn[col.id] })) })
  },

  moveTask: async (taskId, targetColumnId) => {
    const { columns } = get()
    const found = findTask(columns, taskId)
    const targetColumn = columns.find((c) => c.id === targetColumnId)
    if (!found || !targetColumn) return

    const position = targetColumn.tasks.length
    const previousColumnId = found.column.id

    // Optimistic move so the board feels instant; rolled back below if the
    // API call fails.
    set((state) => ({ columns: moveTaskInColumns(state.columns, taskId, targetColumnId) }))

    try {
      await apiClient.patch(`/api/tickets/${taskId}/status`, {
        newStatus: COLUMN_ID_TO_STATUS[targetColumnId],
        position,
      })
    } catch {
      set((state) => ({ columns: moveTaskInColumns(state.columns, taskId, previousColumnId) }))
    }
  },

  // For updates that already happened on the server (a SignalR broadcast) —
  // no API call, just reflects reality locally.
  applyRemoteStatusChange: (taskId, status) => {
    const targetColumnId = STATUS_TO_COLUMN_ID[status]
    set((state) => ({ columns: moveTaskInColumns(state.columns, taskId, targetColumnId) }))
  },

  // A ticket created elsewhere (the board's own "Add task", or an
  // actionItem block in meeting notes) always starts in To Do — this makes
  // it appear live without a refetch. The broadcast is thin (id/title
  // only), so the placeholder card fills in with real priority/assignee on
  // the next normal refetch; guarded against double-insert since the
  // creator's own tab also receives its own broadcast.
  applyRemoteNewTicket: (ticket) => {
    set((state) => {
      if (findTask(state.columns, ticket.id)) return state
      return {
        columns: state.columns.map((col) =>
          col.id === 'todo'
            ? { ...col, tasks: [...col.tasks, { id: ticket.id, title: ticket.title, priority: 'medium' as const }] }
            : col
        ),
      }
    })
  },
}))

export function findTask(columns: TaskColumn[], taskId: string) {
  for (const column of columns) {
    const task = column.tasks.find((t) => t.id === taskId)
    if (task) return { task, column }
  }
  return undefined
}
