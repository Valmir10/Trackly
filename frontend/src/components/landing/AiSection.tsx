import { Sparkles, User } from 'lucide-react'

const messages = [
  {
    role: 'user' as const,
    content: 'Break down "Add user authentication" into subtasks',
  },
  {
    role: 'ai' as const,
    content: null,
    tasks: [
      'Design login and register UI',
      'Implement JWT access + refresh tokens',
      'Add BCrypt password hashing',
      'Build email verification flow',
      'Create forgot password with timed tokens',
      'Write integration tests for auth endpoints',
    ],
  },
]

const highlights = [
  {
    title: 'Task breakdown',
    description: 'Paste a feature description and get 5–10 concrete subtasks in seconds.',
  },
  {
    title: 'Weekly summaries',
    description: "Every Friday, your team gets an AI-written summary of the week's progress by email.",
  },
  {
    title: 'Smart prioritization',
    description: 'Ask "what should I work on next?" and get ranked suggestions based on deadlines and blockers.',
  },
  {
    title: 'Project chat',
    description: 'A sidebar chat that understands your project — ask about status, blockers, or anything.',
  },
]

export default function AiSection() {
  return (
    <section className="py-24 border-t border-border/40">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">

        <div className="grid items-center gap-16 lg:grid-cols-2">

          <div>
            <p className="mb-3 text-sm font-medium text-violet-400 uppercase tracking-wider">AI assistant</p>
            <h2 className="text-3xl font-semibold tracking-tight text-foreground sm:text-4xl">
              AI that actually helps
            </h2>
            <p className="mt-4 text-muted-foreground leading-relaxed">
              Trackly's AI is trained to understand project context — not just answer questions,
              but actively help your team move faster.
            </p>

            <div className="mt-10 flex flex-col gap-6">
              {highlights.map((item) => (
                <div key={item.title} className="flex gap-4">
                  <div className="mt-0.5 flex h-5 w-5 shrink-0 items-center justify-center rounded-full bg-violet-500/15">
                    <div className="h-1.5 w-1.5 rounded-full bg-violet-400" />
                  </div>
                  <div>
                    <p className="text-sm font-medium text-foreground">{item.title}</p>
                    <p className="mt-0.5 text-sm text-muted-foreground">{item.description}</p>
                  </div>
                </div>
              ))}
            </div>
          </div>

          <div className="rounded-xl border border-border/60 bg-card overflow-hidden shadow-xl">
            <div className="flex items-center gap-2 border-b border-border/60 bg-muted/30 px-4 py-3">
              <Sparkles size={14} className="text-violet-400" />
              <span className="text-xs font-medium text-foreground">AI Assistant</span>
              <span className="ml-auto flex items-center gap-1 text-[10px] text-green-400">
                <div className="h-1.5 w-1.5 rounded-full bg-green-400" />
                Online
              </span>
            </div>

            <div className="flex flex-col gap-4 p-4">
              {messages.map((msg, i) => (
                <div key={i} className={`flex gap-3 ${msg.role === 'user' ? 'flex-row-reverse' : ''}`}>
                  <div className={`flex h-7 w-7 shrink-0 items-center justify-center rounded-full ${
                    msg.role === 'ai' ? 'bg-violet-500/15' : 'bg-muted'
                  }`}>
                    {msg.role === 'ai'
                      ? <Sparkles size={13} className="text-violet-400" />
                      : <User size={13} className="text-muted-foreground" />
                    }
                  </div>
                  <div className={`rounded-xl px-4 py-3 text-sm max-w-[80%] ${
                    msg.role === 'user'
                      ? 'bg-violet-600 text-white'
                      : 'bg-muted/50 text-foreground border border-border/40'
                  }`}>
                    {msg.role === 'user' && <p>{msg.content}</p>}
                    {msg.role === 'ai' && msg.tasks && (
                      <div>
                        <p className="mb-2 text-xs text-muted-foreground">Here are the subtasks I'd suggest:</p>
                        <ul className="flex flex-col gap-1.5">
                          {msg.tasks.map((task, j) => (
                            <li key={j} className="flex items-start gap-2 text-xs">
                              <div className="mt-1 h-1.5 w-1.5 shrink-0 rounded-full bg-violet-400" />
                              {task}
                            </li>
                          ))}
                        </ul>
                      </div>
                    )}
                  </div>
                </div>
              ))}

              <div className="mt-2 flex items-center gap-2 rounded-lg border border-border/60 bg-background/50 px-3 py-2">
                <input
                  type="text"
                  placeholder="Ask about your project..."
                  className="flex-1 bg-transparent text-xs text-foreground placeholder:text-muted-foreground outline-none"
                  readOnly
                />
                <Sparkles size={13} className="text-violet-400" />
              </div>
            </div>
          </div>

        </div>

      </div>
    </section>
  )
}
