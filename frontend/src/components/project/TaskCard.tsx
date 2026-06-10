import { Calendar, MessageSquare, Paperclip } from 'lucide-react'
import { Avatar, AvatarFallback } from '@/components/ui/avatar'

interface TaskCardProps {
  title: string
  tag: string
  tagColor: string
  priority: 'High' | 'Medium' | 'Low'
  assignee: { initials: string; color: string }
  dueDate: string
  dueSoon?: boolean
  commentCount?: number
  attachmentCount?: number
}

const priorityStyles = {
  High: 'bg-red-500',
  Medium: 'bg-yellow-500',
  Low: 'bg-green-500',
}

export default function TaskCard({ title, tag, tagColor, priority, assignee, dueDate, dueSoon, commentCount, attachmentCount }: TaskCardProps) {
  return (
    <div className="group rounded-lg border border-border/60 bg-background p-3 hover:border-border hover:shadow-sm transition-all duration-150 cursor-pointer">
      <div className="mb-2 flex items-start justify-between gap-2">
        <p className="text-xs font-medium text-foreground leading-snug">{title}</p>
        <div className={`mt-0.5 h-1.5 w-1.5 shrink-0 rounded-full ${priorityStyles[priority]}`} />
      </div>

      <div className="mb-3 flex flex-wrap gap-1.5">
        <span className={`rounded px-1.5 py-0.5 text-[10px] font-medium ${tagColor}`}>{tag}</span>
      </div>

      <div className="flex items-center justify-between">
        <Avatar className="h-5 w-5">
          <AvatarFallback className={`text-[9px] font-semibold ${assignee.color}`}>
            {assignee.initials}
          </AvatarFallback>
        </Avatar>

        <div className="flex items-center gap-2.5">
          {commentCount ? (
            <div className="flex items-center gap-0.5 text-[10px] text-muted-foreground">
              <MessageSquare size={10} />
              <span>{commentCount}</span>
            </div>
          ) : null}
          {attachmentCount ? (
            <div className="flex items-center gap-0.5 text-[10px] text-muted-foreground">
              <Paperclip size={10} />
              <span>{attachmentCount}</span>
            </div>
          ) : null}
          <div className={`flex items-center gap-0.5 text-[10px] ${dueSoon ? 'text-red-400' : 'text-muted-foreground'}`}>
            <Calendar size={10} />
            <span>{dueDate}</span>
          </div>
        </div>
      </div>
    </div>
  )
}
