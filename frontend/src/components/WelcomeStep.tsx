import { Link } from 'react-router-dom'
import { Sparkles, Kanban, Users } from 'lucide-react'
import { Button } from '@/components/ui/button'

const highlights = [
  { icon: Kanban, text: 'Your first project is ready to go' },
  { icon: Users, text: 'Team invitations have been sent' },
  { icon: Sparkles, text: 'AI assistant is enabled for your workspace' },
]

export default function WelcomeStep() {
  return (
    <div className="flex flex-col items-center gap-6 py-2 text-center">
      <div className="flex h-16 w-16 items-center justify-center rounded-full bg-rose-500/10">
        <span className="text-3xl">🎉</span>
      </div>

      <div>
        <h2 className="text-xl font-semibold text-foreground">You are all set!</h2>
        <p className="mt-1.5 text-sm text-muted-foreground">
          Your workspace is ready. Here is what we have set up for you.
        </p>
      </div>

      <div className="flex w-full flex-col gap-2.5">
        {highlights.map(({ icon: Icon, text }) => (
          <div key={text} className="flex items-center gap-3 rounded-lg border border-border/60 bg-muted/30 px-4 py-3">
            <Icon size={16} className="shrink-0 text-rose-400" />
            <span className="text-sm text-foreground">{text}</span>
          </div>
        ))}
      </div>

      <Link to="/acme-corp/dashboard" className="w-full">
        <Button className="w-full bg-rose-600 hover:bg-rose-700 text-white">
          Go to dashboard
        </Button>
      </Link>
    </div>
  )
}
