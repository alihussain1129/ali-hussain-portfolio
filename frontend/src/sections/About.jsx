import { Link } from 'react-router-dom'
import { site } from '../config/site.js'

export default function About() {
  return (
    <section className="section" id="about">
      <div className="container about-grid">
        <div className="content-card">
          <span className="eyebrow eyebrow-dark">About</span>
          <h2 className="section-title">A developer focused on clean systems and practical outcomes.</h2>
          {site.about.map((paragraph) => (
            <p key={paragraph}>{paragraph}</p>
          ))}
          <Link className="inline-link" to="/about">
            Read more about my process →
          </Link>
        </div>

        <div className="content-card">
          <h3>Core technologies</h3>
          <ul className="tags tags-large">
            {site.skills.map((skill) => (
              <li key={skill}>{skill}</li>
            ))}
          </ul>
        </div>
      </div>
    </section>
  )
}