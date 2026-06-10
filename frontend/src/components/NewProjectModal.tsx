import { X } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'

const colors = [
  { value: 'rose', class: 'bg-rose-500' },
  { value: 'blue', class: 'bg-blue-500' },
  { value: 'green', class: 'bg-green-500' },
  { value: 'orange', class: 'bg-orange-500' },
  { value: 'violet', class: 'bg-violet-500' },
  { value: 'pink', class: 'bg-pink-500' },
  { value: 'cyan', class: 'bg-cyan-500' },
  { value: 'yellow', class: 'bg-yellow-500' },
]

interface NewProjectModalProps {
  onClose: () => void
}

export default function NewProjectModal({ onClose }: NewProjectModalProps) {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center">
      <div className="absolute inset-0 bg-black/50 backdrop-blur-sm" onClick={onClose} />
      <div className="relative w-full max-w-md rounded-xl border border-border/60 bg-card p-6 shadow-2xl">

        <div className="mb-5 flex items-center justify-between">
          <h2 className="text-base font-semibold text-foreground">New project</h2>
          <button onClick={onClose} className="flex h-7 w-7 items-center justify-center rounded-md text-muted-foreground hover:bg-muted hover:text-foreground transition-colors">
            <X size={15} />
          </button>
        </div>

        <div className="flex flex-col gap-4">
          <div className="flex flex-col gap-1.5">
            <Label>Project name</Label>
            <Input placeholder="e.g. Backend API" autoFocus />
          </div>

          <div className="flex flex-col gap-2">
            <Label>Color</Label>
            <div className="flex gap-2">
              {colors.map((c, i) => (
                <button
                  key={c.value}
                  className={`h-7 w-7 rounded-full ${c.class} transition-transform hover:scale-110 ${
                    i === 0 ? 'ring-2 ring-offset-2 ring-offset-card ring-rose-500' : ''
                  }`}
                />
              ))}
            </div>
          </div>

          <div className="flex flex-col gap-1.5">
            <Label>Description <span className="text-muted-foreground">(optional)</span></Label>
            <textarea
              rows={3}
              placeholder="What is this project about?"
              className="w-full resize-none rounded-md border border-input bg-background px-3 py-2 text-sm text-foreground placeholder:text-muted-foreground outline-none focus:ring-2 focus:ring-ring"
            />
          </div>

          <div className="flex justify-end gap-2 pt-1">
            <Button variant="outline" onClick={onClose}>Cancel</Button>
            <Button className="bg-rose-500 hover:bg-rose-600 text-white" onClick={onClose}>Create project</Button>
          </div>
        </div>

      </div>
    </div>
  )
}
