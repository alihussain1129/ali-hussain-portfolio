import { Link } from 'react-router-dom'
import { site } from '../config/site.js'
import SocialLinks from '../components/SocialLinks.jsx'

export default function Hero() {
  return (
    <section className="hero" id="top">
      <div className="container hero-grid">
        <div className="hero-copy">
          <p className="eyebrow">FULL-STACK DEVELOPER</p>
          <h1>{site.headline}</h1>
          <p className="hero-signature">I&apos;m {site.name}, a Computer Science student and developer.</p>
          <p className="lead">{site.tagline}</p>

          <div className="hero-actions">
            <Link className="btn btn-primary" to="/projects">
              Start a project
            </Link>
            <Link className="btn btn-ghost" to="/contact">
              Explore my work
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

        <div className="hero-visual" aria-hidden="true">
          <div className="visual-label">Engineering with intent <span>●</span></div>
          <div className="hero-art">
            <div className="art-orbit art-orbit-one" />
            <div className="art-orbit art-orbit-two" />
            <div className="art-core">
              <span className="art-core-mark">AH</span>
              <span className="art-core-label">DIGITAL<br />SYSTEMS</span>
            </div>
            <span className="art-node art-node-one">C#</span>
            <span className="art-node art-node-two">UI</span>
            <span className="art-node art-node-three">DB</span>
            <span className="art-node art-node-four">API</span>
          </div>
        </div>
      </div>
    </section>
  )
}