import { useEffect, useRef, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { apiClient } from '@/lib/apiClient'
import { useAuthStore } from '@/store/useAuthStore'
import { useMeetingStore } from '@/store/useMeetingStore'
import { useDecisionStore } from '@/store/useDecisionStore'
import { useTaskStore } from '@/store/useTaskStore'
import { detectAuthoringTrigger } from '@/meetings/authoringTrigger'
import { parseNotes } from '@/meetings/parseNotes'
import { resolveUser, resolveTicket, resolveDecision } from '@/meetings/resolvers'
import { renderBlock } from '@/blocks/renderBlock'
import type { AuthoringTrigger } from '@/meetings/authoringTrigger'
import '@/styles/MeetingNotesComposer.css'

interface MeetingNotesComposerProps {
  meetingId: string
  projectId: string
}

const SAVE_DEBOUNCE_MS = 800

const HINT_TEXT: Record<AuthoringTrigger['marker'], string> = {
  '>': 'Press Enter to record this decision',
  '+': 'Press Enter to create this ticket',
}

export default function MeetingNotesComposer({ meetingId, projectId }: MeetingNotesComposerProps) {
  const { slug = '' } = useParams()
  const navigate = useNavigate()
  const storedNotes = useMeetingStore((s) => s.notes)
  const setNotesInStore = useMeetingStore((s) => s.setNotes)

  const [value, setValue] = useState(storedNotes)
  const [armed, setArmed] = useState<AuthoringTrigger | null>(null)
  const [submitting, setSubmitting] = useState(false)
  const textareaRef = useRef<HTMLTextAreaElement>(null)
  const saveTimeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null)
  const loadedMeetingIdRef = useRef<string | null>(null)

  // Notes arrive asynchronously (GetMeetingById) after this component
  // mounts — sync the fetched value into local editor state exactly once
  // per meeting, not on every store update (which would clobber in-flight
  // typing on every render).
  useEffect(() => {
    if (loadedMeetingIdRef.current !== meetingId) {
      loadedMeetingIdRef.current = meetingId
      setValue(storedNotes)
    }
  }, [meetingId, storedNotes])

  useEffect(() => {
    if (saveTimeoutRef.current) clearTimeout(saveTimeoutRef.current)
    saveTimeoutRef.current = setTimeout(() => {
      setNotesInStore(value)
      void apiClient.patch(`/api/meetings/${meetingId}/notes`, { notes: value })
    }, SAVE_DEBOUNCE_MS)

    return () => {
      if (saveTimeoutRef.current) clearTimeout(saveTimeoutRef.current)
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [value, meetingId])

  function handleChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
    const nextValue = e.target.value
    const caret = e.target.selectionStart ?? nextValue.length
    setValue(nextValue)
    setArmed(detectAuthoringTrigger(nextValue, caret, armed))
  }

  async function submitArmedBlock() {
    if (!armed || submitting) return
    const textarea = textareaRef.current
    const caret = textarea?.selectionStart ?? value.length
    const capturedText = value.slice(armed.start + 1, caret).trim()
    if (!capturedText) {
      setArmed(null)
      return
    }

    setSubmitting(true)
    try {
      let id: string
      if (armed.marker === '>') {
        const response = await apiClient.post<{ id: string }>(`/api/meetings/${meetingId}/decisions`, {
          text: capturedText,
        })
        id = response.data.id
        useDecisionStore.getState().applyRemoteDecision({
          id,
          meetingId,
          projectId,
          text: capturedText,
          createdById: useAuthStore.getState().userId ?? '',
          createdAt: new Date().toISOString(),
        })
      } else {
        const response = await apiClient.post<{ id: string }>(`/api/projects/${projectId}/tickets`, {
          title: capturedText,
          description: null,
          priority: 'Medium',
          assignedToId: null,
          dueDate: null,
          originMeetingId: meetingId,
        })
        id = response.data.id
        useTaskStore.getState().applyRemoteNewTicket({ id, title: capturedText })
      }

      const before = value.slice(0, armed.start)
      const after = value.slice(caret)
      const inserted = `${armed.marker}${id} `
      const nextValue = before + inserted + after
      const nextCaret = (before + inserted).length
      setValue(nextValue)
      setArmed(null)
      requestAnimationFrame(() => {
        textareaRef.current?.focus()
        textareaRef.current?.setSelectionRange(nextCaret, nextCaret)
      })
    } finally {
      setSubmitting(false)
    }
  }

  function handleKeyDown(e: React.KeyboardEvent<HTMLTextAreaElement>) {
    if (!armed) return

    if (e.key === 'Enter') {
      e.preventDefault()
      void submitArmedBlock()
      return
    }

    if (e.key === 'Escape') {
      e.preventDefault()
      setArmed(null)
    }
  }

  const blocks = parseNotes(value, { resolveUser, resolveTicket, resolveDecision })

  return (
    <div className="tp-meeting-notes">
      <div className="tp-meeting-notes__editor">
        <textarea
          ref={textareaRef}
          className="tp-input tp-meeting-notes__textarea"
          placeholder="Type meeting notes... > for a decision, + for an action item"
          value={value}
          onChange={handleChange}
          onKeyDown={handleKeyDown}
          onClick={(e) => setArmed(detectAuthoringTrigger(value, e.currentTarget.selectionStart ?? value.length, armed))}
          disabled={submitting}
        />
        {armed && <div className="tp-meeting-notes__hint">{HINT_TEXT[armed.marker]}</div>}
      </div>

      <div className="tp-meeting-notes__preview">
        <span className="tp-label">Preview</span>
        <div className="tp-meeting-notes__preview-body">
          {blocks.map((block, i) =>
            renderBlock(block, i, {
              messageId: meetingId,
              onOpenTicket: (ticketId) => navigate(`/${slug}/projects/${projectId}?ticket=${ticketId}`),
              // Promoting a bare #ref chip to a live card isn't wired for
              // meeting notes yet — actionItem/decisionRef blocks already
              // render as live cards from the moment they're created.
              onPromoteToCard: () => {},
            })
          )}
        </div>
      </div>
    </div>
  )
}
