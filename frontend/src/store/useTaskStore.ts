import { create } from 'zustand'
import type { Task } from '@/components/TaskCard'

// The single mock project's Kanban data, lifted out of ProjectPage so the
// command palette (and anything else) can look up a ticket by id from
// anywhere in the app, not just while the project page happens to be mounted.

export interface TaskColumn {
  id: string
  title: string
  color: string
  tasks: Task[]
}

interface TaskStoreState {
  columns: TaskColumn[]
  moveTask: (taskId: string, targetColumnId: string) => void
}

const initialColumns: TaskColumn[] = [
  {
    id: 'todo',
    title: 'To Do',
    color: 'var(--tp-text-muted)',
    tasks: [
      {
        id: '121',
        title: 'Set up Storybook for component documentation',
        tag: 'Docs',
        priority: 'low',
        assignee: { initials: 'VZ' },
        dueDate: 'Jun 20',
        commentCount: 2,
        description: 'Document the shared component library so new contributors can browse variants without reading source.',
      },
      {
        id: '122',
        title: 'Implement dark mode toggle in settings',
        tag: 'Feature',
        priority: 'medium',
        assignee: { initials: 'SK' },
        dueDate: 'Jun 18',
        attachmentCount: 1,
        description: 'Add a persisted theme toggle to the settings page, matching the toggle already in the app shell.',
      },
      {
        id: '123',
        title: 'Write unit tests for Button component',
        tag: 'Testing',
        priority: 'low',
        assignee: { initials: 'AM' },
        dueDate: 'Jun 22',
        description: 'Cover default, hover, focus-visible, and disabled states.',
      },
    ],
  },
  {
    id: 'inprogress',
    title: 'In Progress',
    color: 'var(--tp-cat-3)',
    tasks: [
      {
        id: '127',
        title: 'Design dashboard layout and stat cards',
        tag: 'Design',
        priority: 'high',
        assignee: { initials: 'VZ' },
        dueDate: 'Jun 11',
        dueSoon: true,
        commentCount: 5,
        attachmentCount: 3,
        description: 'Finalize the stat card layout, weekly summary card, and the two-column task/activity split for the dashboard.',
      },
      {
        id: '128',
        title: 'Integrate TanStack Query for data fetching',
        tag: 'Frontend',
        priority: 'high',
        assignee: { initials: 'JS' },
        dueDate: 'Jun 13',
        commentCount: 1,
        description: 'Stand up the query/mutation layer once the backend vertical slice lands; currently blocked on Move 4.',
      },
    ],
  },
  {
    id: 'review',
    title: 'In Review',
    color: 'var(--tp-warning)',
    tasks: [
      {
        id: '129',
        title: 'Landing page hero section redesign',
        tag: 'Design',
        priority: 'high',
        assignee: { initials: 'SK' },
        dueDate: 'Jun 10',
        dueSoon: true,
        commentCount: 8,
        attachmentCount: 2,
        description: 'Rework the hero headline, subtitle, and product-preview mock per the new positioning.',
      },
      {
        id: '130',
        title: 'Refactor authentication flow components',
        tag: 'Frontend',
        priority: 'medium',
        assignee: { initials: 'VZ' },
        dueDate: 'Jun 12',
        commentCount: 3,
        description: 'Consolidate the login/register/forgot-password forms onto the shared field primitives.',
      },
    ],
  },
  {
    id: 'done',
    title: 'Done',
    color: 'var(--tp-success)',
    tasks: [
      {
        id: '131',
        title: 'Set up Vite + React + TypeScript project',
        tag: 'DevOps',
        priority: 'high',
        assignee: { initials: 'VZ' },
        dueDate: 'Jun 1',
        commentCount: 2,
        description: 'Base project scaffold, path aliases, and lint/typecheck config.',
      },
      {
        id: '132',
        title: 'Configure Tailwind v4 and shadcn/ui',
        tag: 'Frontend',
        priority: 'medium',
        assignee: { initials: 'JS' },
        dueDate: 'Jun 3',
        attachmentCount: 1,
        description: 'Superseded — the project has since moved to the vanilla-CSS Precision Console system.',
      },
      {
        id: '133',
        title: 'Create CI/CD pipeline with GitHub Actions',
        tag: 'DevOps',
        priority: 'high',
        assignee: { initials: 'AM' },
        dueDate: 'Jun 5',
        description: 'Backend and frontend pipelines, path-filtered so only the relevant one fires per change.',
      },
    ],
  },
]

export const useTaskStore = create<TaskStoreState>((set) => ({
  columns: initialColumns,

  moveTask: (taskId, targetColumnId) => {
    set((state) => {
      let moved: Task | undefined
      const stripped = state.columns.map((col) => {
        const remaining = col.tasks.filter((t) => {
          if (t.id === taskId) {
            moved = t
            return false
          }
          return true
        })
        return { ...col, tasks: remaining }
      })
      if (!moved) return state
      return {
        columns: stripped.map((col) =>
          col.id === targetColumnId ? { ...col, tasks: [...col.tasks, moved as Task] } : col
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
