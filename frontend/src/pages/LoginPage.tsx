import { Link, useNavigate } from 'react-router-dom'
import AuthCard from '@/components/AuthCard'

export default function LoginPage() {
  const navigate = useNavigate()

  return (
    <AuthCard title="Welcome back" subtitle="Sign in to your Trackly account">
      <form onSubmit={(e) => { e.preventDefault(); navigate('/acme-corp/dashboard') }} className="tp-auth-form">
        <div className="tp-field">
          <label className="tp-label" htmlFor="email">Email</label>
          <input id="email" type="email" className="tp-input" placeholder="you@company.com" required />
        </div>

        <div className="tp-field">
          <div className="tp-field__row">
            <label className="tp-label" htmlFor="password">Password</label>
            <Link to="/forgot-password" className="tp-auth-form__forgot">Forgot password?</Link>
          </div>
          <input id="password" type="password" className="tp-input" placeholder="••••••••••" required />
        </div>

        <button type="submit" className="tp-btn tp-btn--primary tp-auth-form__submit">
          Sign in
        </button>

        <hr className="tp-divider" />

        <p className="tp-auth-form__footer">
          No account? <Link to="/register">Create one</Link>
        </p>
      </form>
    </AuthCard>
  )
}
