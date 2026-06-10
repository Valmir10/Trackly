import { Link, useNavigate } from 'react-router-dom'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Separator } from '@/components/ui/separator'
import AuthCard from '@/components/auth/AuthCard'

export default function LoginPage() {
  const navigate = useNavigate()

  return (
    <AuthCard title="Welcome back" subtitle="Sign in to your Trackly account">
      <form onSubmit={(e) => { e.preventDefault(); navigate('/acme-corp/dashboard') }} className="flex flex-col gap-4">
        <div className="flex flex-col gap-1.5">
          <Label htmlFor="email">Email</Label>
          <Input id="email" type="email" placeholder="you@company.com" required />
        </div>

        <div className="flex flex-col gap-1.5">
          <div className="flex items-center justify-between">
            <Label htmlFor="password">Password</Label>
            <Link to="/forgot-password" className="text-xs text-muted-foreground hover:text-foreground transition-colors">
              Forgot password?
            </Link>
          </div>
          <Input id="password" type="password" placeholder="••••••••••" required />
        </div>

        <Button type="submit" className="mt-1 w-full bg-violet-600 hover:bg-violet-700 text-white">
          Sign in
        </Button>

        <Separator />

        <p className="text-center text-sm text-muted-foreground">
          No account?{' '}
          <Link to="/register" className="font-medium text-foreground hover:text-violet-400 transition-colors">
            Create one
          </Link>
        </p>
      </form>
    </AuthCard>
  )
}
