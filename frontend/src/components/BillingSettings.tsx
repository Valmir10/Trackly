import { Check, Zap } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'

const features = { free: ['3 users', '1 project', 'Kanban board'], pro: ['20 users', 'Unlimited projects', 'AI assistant', 'Time tracking', 'Analytics', 'Public API', 'Webhooks'] }

const invoices = [
  { date: 'Jun 1, 2025', amount: '$75.00', status: 'Paid' },
  { date: 'May 1, 2025', amount: '$75.00', status: 'Paid' },
  { date: 'Apr 1, 2025', amount: '$75.00', status: 'Paid' },
]

export default function BillingSettings() {
  return (
    <div className="flex flex-col gap-8 max-w-2xl">
      <div>
        <h2 className="text-base font-semibold text-foreground">Billing</h2>
        <p className="mt-1 text-sm text-muted-foreground">Manage your subscription and payment details.</p>
      </div>

      <div className="rounded-xl border border-rose-500/20 bg-rose-500/5 p-5">
        <div className="flex items-start justify-between">
          <div>
            <div className="flex items-center gap-2">
              <Zap size={15} className="text-rose-400" />
              <span className="text-sm font-semibold text-foreground">Pro plan</span>
              <Badge className="bg-rose-500/15 text-rose-400 hover:bg-rose-500/15 text-[10px]">Active</Badge>
            </div>
            <p className="mt-1 text-xs text-muted-foreground">$15 per user / month · 5 users · Next billing Jun 1, 2026</p>
          </div>
          <Button variant="outline" size="sm">Manage</Button>
        </div>
        <Separator className="my-4" />
        <div className="grid grid-cols-2 gap-1.5">
          {features.pro.map(f => (
            <div key={f} className="flex items-center gap-1.5 text-xs text-muted-foreground">
              <Check size={11} className="text-rose-400 shrink-0" />
              {f}
            </div>
          ))}
        </div>
      </div>

      <div>
        <h3 className="mb-3 text-sm font-semibold text-foreground">Invoice history</h3>
        <div className="flex flex-col divide-y divide-border/40 rounded-xl border border-border/60 overflow-hidden">
          {invoices.map((inv) => (
            <div key={inv.date} className="flex items-center justify-between bg-card px-4 py-3">
              <div>
                <p className="text-sm text-foreground">{inv.date}</p>
                <p className="text-xs text-muted-foreground">{inv.amount} · Pro plan</p>
              </div>
              <div className="flex items-center gap-3">
                <Badge variant="outline" className="border-green-500/30 text-green-400 text-[10px]">{inv.status}</Badge>
                <Button variant="ghost" size="sm" className="text-xs">Download</Button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
