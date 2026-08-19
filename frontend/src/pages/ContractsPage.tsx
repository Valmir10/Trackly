import { useEffect, useState } from 'react'
import { useParams, useSearchParams } from 'react-router-dom'
import { Plus, FileText, Check } from 'lucide-react'
import AppShell from '@/components/AppShell'
import { useProjects } from '@/hooks/useProjects'
import { useProjectContracts } from '@/hooks/useProjectContracts'
import { useProjectMilestones } from '@/hooks/useProjectMilestones'
import { useCreateContract } from '@/hooks/useCreateContract'
import { useCreateMilestone } from '@/hooks/useCreateMilestone'
import '@/styles/ContractsPage.css'

export default function ContractsPage() {
  const { projectId = '' } = useParams()
  const [searchParams] = useSearchParams()
  const highlightedMilestoneId = searchParams.get('milestone')
  const { data: projects = [] } = useProjects()
  const { data: contracts = [] } = useProjectContracts(projectId)
  const { data: milestones = [] } = useProjectMilestones(projectId)
  const createContract = useCreateContract(projectId)
  const createMilestone = useCreateMilestone(projectId)

  const project = projects.find((p) => p.id === projectId)

  // Deep-linked from the ⌘K milestone source (?milestone=<id>) — scroll it
  // into view once its data has actually rendered, not on every render.
  useEffect(() => {
    if (!highlightedMilestoneId) return
    document.getElementById(`milestone-${highlightedMilestoneId}`)?.scrollIntoView({ block: 'center', behavior: 'smooth' })
  }, [highlightedMilestoneId, milestones])

  const [newContractTitle, setNewContractTitle] = useState('')
  const [newMilestoneTitleByContract, setNewMilestoneTitleByContract] = useState<Record<string, string>>({})

  function handleCreateContract(e: React.FormEvent) {
    e.preventDefault()
    if (!newContractTitle.trim()) return
    createContract.mutate(newContractTitle.trim(), { onSuccess: () => setNewContractTitle('') })
  }

  function handleCreateMilestone(contractId: string) {
    const title = newMilestoneTitleByContract[contractId]?.trim()
    if (!title) return
    createMilestone.mutate(
      { contractId, title },
      { onSuccess: () => setNewMilestoneTitleByContract((prev) => ({ ...prev, [contractId]: '' })) }
    )
  }

  return (
    <AppShell>
      <div className="tp-contracts-page">
        <div className="tp-page-header">
          <h1 className="tp-page-header__title">Contracts</h1>
          <p className="tp-page-header__subtitle">{project?.name ?? ''} — milestones drive the Client Room's progress view.</p>
        </div>

        <form className="tp-contracts-page__new-contract" onSubmit={handleCreateContract}>
          <input
            type="text"
            className="tp-input"
            placeholder="New contract title"
            value={newContractTitle}
            onChange={(e) => setNewContractTitle(e.target.value)}
          />
          <button type="submit" className="tp-btn tp-btn--primary tp-btn--sm" disabled={createContract.isPending}>
            <Plus size={13} />
            Add contract
          </button>
        </form>

        {contracts.length === 0 ? (
          <div className="tp-contracts-page__empty">
            <FileText size={24} />
            <p>No contracts yet</p>
          </div>
        ) : (
          <div className="tp-contracts-page__list">
            {contracts.map((contract) => {
              const contractMilestones = milestones.filter((m) => m.contractId === contract.id)
              return (
                <div key={contract.id} className="tp-contracts-page__contract">
                  <h2 className="tp-contracts-page__contract-title">{contract.title}</h2>

                  {contractMilestones.length > 0 && (
                    <ul className="tp-contracts-page__milestones">
                      {contractMilestones.map((milestone) => (
                        <li
                          key={milestone.id}
                          id={`milestone-${milestone.id}`}
                          className={`tp-contracts-page__milestone${
                            milestone.id === highlightedMilestoneId ? ' tp-contracts-page__milestone--highlighted' : ''
                          }`}
                        >
                          <span className="tp-contracts-page__milestone-title">{milestone.title}</span>
                          <span className="tp-contracts-page__milestone-progress">
                            {milestone.ticketsDone}/{milestone.ticketsTotal} tickets ({milestone.progressPercentage}%)
                          </span>
                          {milestone.isApproved && (
                            <span className="tp-contracts-page__milestone-approved">
                              <Check size={12} />
                              Approved
                            </span>
                          )}
                        </li>
                      ))}
                    </ul>
                  )}

                  <form
                    className="tp-contracts-page__new-milestone"
                    onSubmit={(e) => {
                      e.preventDefault()
                      handleCreateMilestone(contract.id)
                    }}
                  >
                    <input
                      type="text"
                      className="tp-input"
                      placeholder="New milestone title"
                      value={newMilestoneTitleByContract[contract.id] ?? ''}
                      onChange={(e) =>
                        setNewMilestoneTitleByContract((prev) => ({ ...prev, [contract.id]: e.target.value }))
                      }
                    />
                    <button type="submit" className="tp-btn tp-btn--secondary tp-btn--sm" disabled={createMilestone.isPending}>
                      <Plus size={12} />
                      Add milestone
                    </button>
                  </form>
                </div>
              )
            })}
          </div>
        )}
      </div>
    </AppShell>
  )
}
