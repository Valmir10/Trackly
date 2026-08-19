import { Check } from 'lucide-react'
import { useClientRoomSummary } from '@/hooks/useClientRoomSummary'
import { useApproveMilestone } from '@/hooks/useApproveMilestone'
import '@/styles/tokens.css'
import '@/styles/tp-primitives.css'
import '@/styles/ClientRoomPage.css'

export default function ClientRoomPage() {
  const { data: summary, isLoading, isError } = useClientRoomSummary()
  const approveMilestone = useApproveMilestone()

  if (isLoading) {
    return (
      <div className="tp-shell tp-client-room">
        <div className="tp-client-room__center">Loading…</div>
      </div>
    )
  }

  if (isError || !summary) {
    return (
      <div className="tp-shell tp-client-room">
        <div className="tp-client-room__center">
          <h1 className="tp-client-room__invalid-title">This link is no longer valid</h1>
          <p className="tp-client-room__invalid-text">
            It may have been revoked or expired. Contact your project team for a new link.
          </p>
        </div>
      </div>
    )
  }

  return (
    <div className="tp-shell tp-client-room">
      <header className="tp-client-room__header">
        <span className="tp-client-room__dot" style={{ background: summary.projectColor }} />
        <h1 className="tp-client-room__project-name">{summary.projectName}</h1>
      </header>

      <main className="tp-client-room__body">
        {summary.contracts.length === 0 && (
          <p className="tp-client-room__empty">Nothing has been shared here yet.</p>
        )}

        {summary.contracts.map((contract) => (
          <section key={contract.id} className="tp-client-room__contract">
            <h2 className="tp-client-room__contract-title">{contract.title}</h2>

            {contract.milestones.length === 0 ? (
              <p className="tp-client-room__empty">No milestones yet.</p>
            ) : (
              <ul className="tp-client-room__milestones">
                {contract.milestones.map((milestone) => (
                  <li key={milestone.id} className="tp-client-room__milestone">
                    <div className="tp-client-room__milestone-head">
                      <span className="tp-client-room__milestone-title">{milestone.title}</span>
                      <span className="tp-client-room__milestone-count">
                        {milestone.ticketsDone}/{milestone.ticketsTotal} tickets done
                      </span>
                    </div>

                    <div className="tp-client-room__progress-track">
                      <div
                        className="tp-client-room__progress-fill"
                        style={{ width: `${milestone.progressPercentage}%` }}
                      />
                    </div>

                    <div className="tp-client-room__milestone-foot">
                      {milestone.isApproved ? (
                        <span className="tp-client-room__approved">
                          <Check size={13} />
                          Approved
                          {milestone.approvedAt && ` on ${new Date(milestone.approvedAt).toLocaleDateString()}`}
                        </span>
                      ) : (
                        <button
                          type="button"
                          className="tp-btn tp-btn--primary tp-btn--sm"
                          disabled={approveMilestone.isPending}
                          onClick={() => approveMilestone.mutate(milestone.id)}
                        >
                          {approveMilestone.isPending ? 'Approving…' : 'Approve'}
                        </button>
                      )}
                    </div>
                  </li>
                ))}
              </ul>
            )}
          </section>
        ))}
      </main>
    </div>
  )
}
