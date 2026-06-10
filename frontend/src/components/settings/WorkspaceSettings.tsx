import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Separator } from '@/components/ui/separator'

export default function WorkspaceSettings() {
  return (
    <div className="flex flex-col gap-8 max-w-xl">
      <div>
        <h2 className="text-base font-semibold text-foreground">Workspace</h2>
        <p className="mt-1 text-sm text-muted-foreground">Manage your workspace settings and details.</p>
      </div>

      <div className="flex flex-col gap-5">
        <div className="flex items-center gap-4">
          <div className="flex h-14 w-14 items-center justify-center rounded-xl bg-violet-600 text-xl font-bold text-white">
            A
          </div>
          <div>
            <Button variant="outline" size="sm">Change logo</Button>
            <p className="mt-1 text-xs text-muted-foreground">PNG, JPG up to 2MB</p>
          </div>
        </div>

        <div className="flex flex-col gap-1.5">
          <Label>Workspace name</Label>
          <Input defaultValue="Acme Corp" className="max-w-sm" />
        </div>

        <div className="flex flex-col gap-1.5">
          <Label>Workspace URL</Label>
          <div className="flex items-center rounded-md border border-input overflow-hidden max-w-sm">
            <span className="select-none bg-muted px-3 py-2 text-sm text-muted-foreground border-r border-input">
              trackly.app/
            </span>
            <input defaultValue="acme-corp" className="flex-1 bg-transparent px-3 py-2 text-sm text-foreground outline-none" />
          </div>
          <p className="text-xs text-muted-foreground">Cannot be changed after creation.</p>
        </div>

        <Button className="w-fit bg-violet-600 hover:bg-violet-700 text-white">Save changes</Button>
      </div>

      <Separator />

      <div className="flex flex-col gap-4">
        <div>
          <h3 className="text-sm font-semibold text-foreground">Danger zone</h3>
          <p className="mt-1 text-sm text-muted-foreground">These actions are permanent and cannot be undone.</p>
        </div>
        <Button variant="outline" className="w-fit border-red-500/30 text-red-400 hover:bg-red-500/10 hover:text-red-400">
          Delete workspace
        </Button>
      </div>
    </div>
  )
}
