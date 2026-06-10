import NavBar from './components/NavBar'
import HeroSection from './components/HeroSection'
import FeaturesSection from './components/FeaturesSection'
import AiSection from './components/AiSection'
import PricingSection from './components/PricingSection'
import CtaSection from './components/CtaSection'
import FooterSection from './components/FooterSection'

export default function LandingPage() {
  return (
    <div className="min-h-screen bg-background">
      <NavBar />
      <main>
        <HeroSection />
        <FeaturesSection />
        <AiSection />
        <PricingSection />
        <CtaSection />
      </main>
      <FooterSection />
    </div>
  )
}
