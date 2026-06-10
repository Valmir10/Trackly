import { useState } from 'react'
import { Link } from 'react-router-dom'
import { ArrowLeft, ArrowRight } from 'lucide-react'
import { Button } from '@/components/ui/button'
import StepIndicator from '@/components/onboarding/StepIndicator'
import WorkspaceStep from '@/components/onboarding/WorkspaceStep'
import InviteStep from '@/components/onboarding/InviteStep'
import ProjectStep from '@/components/onboarding/ProjectStep'
import WelcomeStep from '@/components/onboarding/WelcomeStep'

const steps = [
  { label: 'Workspace' },
  { label: 'Team' },
  { label: 'Project' },
  { label: 'Done' },
]

const stepComponents = [WorkspaceStep, InviteStep, ProjectStep, WelcomeStep]

export default function OnboardingPage() {
  const [currentStep, setCurrentStep] = useState(0)
  const isLastStep = currentStep === steps.length - 1
  const isFirstStep = currentStep === 0
  const StepContent = stepComponents[currentStep]

  return (
    <div className="flex min-h-screen flex-col items-center justify-center bg-background px-4 py-12">
      <div className="absolute inset-0 -z-10">
        <div className="absolute left-1/2 top-1/3 h-[400px] w-[700px] -translate-x-1/2 -translate-y-1/2 rounded-full bg-violet-600/8 blur-3xl" />
      </div>

      <Link to="/" className="mb-10 flex items-center gap-2.5">
        <div className="flex h-8 w-8 items-center justify-center rounded-lg bg-violet-600">
          <span className="text-sm font-bold text-white">T</span>
        </div>
        <span className="text-base font-semibold text-foreground tracking-tight">Trackly</span>
      </Link>

      <StepIndicator steps={steps} currentStep={currentStep} />

      <div className="mt-8 w-full max-w-md rounded-xl border border-border/60 bg-card p-8 shadow-xl">
        <StepContent />

        {!isLastStep && (
          <div className="mt-8 flex items-center justify-between">
            {!isFirstStep ? (
              <Button
                variant="ghost"
                size="sm"
                onClick={() => setCurrentStep(s => s - 1)}
                className="gap-1.5 text-muted-foreground"
              >
                <ArrowLeft size={14} />
                Back
              </Button>
            ) : (
              <div />
            )}

            <div className="flex items-center gap-3">
              {currentStep === 1 && (
                <Button
                  variant="ghost"
                  size="sm"
                  onClick={() => setCurrentStep(s => s + 1)}
                  className="text-muted-foreground"
                >
                  Skip for now
                </Button>
              )}
              <Button
                size="sm"
                onClick={() => setCurrentStep(s => s + 1)}
                className="bg-violet-600 hover:bg-violet-700 text-white gap-1.5"
              >
                {currentStep === steps.length - 2 ? 'Finish setup' : 'Continue'}
                <ArrowRight size={14} />
              </Button>
            </div>
          </div>
        )}
      </div>

      <p className="mt-6 text-xs text-muted-foreground">
        Step {currentStep + 1} of {steps.length}
      </p>
    </div>
  )
}
