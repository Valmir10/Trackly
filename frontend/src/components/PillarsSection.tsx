import { ListChecks, CalendarClock, Users, FileSignature, Check } from 'lucide-react'
import '@/styles/PillarsSection.css'

const taskItems = [
  { label: 'Design review', state: 'done' as const },
  { label: 'API integration', state: 'active' as const },
  { label: 'QA pass', state: 'todo' as const },
]

const agendaItems = ['Renewal terms', 'Onboarding blockers']

export default function PillarsSection() {
  return (
    <section id="pillars" className="tp-pillars">
      <div className="tp-container">
        <div className="tp-pillars__intro">
          <h2 className="tp-pillars__heading">Everything your team needs, working as one.</h2>
          <p className="tp-pillars__subheading">
            Tasks, meetings, clients, and contracts all work together from the start,
            each one aware of the others.
          </p>
        </div>

        <div className="tp-pillars__grid">
          <article className="tp-pillar tp-pillar--tasks">
            <div className="tp-pillar__icon">
              <ListChecks size={18} />
            </div>
            <h3 className="tp-pillar__title">Tasks that move fast</h3>
            <p className="tp-pillar__description">
              Kanban boards, sprints, and dependencies, plus automations that notify clients
              the moment a contract milestone ships.
            </p>
            <ul className="tp-pillar__tasklist">
              {taskItems.map((item) => (
                <li key={item.label} className={`tp-pillar__taskitem tp-pillar__taskitem--${item.state}`}>
                  {item.state === 'done' ? <Check size={13} /> : <span className="tp-pillar__taskdot" />}
                  {item.label}
                </li>
              ))}
            </ul>
          </article>

          <article className="tp-pillar tp-pillar--meetings">
            <div className="tp-pillar__icon">
              <CalendarClock size={18} />
            </div>
            <h3 className="tp-pillar__title">Meetings with a memory</h3>
            <p className="tp-pillar__description">
              Co-author the agenda beforehand, take shared notes live, and turn action items
              into tasks in one click.
            </p>
            <div className="tp-pillar__agenda">
              <span className="tp-pillar__agenda-when">Thu 10:00 · Acme kickoff</span>
              <ul className="tp-pillar__agenda-list">
                {agendaItems.map((item) => (
                  <li key={item}>{item}</li>
                ))}
              </ul>
            </div>
          </article>

          <article className="tp-pillar tp-pillar--clients">
            <div className="tp-pillar__icon">
              <Users size={18} />
            </div>
            <h3 className="tp-pillar__title">Clients see progress, not noise</h3>
            <p className="tp-pillar__description">
              A scoped Client Room shows real status and lets clients approve milestones,
              without exposing internal chatter.
            </p>
          </article>

          <article className="tp-pillar tp-pillar--contracts">
            <div className="tp-pillar__icon">
              <FileSignature size={18} />
            </div>
            <h3 className="tp-pillar__title">Contracts that track themselves</h3>
            <p className="tp-pillar__description">
              Milestones link straight to tasks, so delivery against the SOW is visible
              before renewal, not after.
            </p>
          </article>

          <article className="tp-pillar tp-pillar--connect">
            <div className="tp-pillar__connect-diagram" aria-hidden="true">
              <span className="tp-pillar__connect-node tp-pillar__connect-node--1" />
              <span className="tp-pillar__connect-node tp-pillar__connect-node--2" />
              <span className="tp-pillar__connect-node tp-pillar__connect-node--3" />
              <span className="tp-pillar__connect-node tp-pillar__connect-node--4" />
              <span className="tp-pillar__connect-center" />
            </div>
            <h3 className="tp-pillar__title">Everything connects</h3>
            <p className="tp-pillar__description">
              Every pillar links to the others. A task shows its meeting, a meeting shows
              its tasks, and a contract shows its progress.
            </p>
          </article>
        </div>
      </div>
    </section>
  )
}
