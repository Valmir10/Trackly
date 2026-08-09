import { useRef } from 'react'
import { useParams, useSearchParams } from 'react-router-dom'
import AppShell from '@/components/AppShell'
import MeetingNotesComposer from '@/components/MeetingNotesComposer'
import SuggestedAgendaPanel from '@/components/SuggestedAgendaPanel'
import DecisionModal from '@/components/DecisionModal'
import { useMeeting } from '@/hooks/useMeeting'
import { useMeetingDecisions } from '@/hooks/useMeetingDecisions'
import { useProjectTickets } from '@/hooks/useProjectTickets'
import { useProjectHub } from '@/hooks/useProjectHub'
import { useMeetingStore } from '@/store/useMeetingStore'
import { useDecisionStore } from '@/store/useDecisionStore'
import type { MeetingNotesComposerHandle } from '@/components/MeetingNotesComposer'
import '@/styles/MeetingPage.css'

export default function MeetingPage() {
  const { meetingId } = useParams()
  const [searchParams, setSearchParams] = useSearchParams()
  const composerRef = useRef<MeetingNotesComposerHandle>(null)

  useMeeting(meetingId)
  useMeetingDecisions(meetingId)

  const title = useMeetingStore((s) => s.title)
  const projectId = useMeetingStore((s) => s.projectId)
  // actionItem/agendaItem blocks render as InlineTicketCard, which reads
  // from useTaskStore — without this, that store is empty on a fresh page
  // load unless the user happened to visit this project's board first.
  useProjectTickets(projectId ?? undefined)
  useProjectHub(projectId ?? undefined)

  const decisions = useDecisionStore((s) => s.decisions)
  const meetingDecisions = Object.values(decisions).filter((d) => d.meetingId === meetingId)

  const activeDecisionId = searchParams.get('decision')
  const activeDecision = activeDecisionId ? decisions[activeDecisionId] : undefined

  function closeDecision() {
    setSearchParams((prev) => {
      const next = new URLSearchParams(prev)
      next.delete('decision')
      return next
    })
  }

  if (!meetingId || !projectId) {
    return (
      <AppShell>
        <div className="tp-meeting-page" />
      </AppShell>
    )
  }

  return (
    <AppShell>
      <div className="tp-meeting-page">
        <div className="tp-page-header">
          <h1 className="tp-page-header__title">{title || 'Meeting'}</h1>
        </div>

        <div className="tp-meeting-page__body">
          <div className="tp-meeting-page__notes">
            <MeetingNotesComposer ref={composerRef} meetingId={meetingId} projectId={projectId} />
          </div>

          <div className="tp-meeting-page__sidebar">
            <SuggestedAgendaPanel
              projectId={projectId}
              onAddToNotes={(ticketId, reason) => composerRef.current?.appendLine(`~${ticketId}|${reason}`)}
            />

            <div className="tp-meeting-page__decisions">
              <span className="tp-label">Decisions</span>
              {meetingDecisions.length === 0 ? (
                <p className="tp-meeting-page__decisions-empty">No decisions recorded yet.</p>
              ) : (
                <ul className="tp-meeting-page__decisions-list">
                  {meetingDecisions.map((decision) => (
                    <li key={decision.id} className="tp-meeting-page__decision">
                      {decision.text}
                    </li>
                  ))}
                </ul>
              )}
            </div>
          </div>
        </div>
      </div>

      {activeDecision && <DecisionModal decision={activeDecision} onClose={closeDecision} />}
    </AppShell>
  )
}
