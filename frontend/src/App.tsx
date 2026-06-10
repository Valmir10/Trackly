import { BrowserRouter, Routes, Route } from 'react-router-dom'
import LandingPage from '@/pages/Landing/LandingPage'
import LoginPage from '@/pages/Auth/LoginPage'
import RegisterPage from '@/pages/Auth/RegisterPage'
import ForgotPasswordPage from '@/pages/Auth/ForgotPasswordPage'
import EmailVerificationPage from '@/pages/Auth/EmailVerificationPage'
import InvitationPage from '@/pages/Auth/InvitationPage'
import OnboardingPage from '@/pages/Onboarding/OnboardingPage'
import DashboardPage from '@/pages/Dashboard/DashboardPage'
import ProjectsPage from '@/pages/Projects/ProjectsPage'
import ProjectPage from '@/pages/Project/ProjectPage'
import SettingsPage from '@/pages/Settings/SettingsPage'
import NotFoundPage from '@/pages/NotFound/NotFoundPage'

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<LandingPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/forgot-password" element={<ForgotPasswordPage />} />
        <Route path="/verify-email" element={<EmailVerificationPage />} />
        <Route path="/invite/:token" element={<InvitationPage />} />
        <Route path="/onboarding" element={<OnboardingPage />} />
        <Route path="/:slug/dashboard" element={<DashboardPage />} />
        <Route path="/:slug/projects" element={<ProjectsPage />} />
        <Route path="/:slug/projects/:projectId" element={<ProjectPage />} />
        <Route path="/:slug/settings" element={<SettingsPage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </BrowserRouter>
  )
}
