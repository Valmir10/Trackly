import { Link, useNavigate } from 'react-router-dom'
import { Users } from 'lucide-react'
import AuthCard from '@/components/AuthCard'

export default function InvitationPage() {
  const navigate = useNavigate()

  return (
    <AuthCard title="You have been invited" subtitle="Join your team on Trackly">
      <form onSubmit={(e) => { e.preventDefault(); navigate('/acme-corp/dashboard') }} className="tp-auth-form">
        <div className="tp-auth-invite">
          <div className="tp-auth-invite__icon">
            <Users size={16} />
          </div>
          <div>
            <p className="tp-auth-invite__org">Acme Corp</p>
            <p className="tp-auth-invite__by">Invited by John Smith</p>
          </div>
        </div>

        <div className="tp-auth-form__row">
          <div className="tp-field">
            <label className="tp-label" htmlFor="firstName">First name</label>
            <input id="firstName" className="tp-input" placeholder="First name" required />
          </div>
          <div className="tp-field">
            <label className="tp-label" htmlFor="lastName">Last name</label>
            <input id="lastName" className="tp-input" placeholder="Last name" required />
          </div>
        </div>

        <div className="tp-field">
          <label className="tp-label" htmlFor="password">Choose a password</label>
          <input id="password" type="password" className="tp-input" placeholder="Min. 10 characters" required />
        </div>

        <button type="submit" className="tp-btn tp-btn--primary tp-auth-form__submit">
          Accept invitation
        </button>

        <hr className="tp-divider" />

        <p className="tp-auth-form__footer">
          Already have an account? <Link to="/login">Sign in instead</Link>
        </p>
      </form>
    </AuthCard>
  )
}
