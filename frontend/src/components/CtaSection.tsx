import { Link } from 'react-router-dom'
import { ArrowRight } from 'lucide-react'
import '@/styles/CtaSection.css'

export default function CtaSection() {
  return (
    <section className="tp-cta-section">
      <div className="tp-container">
        <div className="tp-cta">
          <h2 className="tp-cta__heading">Bring your team&rsquo;s work into one place</h2>
          <p className="tp-cta__subheading">
            Tasks, meetings, clients, and contracts, all together in one workspace.
            Free to get started, no credit card required.
          </p>

          <div className="tp-cta__actions">
            <Link to="/register" className="tp-btn tp-btn--primary tp-btn--lg">
              Get started for free
              <ArrowRight size={16} />
            </Link>
            <Link to="/login" className="tp-btn tp-btn--ghost tp-btn--lg">
              Sign in to existing account
            </Link>
          </div>
        </div>
      </div>
    </section>
  )
}
