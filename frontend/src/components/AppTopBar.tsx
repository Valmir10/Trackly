import { useRef, useState } from 'react'
import { Link } from 'react-router-dom'
import { Search, Bell, HelpCircle, Menu, Settings, LogOut } from 'lucide-react'
import { useClickOutside } from '@/hooks/useClickOutside'
import '@/styles/AppTopBar.css'

const slug = 'acme-corp'

interface AppTopBarProps {
  onOpenMobileNav: () => void
}

export default function AppTopBar({ onOpenMobileNav }: AppTopBarProps) {
  const [menuOpen, setMenuOpen] = useState(false)
  const menuRef = useRef<HTMLDivElement>(null)
  useClickOutside(menuRef, () => setMenuOpen(false), menuOpen)

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

      <button type="button" className="tp-topbar__search">
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
              <Link to="/login" className="tp-menu-item" onClick={() => setMenuOpen(false)}>
                <LogOut size={14} />
                Sign out
              </Link>
            </div>
          )}
        </div>
      </div>
    </header>
  )
}
