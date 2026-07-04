import { Link } from 'react-router-dom'
import { MailCheck, ArrowLeft } from 'lucide-react'
import AuthCard from '@/components/AuthCard'

export default function EmailVerificationPage() {
  return (
    <AuthCard title="Check your email" subtitle="We sent a verification link to your inbox">
      <div className="tp-auth-form">
        <div className="tp-auth-icon">
          <MailCheck size={28} />
        </div>

        <div className="tp-auth-notice">
          <p className="tp-auth-notice__label">Verification link sent to</p>
          <p className="tp-auth-notice__value">you@company.com</p>
        </div>

        <button type="button" className="tp-btn tp-btn--secondary tp-auth-form__submit">
          Resend email
        </button>

        <Link to="/login" className="tp-auth-form__back">
          <ArrowLeft size={14} />
          Back to sign in
        </Link>

        <p className="tp-auth-form__terms">
          Check your spam folder if you do not see it within a few minutes.
        </p>
      </div>
    </AuthCard>
  )
}
