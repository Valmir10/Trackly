import { useRegisterCommands, useRegisterCommandSource } from './registry'
import { useTaskStore } from '@/store/useTaskStore'
import { useProjects } from '@/hooks/useProjects'
import type { ProjectDto } from '@/api/types'
import type { Command, CommandContext, CommandPage } from './types'

const NAV_COMMANDS: Command[] = [
  {
    id: 'nav.dashboard',
    title: 'Go to Dashboard',
    group: 'navigate',
    keywords: ['home'],
    keybinding: { steps: [{ key: 'g' }, { key: 'd' }], label: 'G D' },
    run: (ctx) => ctx.navigate(`/${ctx.slug}/dashboard`),
  },
  {
    id: 'nav.projects',
    title: 'Go to Projects',
    group: 'navigate',
    keybinding: { steps: [{ key: 'g' }, { key: 'p' }], label: 'G P' },
    run: (ctx) => ctx.navigate(`/${ctx.slug}/projects`),
  },
  {
    id: 'nav.tasks',
    title: 'Go to My Tasks',
    group: 'navigate',
    keybinding: { steps: [{ key: 'g' }, { key: 't' }], label: 'G T' },
    run: (ctx) => ctx.navigate(`/${ctx.slug}/tasks`),
  },
  {
    id: 'nav.analytics',
    title: 'Go to Analytics',
    group: 'navigate',
    keybinding: { steps: [{ key: 'g' }, { key: 'a' }], label: 'G A' },
    run: (ctx) => ctx.navigate(`/${ctx.slug}/analytics`),
  },
  {
    id: 'nav.settings',
    title: 'Go to Settings',
    group: 'navigate',
    keybinding: { steps: [{ key: 'g' }, { key: 's' }], label: 'G S' },
    run: (ctx) => ctx.navigate(`/${ctx.slug}/settings`),
  },
]

function projectJumpCommands(projects: ProjectDto[]): Command[] {
  return projects.map((project) => ({
    id: `nav.project.${project.id}`,
    title: `Go to ${project.name}`,
    group: 'navigate' as const,
    keywords: ['project', 'board', 'kanban'],
    run: (ctx: CommandContext) => ctx.navigate(`/${ctx.slug}/projects/${project.id}`),
  }))
}

function columnIdForTitle(title: string): string {
  const column = useTaskStore.getState().columns.find((c) => c.title === title)
  return column?.id ?? title
}

function statusPage(taskId: string, taskTitle: string, statuses: string[]): CommandPage {
  return {
    id: `page.status.${taskId}`,
    title: `Status: #${taskId}`,
    placeholder: `Set status for "${taskTitle}"...`,
    commands: statuses.map((status) => ({
      id: `action.set-status.${taskId}.${status}`,
      title: status,
      group: 'action' as const,
      run: () => {
        void useTaskStore.getState().moveTask(taskId, columnIdForTitle(status))
      },
    })),
  }
}

function changeStatusCommand(): Command {
  return {
    id: 'action.change-status',
    title: 'Change ticket status…',
    group: 'action',
    keywords: ['move', 'status'],
    run: (): CommandPage => {
      const { columns } = useTaskStore.getState()
      const statuses = columns.map((c) => c.title)
      return {
        id: 'page.change-status.pick-ticket',
        title: 'Change status',
        placeholder: 'Select a ticket...',
        commands: columns.flatMap((col) =>
          col.tasks.map((task) => ({
            id: `select-ticket.${task.id}`,
            title: task.title,
            group: 'ticket' as const,
            mono: `#${task.id}`,
            run: () => statusPage(task.id, task.title, statuses),
          }))
        ),
      }
    },
  }
}

export function useBuiltinCommands() {
  const { data: projects = [] } = useProjects()

  useRegisterCommands(
    () => [...NAV_COMMANDS, ...projectJumpCommands(projects), changeStatusCommand()],
    [projects]
  )

  useRegisterCommandSource(
    () => ({
      id: 'source.tickets',
      getCommands: (query) => {
        if (query.trim().length === 0) return []
        const { columns } = useTaskStore.getState()
        return columns.flatMap((col) =>
          col.tasks.map((task) => ({
            id: `open-ticket.${task.id}`,
            title: task.title,
            group: 'ticket' as const,
            mono: `#${task.id}`,
            recencyKey: 'open-ticket',
            run: (ctx: CommandContext) => ctx.openTicket(task.id, task.projectId ?? ''),
          }))
        )
      },
    }),
    []
  )
}
