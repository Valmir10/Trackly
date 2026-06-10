import { Plus, Webhook } from 'lucide-react'
import { Button } from '@/components/ui/button'

export default function WebhooksSettings() {
  return (
    <div className="flex flex-col gap-8 max-w-2xl">
      <div className="flex items-start justify-between">
        <div>
          <h2 className="text-base font-semibold text-foreground">Webhooks</h2>
          <p className="mt-1 text-sm text-muted-foreground">Send real-time events to your own endpoints when things happen in Trackly.</p>
        </div>
        <Button className="gap-2 bg-rose-500 hover:bg-rose-600 text-white shrink-0" size="sm">
          <Plus size={14} />
          Add webhook
        </Button>
      </div>

      <div className="flex flex-col items-center gap-3 py-16 text-center rounded-xl border border-dashed border-border/60">
        <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-muted">
          <Webhook size={20} className="text-muted-foreground" />
        </div>
        <div>
          <p className="text-sm font-medium text-foreground">No webhooks configured</p>
          <p className="mt-1 text-xs text-muted-foreground max-w-xs">
            Add a webhook endpoint to receive events like task created, task moved, and comment added.
          </p>
        </div>
        <Button variant="outline" size="sm" className="mt-1">
          <Plus size={13} className="mr-1.5" />
          Add your first webhook
        </Button>
      </div>
    </div>
  )
}
