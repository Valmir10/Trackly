import { useParams } from 'react-router-dom'
import AppShell from '@/components/AppShell'
import MeetingNotesComposer from '@/components/MeetingNotesComposer'
import { useMeeting } from '@/hooks/useMeeting'
import { useMeetingDecisions } from '@/hooks/useMeetingDecisions'
import { useProjectHub } from '@/hooks/useProjectHub'
import { useMeetingStore } from '@/store/useMeetingStore'
import { useDecisionStore } from '@/store/useDecisionStore'
import '@/styles/MeetingPage.css'

export default function MeetingPage() {
  const { meetingId } = useParams()
  useMeeting(meetingId)
  useMeetingDecisions(meetingId)

  const title = useMeetingStore((s) => s.title)
  const projectId = useMeetingStore((s) => s.projectId)
  useProjectHub(projectId ?? undefined)

  const decisions = useDecisionStore((s) => s.decisions)
  const meetingDecisions = Object.values(decisions).filter((d) => d.meetingId === meetingId)

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
            <MeetingNotesComposer meetingId={meetingId} projectId={projectId} />
          </div>

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
    </AppShell>
  )
}
