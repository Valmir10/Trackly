import { forwardRef, useEffect, useImperativeHandle, useRef, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { apiClient } from '@/lib/apiClient'
import { useAuthStore } from '@/store/useAuthStore'
import { useMeetingStore } from '@/store/useMeetingStore'
import { useDecisionStore } from '@/store/useDecisionStore'
import { useTaskStore } from '@/store/useTaskStore'
import { WORKSPACE_MEMBERS } from '@/data/users'
import { detectMentionTrigger } from '@/chat/mentionTrigger'
import { detectAuthoringTrigger } from '@/meetings/authoringTrigger'
import { parseNotes } from '@/meetings/parseNotes'
import { resolveUser, resolveTicket, resolveDecision } from '@/meetings/resolvers'
import { renderBlock } from '@/blocks/renderBlock'
import type { MentionTrigger } from '@/chat/mentionTrigger'
import type { AuthoringTrigger } from '@/meetings/authoringTrigger'
import '@/styles/MeetingNotesComposer.css'

interface MeetingNotesComposerProps {
  meetingId: string
  projectId: string
}

export interface MeetingNotesComposerHandle {
  appendLine: (line: string) => void
}

interface PickerItem {
  id: string
  label: string
  insertText: string
  mono?: string
}

const SAVE_DEBOUNCE_MS = 800

const HINT_TEXT: Record<AuthoringTrigger['marker'], string> = {
  '>': 'Press Enter to record this decision',
  '+': 'Press Enter to create this ticket',
}

function matchMembers(query: string): PickerItem[] {
  const q = query.toLowerCase()
  return WORKSPACE_MEMBERS.filter((m) => m.handle.toLowerCase().startsWith(q) || m.name.toLowerCase().includes(q))
    .slice(0, 6)
    .map((m) => ({ id: m.id, label: m.name, insertText: m.handle }))
}

// # surfaces both tickets and decisions — same trigger, two entity types,
// matching parseNotes' own #-resolves-ticket-then-decision dispatch.
function matchTicketsAndDecisions(query: string): PickerItem[] {
  const q = query.toLowerCase()
  const tickets = useTaskStore
    .getState()
    .columns.flatMap((c) => c.tasks)
    .filter((t) => t.id.startsWith(query) || t.title.toLowerCase().includes(q))
    .map((t) => ({ id: t.id, label: t.title, insertText: t.id, mono: `#${t.id}` }))
  const decisions = Object.values(useDecisionStore.getState().decisions)
    .filter((d) => d.id.startsWith(query) || d.text.toLowerCase().includes(q))
    .map((d) => ({ id: d.id, label: d.text, insertText: d.id, mono: `#${d.id}` }))
  return [...tickets, ...decisions].slice(0, 6)
}

const MeetingNotesComposer = forwardRef<MeetingNotesComposerHandle, MeetingNotesComposerProps>(function MeetingNotesComposer(
  { meetingId, projectId },
  ref
) {
  const { slug = '' } = useParams()
  const navigate = useNavigate()
  const storedNotes = useMeetingStore((s) => s.notes)
  const setNotesInStore = useMeetingStore((s) => s.setNotes)

  const [value, setValue] = useState(storedNotes)
  const [armed, setArmed] = useState<AuthoringTrigger | null>(null)
  const [trigger, setTrigger] = useState<MentionTrigger | null>(null)
  const [pickerIndex, setPickerIndex] = useState(0)
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

  // Lets SuggestedAgendaPanel (a sibling, not a parent) append an
  // agendaItem line without lifting all of this component's composer state
  // up to MeetingPage.
  useImperativeHandle(ref, () => ({
    appendLine: (line: string) => {
      setValue((prev) => (prev.length > 0 && !prev.endsWith('\n') ? `${prev}\n${line}\n` : `${prev}${line}\n`))
    },
  }))

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

  const pickerItems: PickerItem[] = trigger
    ? trigger.marker === '@'
      ? matchMembers(trigger.query)
      : matchTicketsAndDecisions(trigger.query)
    : []

  function handleChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
    const nextValue = e.target.value
    const caret = e.target.selectionStart ?? nextValue.length
    setValue(nextValue)
    setArmed(detectAuthoringTrigger(nextValue, caret, armed))
    setTrigger(detectMentionTrigger(nextValue, caret))
    setPickerIndex(0)
  }

  function applyPickerItem(item: PickerItem) {
    if (!trigger) return
    const before = value.slice(0, trigger.start)
    const after = value.slice(trigger.start + trigger.marker.length + trigger.query.length)
    const inserted = `${trigger.marker}${item.insertText} `
    const nextValue = before + inserted + after
    const caret = (before + inserted).length
    setValue(nextValue)
    setTrigger(null)
    requestAnimationFrame(() => {
      textareaRef.current?.focus()
      textareaRef.current?.setSelectionRange(caret, caret)
    })
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
    if (trigger && pickerItems.length > 0) {
      if (e.key === 'ArrowDown') {
        e.preventDefault()
        setPickerIndex((i) => Math.min(i + 1, pickerItems.length - 1))
        return
      }
      if (e.key === 'ArrowUp') {
        e.preventDefault()
        setPickerIndex((i) => Math.max(i - 1, 0))
        return
      }
      const alreadyResolved =
        trigger.marker === '@' ? resolveUser(trigger.query) : (resolveTicket(trigger.query) ?? resolveDecision(trigger.query))
      if ((e.key === 'Enter' || e.key === 'Tab') && !alreadyResolved) {
        e.preventDefault()
        applyPickerItem(pickerItems[pickerIndex])
        return
      }
      if (e.key === 'Escape') {
        e.preventDefault()
        setTrigger(null)
        return
      }
    }

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
          placeholder="Type meeting notes... @ to mention, # for a ticket or decision, > for a new decision, + for a new ticket"
          value={value}
          onChange={handleChange}
          onKeyDown={handleKeyDown}
          onClick={(e) => setArmed(detectAuthoringTrigger(value, e.currentTarget.selectionStart ?? value.length, armed))}
          disabled={submitting}
        />
        {armed && <div className="tp-meeting-notes__hint">{HINT_TEXT[armed.marker]}</div>}

        {trigger && pickerItems.length > 0 && (
          <div className="tp-meeting-notes__picker">
            {pickerItems.map((item, i) => (
              <button
                key={item.id}
                type="button"
                className={`tp-meeting-notes__picker-item${i === pickerIndex ? ' tp-meeting-notes__picker-item--active' : ''}`}
                onMouseEnter={() => setPickerIndex(i)}
                onClick={() => applyPickerItem(item)}
              >
                <span className="tp-meeting-notes__picker-label">{item.label}</span>
                {item.mono && <span className="tp-meeting-notes__picker-mono">{item.mono}</span>}
              </button>
            ))}
          </div>
        )}
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
})

export default MeetingNotesComposer
