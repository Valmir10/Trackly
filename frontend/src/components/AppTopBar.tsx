import { useRef, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { Search, Bell, HelpCircle, Menu, Settings, LogOut } from 'lucide-react'
import { useClickOutside } from '@/hooks/useClickOutside'
import { useCommandRegistry } from '@/commands/registry'
import { useAuthStore } from '@/store/useAuthStore'
import '@/styles/AppTopBar.css'

interface AppTopBarProps {
  onOpenMobileNav: () => void
}

export default function AppTopBar({ onOpenMobileNav }: AppTopBarProps) {
  const { slug = '' } = useParams()
  const navigate = useNavigate()
  const clearSession = useAuthStore((s) => s.clearSession)
  const [menuOpen, setMenuOpen] = useState(false)
  const menuRef = useRef<HTMLDivElement>(null)
  useClickOutside(menuRef, () => setMenuOpen(false), menuOpen)
  const openPalette = useCommandRegistry((s) => s.open)

  function handleSignOut() {
    clearSession()
    navigate('/login')
  }

  return (
    <header className="tp-topbar">
      <button
        type="button"
        className="tp-topbar__menu-toggle"
        onClick={onOpenMobileNav}
        aria-label="Open menu"
      >
        <Menu size={18} />
      </button>

      <button type="button" className="tp-topbar__search" onClick={openPalette}>
        <Search size={14} />
        <span>Search...</span>
        <kbd className="tp-topbar__kbd">⌘K</kbd>
      </button>

      <div className="tp-topbar__actions">
        <button type="button" className="tp-topbar__icon-btn" aria-label="Notifications">
          <Bell size={16} />
          <span className="tp-topbar__notification-dot" aria-hidden="true" />
        </button>
        <button type="button" className="tp-topbar__icon-btn" aria-label="Help">
          <HelpCircle size={16} />
        </button>

        <div className="tp-dropdown" ref={menuRef}>
          <button
            type="button"
            className="tp-topbar__avatar"
            onClick={() => setMenuOpen((open) => !open)}
            aria-label="Account menu"
            aria-expanded={menuOpen}
          >
            VZ
          </button>

          {menuOpen && (
            <div className="tp-dropdown__panel tp-topbar__menu-panel">
              <p className="tp-menu-label">Valmir Zogaj</p>
              <Link to={`/${slug}/settings`} className="tp-menu-item" onClick={() => setMenuOpen(false)}>
                <Settings size={14} />
                Settings
              </Link>
              <div className="tp-menu-divider" />
              <button
                type="button"
                className="tp-menu-item"
                onClick={() => {
                  setMenuOpen(false)
                  handleSignOut()
                }}
              >
                <LogOut size={14} />
                Sign out
              </button>
            </div>
          )}
        </div>
      </div>
    </header>
  )
}
