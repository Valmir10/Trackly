import { Check } from 'lucide-react'

interface Step {
  label: string
}

interface StepIndicatorProps {
  steps: Step[]
  currentStep: number
}

export default function StepIndicator({ steps, currentStep }: StepIndicatorProps) {
  return (
    <div className="flex items-center gap-0">
      {steps.map((step, index) => {
        const isDone = index < currentStep
        const isActive = index === currentStep

        return (
          <div key={step.label} className="flex items-center">
            <div className="flex flex-col items-center gap-1.5">
              <div
                className={`flex h-8 w-8 items-center justify-center rounded-full border-2 text-xs font-semibold transition-colors ${
                  isDone
                    ? 'border-violet-600 bg-violet-600 text-white'
                    : isActive
                    ? 'border-violet-600 bg-background text-violet-400'
                    : 'border-border bg-background text-muted-foreground'
                }`}
              >
                {isDone ? <Check size={14} /> : <span>{index + 1}</span>}
              </div>
              <span
                className={`text-[11px] font-medium whitespace-nowrap ${
                  isActive ? 'text-foreground' : 'text-muted-foreground'
                }`}
              >
                {step.label}
              </span>
            </div>

            {index < steps.length - 1 && (
              <div
                className={`mb-5 h-px w-16 mx-2 transition-colors ${
                  isDone ? 'bg-violet-600' : 'bg-border'
                }`}
              />
            )}
          </div>
        )
      })}
    </div>
  )
}
