import { BrowserRouter, Routes, Route } from 'react-router-dom'
import LandingPage from '@/pages/LandingPage'
import LoginPage from '@/pages/LoginPage'
import RegisterPage from '@/pages/RegisterPage'
import ForgotPasswordPage from '@/pages/ForgotPasswordPage'
import EmailVerificationPage from '@/pages/EmailVerificationPage'
import InvitationPage from '@/pages/InvitationPage'
import OnboardingPage from '@/pages/OnboardingPage'
import DashboardPage from '@/pages/DashboardPage'
import ProjectsPage from '@/pages/ProjectsPage'
import ProjectPage from '@/pages/ProjectPage'
import MyTasksPage from '@/pages/MyTasksPage'
import AnalyticsPage from '@/pages/AnalyticsPage'
import SettingsPage from '@/pages/SettingsPage'
import NotFoundPage from '@/pages/NotFoundPage'
import RequireAuth from '@/components/RequireAuth'

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
        <Route path="/:slug/dashboard" element={<RequireAuth><DashboardPage /></RequireAuth>} />
        <Route path="/:slug/projects" element={<RequireAuth><ProjectsPage /></RequireAuth>} />
        <Route path="/:slug/projects/:projectId" element={<RequireAuth><ProjectPage /></RequireAuth>} />
        <Route path="/:slug/tasks" element={<RequireAuth><MyTasksPage /></RequireAuth>} />
        <Route path="/:slug/analytics" element={<RequireAuth><AnalyticsPage /></RequireAuth>} />
        <Route path="/:slug/settings" element={<RequireAuth><SettingsPage /></RequireAuth>} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </BrowserRouter>
  )
}
