import { Building2 } from 'lucide-react'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'

export default function WorkspaceStep() {
  return (
    <div className="flex flex-col gap-5">
      <div className="flex items-center gap-3">
        <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-violet-500/10">
          <Building2 size={20} className="text-violet-400" />
        </div>
        <div>
          <h2 className="text-base font-semibold text-foreground">Set up your workspace</h2>
          <p className="text-sm text-muted-foreground">This is your company or team name in Trackly.</p>
        </div>
      </div>

      <div className="flex flex-col gap-4">
        <div className="flex flex-col gap-1.5">
          <Label htmlFor="company">Company name</Label>
          <Input id="company" placeholder="Acme Corp" />
        </div>

        <div className="flex flex-col gap-1.5">
          <Label htmlFor="slug">Workspace URL</Label>
          <div className="flex items-center rounded-md border border-input overflow-hidden focus-within:ring-2 focus-within:ring-ring">
            <span className="select-none bg-muted px-3 py-2 text-sm text-muted-foreground border-r border-input">
              trackly.app/
            </span>
            <input
              id="slug"
              className="flex-1 bg-transparent px-3 py-2 text-sm text-foreground placeholder:text-muted-foreground outline-none"
              placeholder="acme-corp"
            />
          </div>
          <p className="text-xs text-muted-foreground">
            Only lowercase letters, numbers and hyphens. Cannot be changed later.
          </p>
        </div>
      </div>
    </div>
  )
}
