import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { AxiosError } from 'axios'
import AuthCard from '@/components/AuthCard'
import { apiClient } from '@/lib/apiClient'
import { useAuthStore } from '@/store/useAuthStore'
import { slugify } from '@/lib/slugify'

export default function RegisterPage() {
  const navigate = useNavigate()
  const setSession = useAuthStore((s) => s.setSession)

  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [company, setCompany] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    setError(null)
    setIsSubmitting(true)

    const tenantSlug = slugify(company)

    try {
      const response = await apiClient.post('/api/auth/register', {
        tenantName: company,
        tenantSlug,
        email,
        password,
        firstName,
        lastName,
      })
      setSession({
        accessToken: response.data.accessToken,
        userId: response.data.userId,
        tenantId: response.data.tenantId,
        tenantSlug,
      })
      navigate(`/${tenantSlug}/dashboard`)
    } catch (err) {
      const status = err instanceof AxiosError ? err.response?.status : null
      if (status === 409) {
        setError('That workspace name is already taken.')
      } else if (status === 400) {
        setError('Check your details — something didn’t pass validation.')
      } else {
        setError('Something went wrong. Try again.')
      }
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <AuthCard title="Create your account" subtitle="Run your team's work in one place">
      <form onSubmit={handleSubmit} className="tp-auth-form">
        <div className="tp-auth-form__row">
          <div className="tp-field">
            <label className="tp-label" htmlFor="firstName">First name</label>
            <input
              id="firstName"
              className="tp-input"
              placeholder="First name"
              value={firstName}
              onChange={(e) => setFirstName(e.target.value)}
              required
            />
          </div>
          <div className="tp-field">
            <label className="tp-label" htmlFor="lastName">Last name</label>
            <input
              id="lastName"
              className="tp-input"
              placeholder="Last name"
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
              required
            />
          </div>
        </div>

        <div className="tp-field">
          <label className="tp-label" htmlFor="company">Company name</label>
          <input
            id="company"
            className="tp-input"
            placeholder="Your company"
            value={company}
            onChange={(e) => setCompany(e.target.value)}
            required
          />
        </div>

        <div className="tp-field">
          <label className="tp-label" htmlFor="email">Work email</label>
          <input
            id="email"
            type="email"
            className="tp-input"
            placeholder="you@company.com"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>

        <div className="tp-field">
          <label className="tp-label" htmlFor="password">Password</label>
          <input
            id="password"
            type="password"
            className="tp-input"
            placeholder="Min. 10 characters"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            minLength={10}
            required
          />
        </div>

        {error && <p className="tp-field__error">{error}</p>}

        <button type="submit" className="tp-btn tp-btn--primary tp-auth-form__submit" disabled={isSubmitting}>
          {isSubmitting ? 'Creating account…' : 'Create account'}
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
