import { Link } from 'react-router-dom'
import { ArrowRight } from 'lucide-react'
import { Button } from '@/components/ui/button'

export default function CtaSection() {
  return (
    <section className="py-24 border-t border-border/40">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">

        <div className="relative overflow-hidden rounded-2xl border border-violet-500/20 bg-violet-500/5 px-8 py-16 text-center">
          <div className="absolute inset-0 -z-10">
            <div className="absolute left-1/2 top-1/2 h-64 w-96 -translate-x-1/2 -translate-y-1/2 rounded-full bg-violet-600/10 blur-3xl" />
          </div>

          <h2 className="text-3xl font-semibold tracking-tight text-foreground sm:text-4xl">
            Start managing projects smarter
          </h2>
          <p className="mx-auto mt-4 max-w-xl text-muted-foreground">
            Join teams already using Trackly to ship faster with AI-assisted project management.
            Free to get started — no credit card required.
          </p>

          <div className="mt-8 flex flex-wrap items-center justify-center gap-4">
            <Link to="/register">
              <Button size="lg" className="bg-violet-600 hover:bg-violet-700 text-white gap-2 px-8">
                Get started for free
                <ArrowRight size={16} />
              </Button>
            </Link>
            <Link to="/login">
              <Button size="lg" variant="ghost">
                Sign in to existing account
              </Button>
            </Link>
          </div>
        </div>

      </div>
    </section>
  )
}
