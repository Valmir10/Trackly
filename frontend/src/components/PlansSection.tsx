import { Check } from 'lucide-react'
import { Link } from 'react-router-dom'
import '@/styles/PlansSection.css'

const tiers = [
  {
    name: 'Starter',
    figure: 'Free',
    figureMono: false,
    caption: 'for every team',
    description: 'Everything you need to run tasks for a small team.',
    cta: 'Get started',
    ctaTo: '/register',
    highlighted: false,
    features: [
      'Up to 3 members',
      '1 project',
      'Kanban board & tasks',
      'Basic task management',
      'Community support',
    ],
  },
  {
    name: 'Growth',
    figure: '10+',
    figureMono: true,
    caption: 'team members',
    description: 'Unlocks automatically once your team grows past ten members.',
    cta: 'Invite your team',
    ctaTo: '/register',
    highlighted: true,
    badge: 'Next milestone',
    features: [
      'Unlimited members',
      'Unlimited projects',
      'Meeting agendas & shared notes',
      'Client Rooms',
      'Smart task automations',
      'Time tracking & analytics',
    ],
  },
  {
    name: 'Scale',
    figure: '30+',
    figureMono: true,
    caption: 'team members',
    description: 'For larger teams running contracts and clients at scale.',
    cta: 'Get in touch',
    ctaTo: '/register',
    highlighted: false,
    features: [
      'Everything in Growth',
      'Contract & milestone tracking',
      'Public REST API & webhooks',
      'SSO / SAML',
      'Audit log',
      'Dedicated support',
    ],
  },
]

export default function PlansSection() {
  return (
    <section id="plans" className="tp-plans">
      <div className="tp-container">
        <div className="tp-plans__intro">
          <h2 className="tp-plans__heading">Start free. Unlock more as your team grows.</h2>
          <p className="tp-plans__subheading">
            No per-seat pricing. Every plan unlocks with your team, not your wallet.
          </p>
        </div>

        <div className="tp-plans__grid">
          {tiers.map((tier) => (
            <div
              key={tier.name}
              className={`tp-plan-card${tier.highlighted ? ' tp-plan-card--highlighted' : ''}`}
            >
              {tier.badge && <span className="tp-plan-card__badge">{tier.badge}</span>}

              <p className="tp-plan-card__name">{tier.name}</p>
              <div className="tp-plan-card__figure-row">
                <span className={`tp-plan-card__figure${tier.figureMono ? ' tp-plan-card__figure--mono' : ''}`}>
                  {tier.figure}
                </span>
                <span className="tp-plan-card__caption">{tier.caption}</span>
              </div>
              <p className="tp-plan-card__description">{tier.description}</p>

              <Link
                to={tier.ctaTo}
                className={`tp-btn tp-plan-card__cta${
                  tier.highlighted ? ' tp-btn--primary' : ' tp-btn--secondary'
                }`}
              >
                {tier.cta}
              </Link>

              <ul className="tp-plan-card__features">
                {tier.features.map((feature) => (
                  <li key={feature}>
                    <Check size={14} />
                    {feature}
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      </div>
    </section>
  )
}
