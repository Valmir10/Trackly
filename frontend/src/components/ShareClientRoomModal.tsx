import { useState } from 'react'
import { X, Copy, Check } from 'lucide-react'
import { useClientRoomAccessList } from '@/hooks/useClientRoomAccessList'
import { useCreateClientRoomAccess } from '@/hooks/useCreateClientRoomAccess'
import { useRevokeClientRoomAccess } from '@/hooks/useRevokeClientRoomAccess'
import '@/styles/ShareClientRoomModal.css'

interface ShareClientRoomModalProps {
  projectId: string
  onClose: () => void
}

export default function ShareClientRoomModal({ projectId, onClose }: ShareClientRoomModalProps) {
  const { data: grants = [] } = useClientRoomAccessList(projectId)
  const createAccess = useCreateClientRoomAccess(projectId)
  const revokeAccess = useRevokeClientRoomAccess(projectId)
  const [copied, setCopied] = useState(false)

  const rawLink = createAccess.data ? `${window.location.origin}/client-room/${createAccess.data.rawToken}` : null

  function copyLink() {
    if (!rawLink) return
    void navigator.clipboard.writeText(rawLink)
    setCopied(true)
    setTimeout(() => setCopied(false), 2000)
  }

  const activeGrants = grants.filter((g) => g.isActive)

  return (
    <div className="tp-modal-overlay" onClick={onClose}>
      <div className="tp-modal tp-share-client-room-modal" onClick={(e) => e.stopPropagation()}>
        <div className="tp-modal__header">
          <h2 className="tp-modal__title">Share with client</h2>
          <button type="button" className="tp-modal__close" onClick={onClose} aria-label="Close">
            <X size={16} />
          </button>
        </div>

        <div className="tp-modal__body tp-share-client-room-modal__body">
          {rawLink ? (
            <div className="tp-field">
              <label className="tp-label">Link (shown once — copy it now)</label>
              <div className="tp-share-client-room-modal__link-row">
                <input type="text" className="tp-input" readOnly value={rawLink} onFocus={(e) => e.target.select()} />
                <button type="button" className="tp-btn tp-btn--secondary tp-btn--sm" onClick={copyLink}>
                  {copied ? <Check size={13} /> : <Copy size={13} />}
                  {copied ? 'Copied' : 'Copy'}
                </button>
              </div>
            </div>
          ) : (
            <button
              type="button"
              className="tp-btn tp-btn--primary"
              disabled={createAccess.isPending}
              onClick={() => createAccess.mutate()}
            >
              {createAccess.isPending ? 'Creating…' : 'Create client link'}
            </button>
          )}

          {activeGrants.length > 0 && (
            <div className="tp-field">
              <label className="tp-label">Active links</label>
              <ul className="tp-share-client-room-modal__grants">
                {activeGrants.map((grant) => (
                  <li key={grant.id} className="tp-share-client-room-modal__grant">
                    <span>Created {new Date(grant.createdAt).toLocaleDateString()}</span>
                    <button
                      type="button"
                      className="tp-btn tp-btn--ghost tp-btn--sm"
                      disabled={revokeAccess.isPending}
                      onClick={() => revokeAccess.mutate(grant.id)}
                    >
                      Revoke
                    </button>
                  </li>
                ))}
              </ul>
            </div>
          )}
        </div>
      </div>
    </div>
  )
}
