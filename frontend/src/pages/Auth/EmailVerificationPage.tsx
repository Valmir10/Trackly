import { Link } from 'react-router-dom'
import { MailCheck, ArrowLeft } from 'lucide-react'
import { Button } from '@/components/ui/button'
import AuthCard from './components/AuthCard'

export default function EmailVerificationPage() {
  return (
    <AuthCard
      title="Check your email"
      subtitle="We sent a verification link to your inbox"
    >
      <div className="flex flex-col items-center gap-6">
        <div className="flex h-16 w-16 items-center justify-center rounded-full bg-violet-500/10">
          <MailCheck size={28} className="text-violet-400" />
        </div>

        <div className="w-full rounded-lg border border-border/60 bg-muted/30 px-4 py-3 text-center">
          <p className="text-sm text-muted-foreground">
            Verification link sent to
          </p>
          <p className="mt-0.5 text-sm font-medium text-foreground">
            you@company.com
          </p>
        </div>

        <div className="flex w-full flex-col gap-3">
          <Button variant="outline" className="w-full">
            Resend email
          </Button>

          <Link
            to="/login"
            className="flex items-center justify-center gap-1.5 text-sm text-muted-foreground hover:text-foreground transition-colors"
          >
            <ArrowLeft size={14} />
            Back to sign in
          </Link>
        </div>

        <p className="text-center text-xs text-muted-foreground">
          Check your spam folder if you do not see it within a few minutes.
        </p>
      </div>
    </AuthCard>
  )
}
