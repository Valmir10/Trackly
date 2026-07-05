import { useSearchParams } from 'react-router-dom'
import AppShell from '@/components/AppShell'
import ProjectHeader from '@/components/ProjectHeader'
import KanbanColumn from '@/components/KanbanColumn'
import TaskCard from '@/components/TaskCard'
import TicketModal from '@/components/TicketModal'
import { useTaskStore, findTask } from '@/store/useTaskStore'

export default function ProjectPage() {
  const columns = useTaskStore((s) => s.columns)
  const moveTask = useTaskStore((s) => s.moveTask)
  const [searchParams, setSearchParams] = useSearchParams()

  const activeTicketId = searchParams.get('ticket')

  function openTicket(taskId: string) {
    setSearchParams((prev) => {
      const next = new URLSearchParams(prev)
      next.set('ticket', taskId)
      return next
    })
  }

  function closeTicket() {
    setSearchParams((prev) => {
      const next = new URLSearchParams(prev)
      next.delete('ticket')
      return next
    })
  }

  const found = activeTicketId ? findTask(columns, activeTicketId) : undefined

  return (
    <AppShell>
      <div className="flex h-full flex-col overflow-hidden">
        <ProjectHeader name="Frontend redesign" dotColor="var(--tp-cat-1)" />
        <div className="flex-1 overflow-x-auto overflow-y-hidden p-6">
          <div className="flex h-full gap-4">
            {columns.map((col) => (
              <KanbanColumn key={col.id} title={col.title} color={col.color} count={col.tasks.length}>
                {col.tasks.map((task) => (
                  <TaskCard key={task.id} task={task} onOpen={openTicket} />
                ))}
              </KanbanColumn>
            ))}
          </div>
        </div>
      </div>

      {found && (
        <TicketModal
          task={found.task}
          statuses={columns.map((c) => c.title)}
          currentStatus={found.column.title}
          onStatusChange={(status) => {
            const target = columns.find((c) => c.title === status)
            if (target) moveTask(found.task.id, target.id)
          }}
          onClose={closeTicket}
        />
      )}
    </AppShell>
  )
}
