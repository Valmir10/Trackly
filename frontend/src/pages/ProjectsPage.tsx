import { Plus } from 'lucide-react'
import { Button } from '@/components/ui/button'
import AppSidebar from '@/components/AppSidebar'
import AppTopBar from '@/components/AppTopBar'
import ProjectCard from '@/components/ProjectCard'

const projects = [
  {
    id: '1',
    name: 'Frontend redesign',
    description: 'Complete overhaul of the user interface with new design system, improved accessibility and performance.',
    color: 'bg-rose-500',
    memberCount: 4,
    taskCount: 18,
    completedCount: 11,
    status: 'Active' as const,
    members: [
      { initials: 'VZ', color: 'bg-rose-500/15 text-rose-400' },
      { initials: 'SK', color: 'bg-blue-500/15 text-blue-400' },
      { initials: 'JS', color: 'bg-green-500/15 text-green-400' },
      { initials: 'AM', color: 'bg-orange-500/15 text-orange-400' },
    ],
  },
  {
    id: '2',
    name: 'API v2',
    description: 'New versioned REST API with improved authentication, rate limiting and comprehensive documentation.',
    color: 'bg-blue-500',
    memberCount: 3,
    taskCount: 24,
    completedCount: 9,
    status: 'Active' as const,
    members: [
      { initials: 'VZ', color: 'bg-rose-500/15 text-rose-400' },
      { initials: 'JS', color: 'bg-green-500/15 text-green-400' },
      { initials: 'LK', color: 'bg-pink-500/15 text-pink-400' },
    ],
  },
  {
    id: '3',
    name: 'Mobile app',
    description: 'Native iOS and Android application with real-time sync, offline support and push notifications.',
    color: 'bg-green-500',
    memberCount: 2,
    taskCount: 31,
    completedCount: 4,
    status: 'On hold' as const,
    members: [
      { initials: 'SK', color: 'bg-blue-500/15 text-blue-400' },
      { initials: 'AM', color: 'bg-orange-500/15 text-orange-400' },
    ],
  },
  {
    id: '4',
    name: 'Marketing site',
    description: 'Redesign of the public marketing website with new landing pages, blog and SEO improvements.',
    color: 'bg-orange-500',
    memberCount: 2,
    taskCount: 12,
    completedCount: 12,
    status: 'Completed' as const,
    members: [
      { initials: 'VZ', color: 'bg-rose-500/15 text-rose-400' },
      { initials: 'LK', color: 'bg-pink-500/15 text-pink-400' },
    ],
  },
]

export default function ProjectsPage() {
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
                <p className="mt-0.5 text-sm text-muted-foreground">{projects.length} projects in your workspace</p>
              </div>
              <Button className="gap-2 bg-rose-600 hover:bg-rose-700 text-white">
                <Plus size={15} />
                New project
              </Button>
            </div>

            <div className="mb-6 flex gap-1 border-b border-border/60">
              {['All projects', 'My projects', 'Archived'].map((tab, i) => (
                <button
                  key={tab}
                  className={`px-4 py-2 text-sm transition-colors ${
                    i === 0
                      ? 'border-b-2 border-rose-500 font-medium text-foreground'
                      : 'text-muted-foreground hover:text-foreground'
                  }`}
                >
                  {tab}
                </button>
              ))}
            </div>

            <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
              {projects.map((project) => (
                <ProjectCard key={project.id} {...project} />
              ))}
            </div>

          </div>
        </main>
      </div>
    </div>
  )
}
