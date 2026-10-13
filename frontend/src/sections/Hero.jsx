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
          <div className="hero-art" role="img" aria-label="A preview of a software product interface designed by Ali">
            <div className="studio-window">
              <div className="studio-toolbar">
                <div className="studio-brand">
                  <span className="studio-brand-mark">d</span>
                  <span>devora studio</span>
                </div>
                <div className="studio-window-controls" aria-hidden="true">
                  <span />
                  <span />
                  <span />
                </div>
                <span className="studio-live"><span />LIVE PREVIEW</span>
              </div>
              <div className="studio-workspace">
                <aside className="studio-sidebar" aria-hidden="true">
                  <span className="studio-sidebar-active" />
                  <span />
                  <span />
                  <span />
                  <span className="studio-sidebar-bottom" />
                </aside>
                <div className="studio-canvas">
                  <div className="studio-greeting">
                    <div>
                      <span className="studio-overline">DEVORA / WORKSPACE</span>
                      <strong>Software, thoughtfully built.</strong>
                    </div>
                    <span className="studio-avatar">A</span>
                  </div>
                  <div className="studio-metrics">
                    <div><span>Projects</span><strong>{String(site.projects.length).padStart(2, '0')}</strong><i>Selected work</i></div>
                    <div><span>Services</span><strong>{String(site.services.length).padStart(2, '0')}</strong><i>Design + engineering</i></div>
                    <div><span>Courses</span><strong>{String(site.courses.length).padStart(2, '0')}</strong><i>Learning resources</i></div>
                  </div>
                  <div className="studio-project">
                    <div className="studio-project-heading">
                      <div><span className="studio-overline">CURRENT FOCUS</span><strong>Product, thoughtfully built.</strong></div>
                      <span className="studio-project-status">IN PROGRESS</span>
                    </div>
                    <div className="studio-project-preview" aria-hidden="true">
                      <div className="studio-preview-nav"><span /><span /><span /><span /></div>
                      <div className="studio-preview-content">
                        <span className="studio-preview-kicker">A clearer way forward</span>
                        <span className="studio-preview-title">Made for the work<br />that matters.</span>
                        <span className="studio-preview-button" />
                        <span className="studio-preview-art"><span /><span /><span /></span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div className="hero-art-bottomline">
              <span>Designed with purpose</span>
              <span>Built to last</span>
            </div>
          </div>
          <p className="hero-visual-note">Curiosity in the details.<br />Care in the systems.</p>
        </div>
      </div>
    </section>
  )
}