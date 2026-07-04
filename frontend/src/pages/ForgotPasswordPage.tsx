import { Link, useNavigate } from 'react-router-dom'
import { ArrowLeft } from 'lucide-react'
import AuthCard from '@/components/AuthCard'

export default function ForgotPasswordPage() {
  const navigate = useNavigate()

  return (
    <AuthCard title="Reset your password" subtitle="Enter your email and we'll send you a reset link">
      <form onSubmit={(e) => { e.preventDefault(); navigate('/verify-email') }} className="tp-auth-form">
        <div className="tp-field">
          <label className="tp-label" htmlFor="email">Email</label>
          <input id="email" type="email" className="tp-input" placeholder="you@company.com" required />
        </div>

        <button type="submit" className="tp-btn tp-btn--primary tp-auth-form__submit">
          Send reset link
        </button>

        <Link to="/login" className="tp-auth-form__back">
          <ArrowLeft size={14} />
          Back to sign in
        </Link>
      </form>
    </AuthCard>
  )
}
