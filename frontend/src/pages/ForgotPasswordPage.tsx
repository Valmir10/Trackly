import { Link, useNavigate } from 'react-router-dom'
import { ArrowLeft } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import AuthCard from '@/components/AuthCard'

export default function ForgotPasswordPage() {
  const navigate = useNavigate()

  return (
    <AuthCard title="Reset your password" subtitle="Enter your email and we'll send you a reset link">
      <form onSubmit={(e) => { e.preventDefault(); navigate('/verify-email') }} className="flex flex-col gap-4">
        <div className="flex flex-col gap-1.5">
          <Label htmlFor="email">Email</Label>
          <Input id="email" type="email" placeholder="you@company.com" required />
        </div>

        <Button type="submit" className="mt-1 w-full bg-rose-600 hover:bg-rose-700 text-white">
          Send reset link
        </Button>

        <Link
          to="/login"
          className="flex items-center justify-center gap-1.5 text-sm text-muted-foreground hover:text-foreground transition-colors"
        >
          <ArrowLeft size={14} />
          Back to sign in
        </Link>
      </form>
    </AuthCard>
  )
}
