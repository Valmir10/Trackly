import { Link } from 'react-router-dom'
import { Users } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Separator } from '@/components/ui/separator'
import AuthCard from '@/components/auth/AuthCard'

export default function InvitationPage() {
  return (
    <AuthCard
      title="You have been invited"
      subtitle="Join your team on Trackly"
    >
      <div className="flex flex-col gap-4">
        <div className="flex items-center gap-3 rounded-lg border border-border/60 bg-muted/30 p-3">
          <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-violet-500/10">
            <Users size={16} className="text-violet-400" />
          </div>
          <div>
            <p className="text-sm font-medium text-foreground">Acme Corp</p>
            <p className="text-xs text-muted-foreground">Invited by John Smith</p>
          </div>
        </div>

        <div className="grid grid-cols-2 gap-3">
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="firstName">First name</Label>
            <Input id="firstName" placeholder="Valmir" />
          </div>
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="lastName">Last name</Label>
            <Input id="lastName" placeholder="Zogaj" />
          </div>
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="password">Choose a password</Label>
          <Input id="password" type="password" placeholder="Min. 10 characters" />
        </div>

        <Button className="mt-1 w-full bg-violet-600 hover:bg-violet-700 text-white">
          Accept invitation
        </Button>

        <Separator />

        <p className="text-center text-sm text-muted-foreground">
          Already have an account?{' '}
          <Link to="/login" className="font-medium text-foreground hover:text-violet-400 transition-colors">
            Sign in instead
          </Link>
        </p>
      </div>
    </AuthCard>
  )
}
