import { useState } from 'react'
import { Link } from 'react-router-dom'
import { Moon, Sun, Menu, X } from 'lucide-react'
import { useTheme } from '@/hooks/useTheme'
import '@/styles/NavBar.css'

const LINKS = [
  { href: '#pillars', label: 'Product' },
  { href: '#plans', label: 'Plans' },
  { href: '#', label: 'Docs' },
]

export default function NavBar() {
  const { theme, toggleTheme } = useTheme()
  const [menuOpen, setMenuOpen] = useState(false)

  return (
    <nav className="tp-nav">
      <div className="tp-nav__inner">
        <Link to="/" className="tp-nav__brand" onClick={() => setMenuOpen(false)}>
          <span className="tp-nav__mark">T</span>
          <span className="tp-nav__name">Trackly</span>
        </Link>

        <div className="tp-nav__links">
          {LINKS.map((link) => (
            <a key={link.label} href={link.href} className="tp-nav__link">
              {link.label}
            </a>
          ))}
        </div>

        <div className="tp-nav__actions">
          <button
            type="button"
            onClick={toggleTheme}
            className="tp-nav__icon-btn"
            aria-label={theme === 'dark' ? 'Switch to light theme' : 'Switch to dark theme'}
          >
            {theme === 'dark' ? <Sun size={16} /> : <Moon size={16} />}
          </button>
          <Link to="/login" className="tp-nav__signin">Sign in</Link>
          <Link to="/register" className="tp-btn tp-btn--primary tp-btn--sm">Get started</Link>
          <button
            type="button"
            onClick={() => setMenuOpen((open) => !open)}
            className="tp-nav__icon-btn tp-nav__menu-toggle"
            aria-label={menuOpen ? 'Close menu' : 'Open menu'}
            aria-expanded={menuOpen}
          >
            {menuOpen ? <X size={18} /> : <Menu size={18} />}
          </button>
        </div>
      </div>

      {menuOpen && (
        <div className="tp-nav__mobile">
          {LINKS.map((link) => (
            <a
              key={link.label}
              href={link.href}
              className="tp-nav__mobile-link"
              onClick={() => setMenuOpen(false)}
            >
              {link.label}
            </a>
          ))}
          <Link to="/login" className="tp-nav__mobile-link" onClick={() => setMenuOpen(false)}>
            Sign in
          </Link>
        </div>
      )}
    </nav>
  )
}
