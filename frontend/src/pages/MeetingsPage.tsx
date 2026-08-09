import { useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { CalendarDays, Plus } from 'lucide-react'
import AppShell from '@/components/AppShell'
import ScopeSwitcher from '@/components/ScopeSwitcher'
import NewMeetingModal from '@/components/NewMeetingModal'
import { useProjects } from '@/hooks/useProjects'
import { useWorkspaceMeetings } from '@/hooks/useWorkspaceMeetings'
import { useWorkspaceScope } from '@/store/useWorkspaceScope'
import { filterMeetingsByScope } from '@/utils/scope'
import '@/styles/MeetingsPage.css'

export default function MeetingsPage() {
  const { slug = '' } = useParams()
  const navigate = useNavigate()
  const { data: allMeetings = [] } = useWorkspaceMeetings()
  const { data: projects = [] } = useProjects()
  const scope = useWorkspaceScope((s) => s.scope)
  const [showModal, setShowModal] = useState(false)

  const meetings = filterMeetingsByScope(allMeetings, scope)
    .slice()
    .sort((a, b) => new Date(a.scheduledAt).getTime() - new Date(b.scheduledAt).getTime())

  const projectById = new Map(projects.map((p) => [p.id, p]))

  return (
    <AppShell>
      <div className="tp-meetings-page">
        <div className="tp-page-header tp-page-header--row">
          <div>
            <h1 className="tp-page-header__title">Meetings</h1>
            <p className="tp-page-header__subtitle">Agenda, decisions, and action items — captured where they happen.</p>
          </div>
          <div className="tp-meetings-page__actions">
            <ScopeSwitcher />
            <button type="button" className="tp-btn tp-btn--primary tp-btn--sm" onClick={() => setShowModal(true)}>
              <Plus size={14} />
              New meeting
            </button>
          </div>
        </div>

        {meetings.length === 0 ? (
          <div className="tp-meetings-page__empty">
            <CalendarDays size={24} />
            <p>No meetings yet</p>
            <span>Create one to start capturing decisions and action items.</span>
          </div>
        ) : (
          <div className="tp-meetings-page__list">
            {meetings.map((meeting) => {
              const project = projectById.get(meeting.projectId)
              return (
                <button
                  key={meeting.id}
                  type="button"
                  className="tp-meeting-row"
                  onClick={() => navigate(`/${slug}/meetings/${meeting.id}`)}
                >
                  <span className="tp-meeting-row__title">{meeting.title}</span>
                  <span className="tp-meeting-row__meta">
                    {project && (
                      <span className="tp-meeting-row__project">
                        <span className="tp-meeting-row__project-dot" style={{ background: project.color }} />
                        {project.name}
                      </span>
                    )}
                    <span className="tp-meeting-row__date">
                      {new Date(meeting.scheduledAt).toLocaleDateString('en-US', {
                        month: 'short',
                        day: 'numeric',
                        hour: 'numeric',
                        minute: '2-digit',
                      })}
                    </span>
                  </span>
                </button>
              )
            })}
          </div>
        )}
      </div>

      {showModal && (
        <NewMeetingModal
          defaultProjectId={scope.type === 'project' ? scope.projectId : undefined}
          onClose={() => setShowModal(false)}
          onCreated={(meetingId) => navigate(`/${slug}/meetings/${meetingId}`)}
        />
      )}
    </AppShell>
  )
}
