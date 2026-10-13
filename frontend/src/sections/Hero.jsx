import { Link } from 'react-router-dom'
import { site } from '../config/site.js'
import SocialLinks from '../components/SocialLinks.jsx'

export default function Hero() {
  return (
    <section className="hero" id="top">
      <div className="container hero-grid">
        <div className="hero-copy">
          <p className="eyebrow"><span className="availability-dot" /> FULL-STACK DEVELOPER</p>
          <h1>
            Software with
            <span> a human side.</span>
          </h1>
          <p className="hero-signature">I&apos;m Ali Hussain — a Computer Science student and developer.</p>
          <p className="lead">{site.tagline}</p>

          <div className="hero-actions">
            <Link className="btn btn-primary" to="/projects">
              Explore my work <span aria-hidden="true">↗</span>
            </Link>
            <Link className="btn btn-ghost" to="/contact">
              Let&apos;s talk
            </Link>
          </div>

          <div className="badge-row" aria-label="Technology stack">
            {site.techBadges.map((badge) => (
              <span className="tech-badge" key={badge}>
                {badge}
              </span>
            ))}
          </div>

          <SocialLinks variant="icons" />
        </div>

        <div className="hero-visual">
          <div className="hero-art" role="img" aria-label="A visual introduction to Ali's software practice">
            <div className="hero-art-topline">
              <span>ALI HUSSAIN</span>
              <span>SOFTWARE / 01</span>
            </div>
            <div className="hero-art-grid" aria-hidden="true" />
            <div className="hero-art-monogram" aria-hidden="true">AH</div>
            <div className="hero-art-stamp" aria-hidden="true">BUILT<br />WITH<br />INTENT</div>
            <div className="hero-art-bottomline">
              <span>From first sketch</span>
              <span aria-hidden="true">→</span>
              <span>to the final detail</span>
            </div>
          </div>
          <p className="hero-visual-note">Curiosity in the details.<br />Care in the systems.</p>
        </div>
      </div>
    </section>
  )
}