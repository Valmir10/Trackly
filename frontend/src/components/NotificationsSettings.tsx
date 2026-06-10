import { Separator } from '@/components/ui/separator'

const groups = [
  {
    title: 'Task activity',
    items: [
      { label: 'Task assigned to me', email: true, inApp: true },
      { label: 'Task status changed', email: false, inApp: true },
      { label: 'Task commented on', email: true, inApp: true },
      { label: 'Task due date approaching', email: true, inApp: true },
    ],
  },
  {
    title: 'Digests',
    items: [
      { label: 'Daily digest', email: true, inApp: false },
      { label: 'Weekly AI summary', email: true, inApp: false },
    ],
  },
  {
    title: 'Workspace',
    items: [
      { label: 'New member joined', email: false, inApp: true },
      { label: 'Project created or archived', email: false, inApp: true },
    ],
  },
]

function Toggle({ on }: { on: boolean }) {
  return (
    <div className={`relative h-5 w-9 rounded-full transition-colors ${on ? 'bg-rose-500' : 'bg-muted'}`}>
      <div className={`absolute top-0.5 h-4 w-4 rounded-full bg-white shadow transition-transform ${on ? 'translate-x-4' : 'translate-x-0.5'}`} />
    </div>
  )
}

export default function NotificationsSettings() {
  return (
    <div className="flex flex-col gap-8 max-w-xl">
      <div>
        <h2 className="text-base font-semibold text-foreground">Notifications</h2>
        <p className="mt-1 text-sm text-muted-foreground">Choose how and when you want to be notified.</p>
      </div>

      <div className="flex flex-col gap-6">
        <div className="grid grid-cols-3 gap-2 text-xs font-semibold uppercase tracking-wider text-muted-foreground">
          <span className="col-span-1"></span>
          <span className="text-center">Email</span>
          <span className="text-center">In-app</span>
        </div>

        {groups.map((group, gi) => (
          <div key={group.title}>
            {gi > 0 && <Separator className="mb-6" />}
            <p className="mb-3 text-xs font-semibold text-foreground">{group.title}</p>
            <div className="flex flex-col gap-3">
              {group.items.map((item) => (
                <div key={item.label} className="grid grid-cols-3 items-center gap-2">
                  <span className="col-span-1 text-sm text-foreground">{item.label}</span>
                  <div className="flex justify-center"><Toggle on={item.email} /></div>
                  <div className="flex justify-center"><Toggle on={item.inApp} /></div>
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}
