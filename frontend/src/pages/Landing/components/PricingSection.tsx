import { Check } from 'lucide-react'
import { Link } from 'react-router-dom'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'

const tiers = [
  {
    name: 'Free',
    price: '$0',
    period: 'forever',
    description: 'For individuals and small teams getting started.',
    cta: 'Get started',
    ctaTo: '/register',
    highlighted: false,
    features: [
      'Up to 3 users',
      '1 project',
      'Kanban board',
      'Basic task management',
      'Community support',
    ],
  },
  {
    name: 'Pro',
    price: '$15',
    period: 'per user / month',
    description: 'For growing teams that need AI and unlimited projects.',
    cta: 'Start free trial',
    ctaTo: '/register',
    highlighted: true,
    badge: 'Most popular',
    features: [
      'Up to 20 users',
      'Unlimited projects',
      'AI task breakdown',
      'AI chat & summaries',
      'Time tracking',
      'Analytics dashboard',
      'Public REST API',
      'Webhooks',
      'Priority support',
    ],
  },
  {
    name: 'Enterprise',
    price: '$40',
    period: 'per user / month',
    description: 'For large organisations with advanced security needs.',
    cta: 'Contact us',
    ctaTo: '/register',
    highlighted: false,
    features: [
      'Unlimited users',
      'Everything in Pro',
      'SSO / SAML',
      'Higher AI quota',
      'Audit log',
      'Custom contracts',
      'Dedicated support',
    ],
  },
]

export default function PricingSection() {
  return (
    <section id="pricing" className="py-24 border-t border-border/40">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">

        <div className="mb-16 text-center">
          <p className="mb-3 text-sm font-medium text-violet-400 uppercase tracking-wider">Pricing</p>
          <h2 className="text-3xl font-semibold tracking-tight text-foreground sm:text-4xl">
            Simple, transparent pricing
          </h2>
          <p className="mt-4 text-muted-foreground">
            Start free. Upgrade when your team is ready.
          </p>
        </div>

        <div className="grid gap-6 lg:grid-cols-3">
          {tiers.map((tier) => (
            <div
              key={tier.name}
              className={`relative flex flex-col rounded-xl border p-8 ${
                tier.highlighted
                  ? 'border-violet-500/60 bg-violet-500/5 shadow-xl shadow-violet-500/10'
                  : 'border-border/60 bg-card'
              }`}
            >
              {tier.badge && (
                <Badge className="absolute -top-3 left-1/2 -translate-x-1/2 bg-violet-600 text-white hover:bg-violet-600 text-xs">
                  {tier.badge}
                </Badge>
              )}

              <div className="mb-6">
                <p className="text-sm font-medium text-foreground">{tier.name}</p>
                <div className="mt-3 flex items-baseline gap-1">
                  <span className="text-4xl font-semibold tracking-tight text-foreground">{tier.price}</span>
                  <span className="text-sm text-muted-foreground">{tier.period}</span>
                </div>
                <p className="mt-3 text-sm text-muted-foreground">{tier.description}</p>
              </div>

              <Link to={tier.ctaTo} className="mb-8">
                <Button
                  className={`w-full ${
                    tier.highlighted
                      ? 'bg-violet-600 hover:bg-violet-700 text-white'
                      : ''
                  }`}
                  variant={tier.highlighted ? 'default' : 'outline'}
                >
                  {tier.cta}
                </Button>
              </Link>

              <ul className="flex flex-col gap-3">
                {tier.features.map((feature) => (
                  <li key={feature} className="flex items-center gap-2.5 text-sm text-muted-foreground">
                    <Check size={14} className="shrink-0 text-violet-400" />
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
