import { Link } from 'react-router-dom'
import { Moon, Sun } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { useTheme } from '@/hooks/useTheme'

export default function NavBar() {
  const { theme, toggleTheme } = useTheme()

  return (
    <nav className="fixed top-0 left-0 right-0 z-50 border-b border-border/40 bg-background/80 backdrop-blur-md">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
        <div className="flex h-16 items-center justify-between">

          <Link to="/" className="flex items-center gap-2.5">
            <div className="flex h-7 w-7 items-center justify-center rounded-lg bg-violet-600">
              <span className="text-xs font-bold text-white">T</span>
            </div>
            <span className="text-sm font-semibold text-foreground tracking-tight">Trackly</span>
          </Link>

          <div className="hidden items-center gap-7 md:flex">
            <a href="#features" className="text-sm text-muted-foreground hover:text-foreground transition-colors">Features</a>
            <a href="#pricing" className="text-sm text-muted-foreground hover:text-foreground transition-colors">Pricing</a>
            <a href="#" className="text-sm text-muted-foreground hover:text-foreground transition-colors">Docs</a>
          </div>

          <div className="flex items-center gap-2">
            <button
              onClick={toggleTheme}
              className="flex h-8 w-8 items-center justify-center rounded-md text-muted-foreground hover:text-foreground hover:bg-muted transition-colors"
              aria-label="Toggle theme"
            >
              {theme === 'dark' ? <Sun size={15} /> : <Moon size={15} />}
            </button>
            <Link to="/login">
              <Button variant="ghost" size="sm" className="text-sm">Sign in</Button>
            </Link>
            <Link to="/register">
              <Button size="sm" className="bg-violet-600 hover:bg-violet-700 text-white text-sm">
                Get started
              </Button>
            </Link>
          </div>

        </div>
      </div>
    </nav>
  )
}
