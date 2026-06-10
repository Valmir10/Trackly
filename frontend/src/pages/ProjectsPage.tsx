import { useState } from 'react'
import { Plus, FolderOpen } from 'lucide-react'
import { Button } from '@/components/ui/button'
import AppSidebar from '@/components/AppSidebar'
import AppTopBar from '@/components/AppTopBar'
import ProjectCard from '@/components/ProjectCard'
import NewProjectModal from '@/components/NewProjectModal'

const allProjects = [
  { id: '1', name: 'Frontend redesign', description: 'Complete overhaul of the user interface with new design system, improved accessibility and performance.', color: 'bg-rose-500', memberCount: 4, taskCount: 18, completedCount: 11, status: 'Active' as const, members: [{ initials: 'VZ', color: 'bg-rose-500/15 text-rose-400' }, { initials: 'SK', color: 'bg-blue-500/15 text-blue-400' }, { initials: 'JS', color: 'bg-green-500/15 text-green-400' }, { initials: 'AM', color: 'bg-orange-500/15 text-orange-400' }] },
  { id: '2', name: 'API v2', description: 'New versioned REST API with improved authentication, rate limiting and comprehensive documentation.', color: 'bg-blue-500', memberCount: 3, taskCount: 24, completedCount: 9, status: 'Active' as const, members: [{ initials: 'VZ', color: 'bg-rose-500/15 text-rose-400' }, { initials: 'JS', color: 'bg-green-500/15 text-green-400' }, { initials: 'LK', color: 'bg-pink-500/15 text-pink-400' }] },
  { id: '3', name: 'Mobile app', description: 'Native iOS and Android application with real-time sync, offline support and push notifications.', color: 'bg-green-500', memberCount: 2, taskCount: 31, completedCount: 4, status: 'On hold' as const, members: [{ initials: 'SK', color: 'bg-blue-500/15 text-blue-400' }, { initials: 'AM', color: 'bg-orange-500/15 text-orange-400' }] },
  { id: '4', name: 'Marketing site', description: 'Redesign of the public marketing website with new landing pages, blog and SEO improvements.', color: 'bg-orange-500', memberCount: 2, taskCount: 12, completedCount: 12, status: 'Completed' as const, members: [{ initials: 'VZ', color: 'bg-rose-500/15 text-rose-400' }, { initials: 'LK', color: 'bg-pink-500/15 text-pink-400' }] },
]

const myProjects = allProjects.filter(p => p.members.some(m => m.initials === 'VZ'))
const tabs = ['All projects', 'My projects', 'Archived']

function EmptyState({ title, description }: { title: string; description: string }) {
  return (
    <div className="flex flex-col items-center gap-3 py-20 text-center">
      <div className="flex h-14 w-14 items-center justify-center rounded-xl bg-muted">
        <FolderOpen size={24} className="text-muted-foreground" />
      </div>
      <div>
        <p className="text-sm font-medium text-foreground">{title}</p>
        <p className="mt-1 text-xs text-muted-foreground">{description}</p>
      </div>
    </div>
  )
}

export default function ProjectsPage() {
  const [activeTab, setActiveTab] = useState('All projects')
  const [showModal, setShowModal] = useState(false)

  const projects = activeTab === 'All projects' ? allProjects : activeTab === 'My projects' ? myProjects : []

  return (
    <div className="flex h-screen overflow-hidden bg-background">
      <AppSidebar />
      <div className="flex flex-1 flex-col overflow-hidden">
        <AppTopBar />
        <main className="flex-1 overflow-y-auto p-6">
          <div className="mx-auto max-w-6xl">

            <div className="mb-6 flex items-center justify-between">
              <div>
                <h1 className="text-xl font-semibold text-foreground">Projects</h1>
                <p className="mt-0.5 text-sm text-muted-foreground">{allProjects.length} projects in your workspace</p>
              </div>
              <Button onClick={() => setShowModal(true)} className="gap-2 bg-rose-500 hover:bg-rose-600 text-white">
                <Plus size={15} />
                New project
              </Button>
            </div>

            <div className="mb-6 flex gap-1 border-b border-border/60">
              {tabs.map((tab) => (
                <button
                  key={tab}
                  onClick={() => setActiveTab(tab)}
                  className={`px-4 py-2 text-sm transition-colors ${
                    activeTab === tab
                      ? 'border-b-2 border-rose-500 font-medium text-foreground'
                      : 'text-muted-foreground hover:text-foreground'
                  }`}
                >
                  {tab}
                </button>
              ))}
            </div>

            {activeTab === 'Archived' ? (
              <EmptyState title="No archived projects" description="Projects you archive will appear here." />
            ) : projects.length === 0 ? (
              <EmptyState title="No projects yet" description="You are not assigned to any projects." />
            ) : (
              <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
                {projects.map((project) => (
                  <ProjectCard key={project.id} {...project} />
                ))}
              </div>
            )}

          </div>
        </main>
      </div>

      {showModal && <NewProjectModal onClose={() => setShowModal(false)} />}
    </div>
  )
}
