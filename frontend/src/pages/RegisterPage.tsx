import { Link, useNavigate } from 'react-router-dom'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Separator } from '@/components/ui/separator'
import AuthCard from '@/components/auth/AuthCard'

export default function RegisterPage() {
  const navigate = useNavigate()

  return (
    <AuthCard title="Create your account" subtitle="Start managing projects with your team">
      <form onSubmit={(e) => { e.preventDefault(); navigate('/verify-email') }} className="flex flex-col gap-4">
        <div className="grid grid-cols-2 gap-3">
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="firstName">First name</Label>
            <Input id="firstName" placeholder="Valmir" required />
          </div>
          <div className="flex flex-col gap-1.5">
            <Label htmlFor="lastName">Last name</Label>
            <Input id="lastName" placeholder="Zogaj" required />
          </div>
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="company">Company name</Label>
          <Input id="company" placeholder="Acme Corp" required />
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="email">Work email</Label>
          <Input id="email" type="email" placeholder="you@company.com" required />
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="password">Password</Label>
          <Input id="password" type="password" placeholder="Min. 10 characters" required />
        </div>

        <Button type="submit" className="mt-1 w-full bg-violet-600 hover:bg-violet-700 text-white">
          Create account
        </Button>

        <p className="text-center text-xs text-muted-foreground leading-relaxed">
          By creating an account you agree to our{' '}
          <a href="#" className="underline underline-offset-2 hover:text-foreground transition-colors">Terms of Service</a>
          {' '}and{' '}
          <a href="#" className="underline underline-offset-2 hover:text-foreground transition-colors">Privacy Policy</a>.
        </p>

        <Separator />

        <p className="text-center text-sm text-muted-foreground">
          Already have an account?{' '}
          <Link to="/login" className="font-medium text-foreground hover:text-violet-400 transition-colors">
            Sign in
          </Link>
        </p>
      </form>
    </AuthCard>
  )
}
