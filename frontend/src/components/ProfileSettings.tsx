import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Separator } from '@/components/ui/separator'
import { Avatar, AvatarFallback } from '@/components/ui/avatar'

export default function ProfileSettings() {
  return (
    <div className="flex flex-col gap-8 max-w-xl">
      <div>
        <h2 className="text-base font-semibold text-foreground">Profile</h2>
        <p className="mt-1 text-sm text-muted-foreground">Update your personal information and preferences.</p>
      </div>

      <div className="flex items-center gap-4">
        <Avatar className="h-16 w-16">
          <AvatarFallback className="bg-rose-500/15 text-xl font-semibold text-rose-400">VZ</AvatarFallback>
        </Avatar>
        <div>
          <Button variant="outline" size="sm">Change avatar</Button>
          <p className="mt-1 text-xs text-muted-foreground">PNG, JPG up to 2MB</p>
        </div>
      </div>

      <div className="flex flex-col gap-4">
        <div className="grid grid-cols-2 gap-3">
          <div className="flex flex-col gap-1.5">
            <Label>First name</Label>
            <Input defaultValue="Valmir" />
          </div>
          <div className="flex flex-col gap-1.5">
            <Label>Last name</Label>
            <Input defaultValue="Zogaj" />
          </div>
        </div>
        <div className="flex flex-col gap-1.5">
          <Label>Email</Label>
          <Input type="email" defaultValue="valmir@acme.com" />
        </div>
        <div className="flex flex-col gap-1.5">
          <Label>Job title <span className="text-muted-foreground">(optional)</span></Label>
          <Input placeholder="e.g. Frontend Developer" />
        </div>
        <Button className="w-fit bg-rose-500 hover:bg-rose-600 text-white">Save changes</Button>
      </div>

      <Separator />

      <div className="flex flex-col gap-4">
        <h3 className="text-sm font-semibold text-foreground">Change password</h3>
        <div className="flex flex-col gap-3">
          <div className="flex flex-col gap-1.5">
            <Label>Current password</Label>
            <Input type="password" placeholder="••••••••••" />
          </div>
          <div className="flex flex-col gap-1.5">
            <Label>New password</Label>
            <Input type="password" placeholder="Min. 10 characters" />
          </div>
        </div>
        <Button variant="outline" className="w-fit">Update password</Button>
      </div>
    </div>
  )
}
