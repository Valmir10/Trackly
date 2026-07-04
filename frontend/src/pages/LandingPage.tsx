import NavBar from '@/components/NavBar'
import HeroSection from '@/components/HeroSection'
import PillarsSection from '@/components/PillarsSection'
import PlansSection from '@/components/PlansSection'
import CtaSection from '@/components/CtaSection'
import FooterSection from '@/components/FooterSection'
import '@/styles/tokens.css'
import '@/styles/tp-primitives.css'
import '@/styles/LandingPage.css'

export default function LandingPage() {
  return (
    <div className="tp-shell">
      <div className="tp-dot-grid" aria-hidden="true" />
      <NavBar />
      <main>
        <HeroSection />
        <PillarsSection />
        <PlansSection />
        <CtaSection />
      </main>
      <FooterSection />
    </div>
  )
}
