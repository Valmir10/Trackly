import { FolderKanban } from 'lucide-react'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'

const colors = [
  { value: 'rose', class: 'bg-rose-500' },
  { value: 'blue', class: 'bg-blue-500' },
  { value: 'green', class: 'bg-green-500' },
  { value: 'yellow', class: 'bg-yellow-500' },
  { value: 'red', class: 'bg-red-500' },
  { value: 'pink', class: 'bg-pink-500' },
  { value: 'orange', class: 'bg-orange-500' },
  { value: 'cyan', class: 'bg-cyan-500' },
]

export default function ProjectStep() {
  return (
    <div className="flex flex-col gap-5">
      <div className="flex items-center gap-3">
        <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-rose-500/10">
          <FolderKanban size={20} className="text-rose-400" />
        </div>
        <div>
          <h2 className="text-base font-semibold text-foreground">Create your first project</h2>
          <p className="text-sm text-muted-foreground">You can create more projects from the dashboard later.</p>
        </div>
      </div>

      <div className="flex flex-col gap-4">
        <div className="flex flex-col gap-1.5">
          <Label htmlFor="projectName">Project name</Label>
          <Input id="projectName" placeholder="e.g. Frontend redesign" />
        </div>

        <div className="flex flex-col gap-2">
          <Label>Project color</Label>
          <div className="flex gap-2">
            {colors.map((color, i) => (
              <button
                key={color.value}
                className={`h-7 w-7 rounded-full ${color.class} transition-transform hover:scale-110 ${
                  i === 0 ? 'ring-2 ring-offset-2 ring-offset-background ring-rose-500' : ''
                }`}
                aria-label={color.value}
              />
            ))}
          </div>
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="description">Description <span className="text-muted-foreground">(optional)</span></Label>
          <textarea
            id="description"
            rows={3}
            placeholder="What is this project about?"
            className="w-full resize-none rounded-md border border-input bg-background px-3 py-2 text-sm text-foreground placeholder:text-muted-foreground outline-none focus:ring-2 focus:ring-ring"
          />
        </div>
      </div>
    </div>
  )
}
