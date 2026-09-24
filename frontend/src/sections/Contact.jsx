import { site } from '../config/site.js'
import ConnectSection from '../components/ConnectSection.jsx'

export default function Contact() {
  return (
    <section className="section" id="contact">
      <div className="container">
        <h2 className="section-title">Contact</h2>
        <ConnectSection />
        {site.email && (
          <p className="email-line">
            Or write to <a href={`mailto:${site.email}`}>{site.email}</a>
          </p>
        )}
      </div>
    </section>
  )
}