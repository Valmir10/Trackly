import { Link, useNavigate } from 'react-router-dom'
import AuthCard from '@/components/AuthCard'

export default function RegisterPage() {
  const navigate = useNavigate()

  return (
    <AuthCard title="Create your account" subtitle="Run your team's work in one place">
      <form onSubmit={(e) => { e.preventDefault(); navigate('/acme-corp/dashboard') }} className="tp-auth-form">
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
          <label className="tp-label" htmlFor="company">Company name</label>
          <input id="company" className="tp-input" placeholder="Your company" required />
        </div>

        <div className="tp-field">
          <label className="tp-label" htmlFor="email">Work email</label>
          <input id="email" type="email" className="tp-input" placeholder="you@company.com" required />
        </div>

        <div className="tp-field">
          <label className="tp-label" htmlFor="password">Password</label>
          <input id="password" type="password" className="tp-input" placeholder="Min. 10 characters" required />
        </div>

        <button type="submit" className="tp-btn tp-btn--primary tp-auth-form__submit">
          Create account
        </button>

        <p className="tp-auth-form__terms">
          By creating an account you agree to our{' '}
          <a href="#">Terms of Service</a> and <a href="#">Privacy Policy</a>.
        </p>

        <hr className="tp-divider" />

        <p className="tp-auth-form__footer">
          Already have an account? <Link to="/login">Sign in</Link>
        </p>
      </form>
    </AuthCard>
  )
}
