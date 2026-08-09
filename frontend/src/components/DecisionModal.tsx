import { X } from 'lucide-react'
import type { DecisionDto } from '@/api/types'
import '@/styles/DecisionModal.css'

interface DecisionModalProps {
  decision: DecisionDto
  onClose: () => void
}

// Read-only — mirrors TicketModal's `?ticket=<id>` deep-link pattern for
// `?decision=<id>`. Decisions are immutable once recorded, so there's
// nothing to edit here, just the provenance-origin-point record itself.
export default function DecisionModal({ decision, onClose }: DecisionModalProps) {
  return (
    <div className="tp-modal-overlay" onClick={onClose}>
      <div className="tp-modal tp-decision-modal" onClick={(e) => e.stopPropagation()}>
        <div className="tp-modal__header">
          <div>
            <span className="tp-decision-modal__id">&gt;{decision.id}</span>
            <h2 className="tp-modal__title">Decision</h2>
          </div>
          <button type="button" className="tp-modal__close" onClick={onClose} aria-label="Close">
            <X size={16} />
          </button>
        </div>

        <div className="tp-modal__body">
          <p className="tp-decision-modal__text">{decision.text}</p>
          <span className="tp-decision-modal__meta">Recorded {new Date(decision.createdAt).toLocaleString()}</span>
        </div>
      </div>
    </div>
  )
}
