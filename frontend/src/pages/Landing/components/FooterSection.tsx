import { Link } from 'react-router-dom'

const links = {
  Product: ['Features', 'Pricing', 'Changelog', 'Roadmap'],
  Company: ['About', 'Blog', 'Careers', 'Press'],
  Legal: ['Privacy', 'Terms', 'Security', 'Cookies'],
  Support: ['Documentation', 'Status', 'Contact', 'Community'],
}

export default function FooterSection() {
  return (
    <footer className="border-t border-border/40 py-16">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">

        <div className="grid gap-12 sm:grid-cols-2 lg:grid-cols-5">

          <div className="lg:col-span-1">
            <Link to="/" className="flex items-center gap-2.5">
              <div className="flex h-7 w-7 items-center justify-center rounded-lg bg-violet-600">
                <span className="text-xs font-bold text-white">T</span>
              </div>
              <span className="text-sm font-semibold text-foreground tracking-tight">Trackly</span>
            </Link>
            <p className="mt-4 text-xs text-muted-foreground leading-relaxed max-w-[180px]">
              Project management for modern teams, powered by AI.
            </p>
          </div>

          {Object.entries(links).map(([category, items]) => (
            <div key={category}>
              <p className="mb-4 text-xs font-semibold text-foreground uppercase tracking-wider">{category}</p>
              <ul className="flex flex-col gap-2.5">
                {items.map((item) => (
                  <li key={item}>
                    <a href="#" className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                      {item}
                    </a>
                  </li>
                ))}
              </ul>
            </div>
          ))}

        </div>

        <div className="mt-16 flex flex-col items-center justify-between gap-4 border-t border-border/40 pt-8 sm:flex-row">
          <p className="text-xs text-muted-foreground">
            © {new Date().getFullYear()} Trackly. All rights reserved.
          </p>
          <p className="text-xs text-muted-foreground">
            Built with React, .NET 10, and PostgreSQL.
          </p>
        </div>

      </div>
    </footer>
  )
}
