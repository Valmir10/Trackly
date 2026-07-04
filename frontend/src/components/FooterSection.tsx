import { Link } from 'react-router-dom'
import '@/styles/FooterSection.css'

const links = {
  Product: ['Tasks', 'Meetings', 'Clients', 'Contracts', 'Plans'],
  Company: ['About', 'Blog', 'Careers'],
  Legal: ['Privacy', 'Terms', 'Security'],
  Support: ['Documentation', 'Status', 'Contact'],
}

export default function FooterSection() {
  return (
    <footer className="tp-footer">
      <div className="tp-container">
        <div className="tp-footer__grid">
          <div className="tp-footer__brand">
            <Link to="/" className="tp-footer__logo">
              <span className="tp-footer__mark">T</span>
              <span className="tp-footer__name">Trackly</span>
            </Link>
            <p className="tp-footer__tagline">
              One workspace for tasks, meetings, clients, and contracts.
            </p>
          </div>

          {Object.entries(links).map(([category, items]) => (
            <div key={category}>
              <p className="tp-footer__category">{category}</p>
              <ul className="tp-footer__links">
                {items.map((item) => (
                  <li key={item}>
                    <a href="#">{item}</a>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>

        <div className="tp-footer__bottom">
          <p>© {new Date().getFullYear()} Trackly. All rights reserved.</p>
        </div>
      </div>
    </footer>
  )
}
