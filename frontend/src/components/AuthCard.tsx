import { Link } from 'react-router-dom'
import type { ReactNode } from 'react'
import '@/styles/tokens.css'
import '@/styles/tp-primitives.css'
import '@/styles/AuthCard.css'

interface AuthCardProps {
  title: string
  subtitle: string
  children: ReactNode
}

export default function AuthCard({ title, subtitle, children }: AuthCardProps) {
  return (
    <div className="tp-shell tp-auth">
      <div className="tp-auth__grid" aria-hidden="true" />
      <div className="tp-auth__glow" aria-hidden="true" />

      <Link to="/" className="tp-auth__brand">
        <span className="tp-auth__mark">T</span>
        <span className="tp-auth__name">Trackly</span>
      </Link>

      <div className="tp-auth__card">
        <div className="tp-auth__header">
          <h1 className="tp-auth__title">{title}</h1>
          <p className="tp-auth__subtitle">{subtitle}</p>
        </div>
        {children}
      </div>
    </div>
  )
}
