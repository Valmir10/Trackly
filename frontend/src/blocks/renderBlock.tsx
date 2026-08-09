import { Maximize2 } from 'lucide-react'
import InlineTicketCard from '@/components/InlineTicketCard'
import type { MessageBlock } from '@/blocks/types'

// The literal "same renderer, new container" contract: both ChatThread and
// the meeting notes composer's preview pane call this same function.
export interface RenderBlockContext {
  messageId: string
  onOpenTicket: (id: string) => void
  onPromoteToCard: (messageId: string, blockIndex: number) => void
  // Only meaningful for decisionRef — chat's parseMessage never produces
  // one, so ChatThread doesn't need to supply this.
  onOpenDecision?: (decisionId: string) => void
}

export function renderBlock(block: MessageBlock, key: number, ctx: RenderBlockContext) {
  switch (block.type) {
    case 'text':
      return <span key={key}>{block.text}</span>
    case 'mention':
      return (
        <span key={key} className="tp-chat__mention">
          @{block.label}
        </span>
      )
    case 'ticketRef':
      return (
        <span key={key} className="tp-chat__ticket-ref">
          <button type="button" className="tp-chat__ticket-chip" onClick={() => ctx.onOpenTicket(block.ticketId)}>
            #{block.ticketId}
          </button>
          <button
            type="button"
            className="tp-chat__promote"
            aria-label={`Show #${block.ticketId} as a live card`}
            onClick={() => ctx.onPromoteToCard(ctx.messageId, key)}
          >
            <Maximize2 size={11} />
          </button>
        </span>
      )
    case 'card':
      return <InlineTicketCard key={key} ticketId={block.ticketId} onOpenTicket={ctx.onOpenTicket} />
    case 'decisionRef':
      return (
        <button key={key} type="button" className="tp-chat__decision-chip" onClick={() => ctx.onOpenDecision?.(block.decisionId)}>
          &gt;{block.label}
        </button>
      )
    case 'actionItem':
      return <InlineTicketCard key={key} ticketId={block.ticketId} onOpenTicket={ctx.onOpenTicket} />
    case 'agendaItem':
      return (
        <span key={key} className="tp-chat__agenda-item">
          <InlineTicketCard ticketId={block.ticketId} onOpenTicket={ctx.onOpenTicket} />
          <span className="tp-chat__agenda-reason">{block.reason}</span>
        </span>
      )
  }
}
