import { Link } from 'react-router-dom'
import { CheckSquare } from 'lucide-react'
import { Avatar, AvatarFallback } from '@/components/ui/avatar'
import { Badge } from '@/components/ui/badge'

interface ProjectCardProps {
  id: string
  name: string
  description: string
  color: string
  memberCount: number
  taskCount: number
  completedCount: number
  members: { initials: string; color: string }[]
  status: 'Active' | 'On hold' | 'Completed'
}

export default function ProjectCard({ id, name, description, color, taskCount, completedCount, members, status }: ProjectCardProps) {
  const progress = taskCount > 0 ? Math.round((completedCount / taskCount) * 100) : 0

  return (
    <Link to={`/acme-corp/projects/${id}`} className="group flex flex-col rounded-xl border border-border/60 bg-card hover:border-border transition-all duration-200 overflow-hidden">
      <div className={`h-1.5 w-full ${color}`} />
      <div className="flex flex-col gap-3 p-5">
        <div className="flex items-start justify-between gap-2">
          <h3 className="text-sm font-semibold text-foreground group-hover:text-rose-400 transition-colors leading-snug">
            {name}
          </h3>
          <Badge
            variant="outline"
            className={`shrink-0 text-[10px] ${
              status === 'Active' ? 'border-green-500/30 text-green-400' :
              status === 'On hold' ? 'border-yellow-500/30 text-yellow-400' :
              'border-border text-muted-foreground'
            }`}
          >
            {status}
          </Badge>
        </div>

        <p className="text-xs text-muted-foreground leading-relaxed line-clamp-2">{description}</p>

        <div className="flex flex-col gap-1.5">
          <div className="flex items-center justify-between text-[10px] text-muted-foreground">
            <span>{completedCount}/{taskCount} tasks</span>
            <span>{progress}%</span>
          </div>
          <div className="h-1 w-full rounded-full bg-muted overflow-hidden">
            <div className={`h-full ${color} rounded-full`} style={{ width: `${progress}%` }} />
          </div>
        </div>

        <div className="flex items-center justify-between">
          <div className="flex -space-x-1.5">
            {members.slice(0, 4).map((m, i) => (
              <Avatar key={i} className="h-6 w-6 border-2 border-card">
                <AvatarFallback className={`text-[9px] font-semibold ${m.color}`}>{m.initials}</AvatarFallback>
              </Avatar>
            ))}
            {members.length > 4 && (
              <div className="flex h-6 w-6 items-center justify-center rounded-full border-2 border-card bg-muted text-[9px] text-muted-foreground">
                +{members.length - 4}
              </div>
            )}
          </div>
          <div className="flex items-center gap-1 text-[10px] text-muted-foreground">
            <CheckSquare size={11} />
            <span>{taskCount - completedCount} open</span>
          </div>
        </div>
      </div>
    </Link>
  )
}
