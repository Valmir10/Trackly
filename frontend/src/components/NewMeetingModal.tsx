import { useState } from 'react'
import { X } from 'lucide-react'
import { apiClient } from '@/lib/apiClient'
import { useProjects } from '@/hooks/useProjects'
import '@/styles/NewMeetingModal.css'

interface NewMeetingModalProps {
  defaultProjectId?: string
  onClose: () => void
  onCreated: (meetingId: string) => void
}

export default function NewMeetingModal({ defaultProjectId, onClose, onCreated }: NewMeetingModalProps) {
  const { data: projects = [] } = useProjects()
  const [title, setTitle] = useState('')
  const [projectId, setProjectId] = useState(defaultProjectId ?? '')
  const [scheduledAt, setScheduledAt] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const resolvedProjectId = projectId || projects[0]?.id || ''

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!title.trim() || !scheduledAt || !resolvedProjectId) return

    setSubmitting(true)
    setError(null)
    try {
      const response = await apiClient.post<{ id: string }>(`/api/projects/${resolvedProjectId}/meetings`, {
        title: title.trim(),
        scheduledAt: new Date(scheduledAt).toISOString(),
      })
      onCreated(response.data.id)
    } catch {
      setError('Could not create the meeting. Try again.')
      setSubmitting(false)
    }
  }

  return (
    <div className="tp-modal-overlay" onClick={onClose}>
      <div className="tp-modal tp-new-meeting-modal" onClick={(e) => e.stopPropagation()}>
        <div className="tp-modal__header">
          <h2 className="tp-modal__title">New meeting</h2>
          <button type="button" className="tp-modal__close" onClick={onClose} aria-label="Close">
            <X size={16} />
          </button>
        </div>

        <form className="tp-modal__body tp-new-meeting-modal__form" onSubmit={handleSubmit}>
          <div className="tp-field">
            <label className="tp-label" htmlFor="meeting-title">
              Title
            </label>
            <input
              id="meeting-title"
              type="text"
              className="tp-input"
              placeholder="e.g. Sprint Planning 14"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              autoFocus
            />
          </div>

          <div className="tp-field">
            <label className="tp-label" htmlFor="meeting-project">
              Project
            </label>
            <select
              id="meeting-project"
              className="tp-input"
              value={resolvedProjectId}
              onChange={(e) => setProjectId(e.target.value)}
            >
              {projects.map((project) => (
                <option key={project.id} value={project.id}>
                  {project.name}
                </option>
              ))}
            </select>
          </div>

          <div className="tp-field">
            <label className="tp-label" htmlFor="meeting-scheduled-at">
              When
            </label>
            <input
              id="meeting-scheduled-at"
              type="datetime-local"
              className="tp-input"
              value={scheduledAt}
              onChange={(e) => setScheduledAt(e.target.value)}
            />
          </div>

          {error && <p className="tp-field__error">{error}</p>}

          <div className="tp-new-meeting-modal__actions">
            <button type="button" className="tp-btn tp-btn--secondary" onClick={onClose}>
              Cancel
            </button>
            <button
              type="submit"
              className="tp-btn tp-btn--primary"
              disabled={submitting || !title.trim() || !scheduledAt || !resolvedProjectId}
            >
              {submitting ? 'Creating…' : 'Create meeting'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
