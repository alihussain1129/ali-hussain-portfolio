import { useState } from 'react'
import { Link, NavLink } from 'react-router-dom'
import { site } from '../config/site.js'

export default function Header() {
  const [menuOpen, setMenuOpen] = useState(false)

  const handleNavClick = () => setMenuOpen(false)

  return (
    <header className="site-header">
      <div className="container header-inner">
        <Link to="/" className="brand" aria-label="Ali Hussain home">
          <img className="brand-mark" src="/favicon.svg" alt="" />
          <span className="brand-name">{site.name}</span>
        </Link>

        <button
          type="button"
          className="nav-toggle"
          aria-expanded={menuOpen}
          aria-label="Toggle navigation"
          onClick={() => setMenuOpen((current) => !current)}
        >
          <span />
          <span />
          <span />
        </button>

        <nav className={`site-nav ${menuOpen ? 'open' : ''}`} aria-label="Main navigation">
          <ul className="nav">
            {site.nav.map(({ label, to }) => (
              <li key={to}>
                <NavLink
                  to={to}
                  className={({ isActive }) => (isActive ? 'active' : '')}
                  onClick={handleNavClick}
                >
                  {label}
                </NavLink>
              </li>
            ))}
          </ul>
          <Link className="btn btn-primary nav-cta" to="/contact" onClick={handleNavClick}>
            Let&apos;s Work Together
          </Link>
        </nav>
      </div>
    </header>
  )
}