import { Link } from 'react-router-dom'
import { ArrowRight, Calendar, FileText } from 'lucide-react'
import '@/styles/HeroSection.css'

type Task = {
  title: string
  tag: string
  assignee: string
  priority: 'low' | 'medium' | 'high'
  meeting?: string
  contract?: string
}

type Column = {
  title: string
  tasks: Task[]
}

const columns: Column[] = [
  {
    title: 'To do',
    tasks: [
      { title: 'Draft onboarding checklist', tag: 'Design', assignee: 'MK', priority: 'medium' },
      { title: 'Review Q3 renewal terms', tag: 'Contracts', assignee: 'JR', priority: 'low' },
    ],
  },
  {
    title: 'In progress',
    tasks: [
      {
        title: 'Prep board for Acme kickoff',
        tag: 'Meetings',
        assignee: 'AL',
        priority: 'high',
        meeting: 'Thu 10:00',
        contract: 'Acme · Q3 SOW',
      },
      { title: 'Fix invoice export bug', tag: 'Engineering', assignee: 'DT', priority: 'high' },
    ],
  },
  {
    title: 'Done',
    tasks: [
      { title: 'Send client status update', tag: 'Clients', assignee: 'JR', priority: 'medium' },
      { title: 'Ship v2.4 release notes', tag: 'Engineering', assignee: 'MK', priority: 'low' },
    ],
  },
]

function ProductPreview() {
  return (
    <div className="tp-hero__preview">
      <div className="tp-hero__preview-chrome">
        <span className="tp-hero__preview-live">
          <span className="tp-hero__preview-live-dot" />
          Live
        </span>
        <span className="tp-hero__preview-url">trackly.app/acme/projects/website-redesign</span>
      </div>

      <div className="tp-hero__preview-board">
        {columns.map((column) => (
          <div key={column.title} className="tp-hero__preview-column">
            <div className="tp-hero__preview-column-head">
              <span>{column.title}</span>
              <span className="tp-hero__preview-count">{column.tasks.length}</span>
            </div>

            <div className="tp-hero__preview-cards">
              {column.tasks.map((task) => (
                <div key={task.title} className="tp-hero__preview-card">
                  <p className="tp-hero__preview-card-title">{task.title}</p>

                  <div className="tp-hero__preview-card-footer">
                    <span className="tp-hero__preview-card-tag">{task.tag}</span>
                    <span
                      className={`tp-hero__preview-card-priority tp-hero__preview-card-priority--${task.priority}`}
                      aria-label={`${task.priority} priority`}
                    />
                    <span className="tp-hero__preview-card-avatar">{task.assignee}</span>
                  </div>

                  {(task.meeting || task.contract) && (
                    <div className="tp-hero__preview-card-links">
                      {task.meeting && (
                        <span className="tp-hero__preview-card-link">
                          <Calendar size={11} />
                          {task.meeting}
                        </span>
                      )}
                      {task.contract && (
                        <span className="tp-hero__preview-card-link">
                          <FileText size={11} />
                          {task.contract}
                        </span>
                      )}
                    </div>
                  )}
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default function HeroSection() {
  return (
    <section className="tp-hero">
      <div className="tp-container tp-hero__inner">
        <h1 className="tp-hero__title">
          Your team&rsquo;s work, <span className="tp-hero__title-accent">together in one place.</span>
        </h1>

        <p className="tp-hero__subtitle">
          Trackly brings tasks, meetings, client updates, and contracts into one workspace,
          so your team always knows what&rsquo;s happening and what&rsquo;s next.
        </p>

        <div className="tp-hero__actions">
          <Link to="/register" className="tp-btn tp-btn--primary tp-btn--lg">
            Get started for free
            <ArrowRight size={16} />
          </Link>
          <a href="#plans" className="tp-btn tp-btn--secondary tp-btn--lg">
            See plans
          </a>
        </div>

        <p className="tp-hero__note">Free plan available · No credit card required</p>
      </div>

      <ProductPreview />
    </section>
  )
}
