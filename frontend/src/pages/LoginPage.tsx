import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { AxiosError } from 'axios'
import AuthCard from '@/components/AuthCard'
import { apiClient } from '@/lib/apiClient'
import { useAuthStore } from '@/store/useAuthStore'

export default function LoginPage() {
  const navigate = useNavigate()
  const setSession = useAuthStore((s) => s.setSession)

  const [tenantSlug, setTenantSlug] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    setError(null)
    setIsSubmitting(true)

    try {
      const response = await apiClient.post('/api/auth/login', { tenantSlug, email, password })
      setSession({
        accessToken: response.data.accessToken,
        userId: response.data.userId,
        tenantId: response.data.tenantId,
        tenantSlug,
      })
      navigate(`/${tenantSlug}/dashboard`)
    } catch (err) {
      const status = err instanceof AxiosError ? err.response?.status : null
      setError(status === 401 ? 'Invalid workspace, email, or password.' : 'Something went wrong. Try again.')
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <AuthCard title="Welcome back" subtitle="Sign in to your Trackly account">
      <form onSubmit={handleSubmit} className="tp-auth-form">
        <div className="tp-field">
          <label className="tp-label" htmlFor="tenantSlug">Workspace</label>
          <input
            id="tenantSlug"
            className="tp-input"
            placeholder="acme-corp"
            value={tenantSlug}
            onChange={(e) => setTenantSlug(e.target.value)}
            required
          />
        </div>

        <div className="tp-field">
          <label className="tp-label" htmlFor="email">Email</label>
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
          <div className="tp-field__row">
            <label className="tp-label" htmlFor="password">Password</label>
            <Link to="/forgot-password" className="tp-auth-form__forgot">Forgot password?</Link>
          </div>
          <input
            id="password"
            type="password"
            className="tp-input"
            placeholder="••••••••••"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>

        {error && <p className="tp-field__error">{error}</p>}

        <button type="submit" className="tp-btn tp-btn--primary tp-auth-form__submit" disabled={isSubmitting}>
          {isSubmitting ? 'Signing in…' : 'Sign in'}
        </button>

        <hr className="tp-divider" />

        <p className="tp-auth-form__footer">
          No account? <Link to="/register">Create one</Link>
        </p>
      </form>
    </AuthCard>
  )
}
