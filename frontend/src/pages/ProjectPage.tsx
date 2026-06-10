import AppSidebar from '@/components/dashboard/AppSidebar'
import AppTopBar from '@/components/dashboard/AppTopBar'
import ProjectHeader from '@/components/project/ProjectHeader'
import KanbanColumn from '@/components/project/KanbanColumn'
import TaskCard from '@/components/project/TaskCard'

const columns = [
  {
    id: 'todo',
    title: 'To Do',
    color: 'bg-slate-400',
    tasks: [
      { title: 'Set up Storybook for component documentation', tag: 'Docs', tagColor: 'bg-slate-500/10 text-slate-400', priority: 'Low' as const, assignee: { initials: 'VZ', color: 'bg-violet-500/15 text-violet-400' }, dueDate: 'Jun 20', commentCount: 2 },
      { title: 'Implement dark mode toggle in settings', tag: 'Feature', tagColor: 'bg-violet-500/10 text-violet-400', priority: 'Medium' as const, assignee: { initials: 'SK', color: 'bg-blue-500/15 text-blue-400' }, dueDate: 'Jun 18', attachmentCount: 1 },
      { title: 'Write unit tests for Button component', tag: 'Testing', tagColor: 'bg-green-500/10 text-green-400', priority: 'Low' as const, assignee: { initials: 'AM', color: 'bg-orange-500/15 text-orange-400' }, dueDate: 'Jun 22' },
    ],
  },
  {
    id: 'inprogress',
    title: 'In Progress',
    color: 'bg-blue-400',
    tasks: [
      { title: 'Design dashboard layout and stat cards', tag: 'Design', tagColor: 'bg-pink-500/10 text-pink-400', priority: 'High' as const, assignee: { initials: 'VZ', color: 'bg-violet-500/15 text-violet-400' }, dueDate: 'Jun 11', dueSoon: true, commentCount: 5, attachmentCount: 3 },
      { title: 'Integrate TanStack Query for data fetching', tag: 'Frontend', tagColor: 'bg-blue-500/10 text-blue-400', priority: 'High' as const, assignee: { initials: 'JS', color: 'bg-green-500/15 text-green-400' }, dueDate: 'Jun 13', commentCount: 1 },
    ],
  },
  {
    id: 'review',
    title: 'In Review',
    color: 'bg-yellow-400',
    tasks: [
      { title: 'Landing page hero section redesign', tag: 'Design', tagColor: 'bg-pink-500/10 text-pink-400', priority: 'High' as const, assignee: { initials: 'SK', color: 'bg-blue-500/15 text-blue-400' }, dueDate: 'Jun 10', dueSoon: true, commentCount: 8, attachmentCount: 2 },
      { title: 'Refactor authentication flow components', tag: 'Frontend', tagColor: 'bg-blue-500/10 text-blue-400', priority: 'Medium' as const, assignee: { initials: 'VZ', color: 'bg-violet-500/15 text-violet-400' }, dueDate: 'Jun 12', commentCount: 3 },
    ],
  },
  {
    id: 'done',
    title: 'Done',
    color: 'bg-green-400',
    tasks: [
      { title: 'Set up Vite + React + TypeScript project', tag: 'DevOps', tagColor: 'bg-orange-500/10 text-orange-400', priority: 'High' as const, assignee: { initials: 'VZ', color: 'bg-violet-500/15 text-violet-400' }, dueDate: 'Jun 1', commentCount: 2 },
      { title: 'Configure Tailwind v4 and shadcn/ui', tag: 'Frontend', tagColor: 'bg-blue-500/10 text-blue-400', priority: 'Medium' as const, assignee: { initials: 'JS', color: 'bg-green-500/15 text-green-400' }, dueDate: 'Jun 3', attachmentCount: 1 },
      { title: 'Create CI/CD pipeline with GitHub Actions', tag: 'DevOps', tagColor: 'bg-orange-500/10 text-orange-400', priority: 'High' as const, assignee: { initials: 'AM', color: 'bg-orange-500/15 text-orange-400' }, dueDate: 'Jun 5' },
    ],
  },
]

export default function ProjectPage() {
  return (
    <div className="flex h-screen overflow-hidden bg-background">
      <AppSidebar />

      <div className="flex flex-1 flex-col overflow-hidden">
        <AppTopBar />
        <ProjectHeader />

        <main className="flex-1 overflow-x-auto overflow-y-hidden p-6">
          <div className="flex h-full gap-4">
            {columns.map((col) => (
              <KanbanColumn key={col.id} title={col.title} color={col.color} count={col.tasks.length}>
                {col.tasks.map((task) => (
                  <TaskCard key={task.title} {...task} />
                ))}
              </KanbanColumn>
            ))}
          </div>
        </main>
      </div>
    </div>
  )
}
