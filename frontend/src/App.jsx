import { useEffect, useState } from 'react'
import { BrowserRouter, Link, Route, Routes, useLocation, useParams } from 'react-router-dom'
import Header from './components/Header.jsx'
import Hero from './sections/Hero.jsx'
import About from './sections/About.jsx'
import Footer from './sections/Footer.jsx'
import Projects from './sections/Projects.jsx'
import { site } from './config/site.js'
import AdminPage from './pages/AdminPage.jsx'
import { useCourses } from './hooks/useCourses.js'
import { apiUrl } from './config/api.js'

function ScrollToTop() {
  const { pathname } = useLocation()

  useEffect(() => {
    window.scrollTo({ top: 0, left: 0, behavior: 'instant' })
  }, [pathname])

  return null
}

function PageIntro({ eyebrow, title, description }) {
  return (
    <div className="page-intro">
      {eyebrow && <span className="eyebrow">{eyebrow}</span>}
      <h1>{title}</h1>
      {description && <p>{description}</p>}
    </div>
  )
}

function HomePage() {
  const { courses } = useCourses()
  const previewCourses = courses.slice(0, 3)

  return (
    <>
      <Hero />

      <section className="trust-strip" aria-label="Portfolio highlights">
        <div className="container trust-grid">
          {site.services.slice(0, 4).map((service, index) => (
            <div key={service.title}>
              <span className="service-index">0{index + 1}</span>
              <strong>{service.title.replace(' Development', '')}</strong>
              <span>{service.points[0]}</span>
            </div>
          ))}
        </div>
      </section>

      <About />

      <section className="section section-surface" id="services">
        <div className="container">
          <div className="section-heading">
            <span className="eyebrow eyebrow-dark">What I do</span>
            <h2>Practical software solutions for real business needs.</h2>
          </div>
          <div className="card-grid three-up">
            {site.services.map((service) => (
              <article className="feature-card" key={service.title}>
                <div className="feature-icon">✦</div>
                <h3>{service.title}</h3>
                <p>{service.description}</p>
                <ul>
                  {service.points.map((point) => (
                    <li key={point}>{point}</li>
                  ))}
                </ul>
              </article>
            ))}
          </div>
        </div>
      </section>

      <section className="section section-alt" id="projects-overview">
        <div className="container">
          <div className="section-heading split-heading">
            <div>
              <span className="eyebrow eyebrow-dark">Selected work</span>
              <h2>Projects designed to solve tangible problems.</h2>
            </div>
            <Link className="text-link" to="/projects">
              Explore all projects →
            </Link>
          </div>
          <Projects limit={3} />
        </div>
      </section>

      <section className="section section-surface" id="courses-overview">
        <div className="container">
          <div className="section-heading split-heading">
            <div>
              <span className="eyebrow eyebrow-dark">Learning resources</span>
              <h2>Tools, lessons, and practical learning paths.</h2>
            </div>
            <Link className="text-link" to="/courses">
              View courses →
            </Link>
          </div>
          <div className="card-grid three-up">
            {previewCourses.map((course, index) => (
              <article className="course-card" key={course.slug}>
                <div className="course-topline">
                  <span className="course-number">0{index + 1}</span>
                  <span className="pill">{course.level}</span>
                </div>
                <div className="course-copy">
                  <h3>{course.title}</h3>
                  <p>{course.description}</p>
                </div>
                <div className="course-bottom">
                  <div className="meta-row">
                    <span>{course.duration}</span>
                    <span>{course.lessons} lessons</span>
                  </div>
                  <Link to={`/courses/${course.slug}`} className="course-link" aria-label={`Learn more about ${course.title}`}>
                    <span>Explore</span>
                    <span aria-hidden="true">↗</span>
                  </Link>
                </div>
              </article>
            ))}
          </div>
        </div>
      </section>

      <section className="section">
        <div className="container">
          <div className="cta-box">
            <div>
              <span className="eyebrow eyebrow-dark">Let’s build</span>
              <h2>Need a reliable development partner for your next product or platform?</h2>
            </div>
            <Link className="btn btn-primary" to="/contact">
              Hire Me
            </Link>
          </div>
        </div>
      </section>
    </>
  )
}

function AboutPage() {
  return (
    <div className="page-container">
      <PageIntro
        eyebrow="About"
        title="Building software with intention, clarity, and maintainability."
        description="I focus on creating dependable digital products that support real-world business goals and provide excellent user experiences."
      />
      <section className="section section-surface">
        <div className="container two-column-layout">
          <div className="content-card">
            <h2>My approach</h2>
            {site.about.map((paragraph) => (
              <p key={paragraph}>{paragraph}</p>
            ))}
          </div>
          <div className="content-card">
            <h2>Core focus</h2>
            <ul className="stack-list">
              <li>Business-driven product thinking</li>
              <li>Clear architecture and maintainable code</li>
              <li>Responsive, accessible user interfaces</li>
              <li>Secure and scalable backend systems</li>
            </ul>
          </div>
        </div>
      </section>

      <section className="section" id="skills">
        <div className="container">
          <div className="section-heading">
            <span className="eyebrow eyebrow-dark">Skill set</span>
            <h2>Technologies I work with.</h2>
          </div>
          <div className="tag-cloud">
            {site.skills.map((skill) => (
              <span className="tag" key={skill}>{skill}</span>
            ))}
          </div>
        </div>
      </section>
    </div>
  )
}

function ServicesPage() {
  return (
    <div className="page-container">
      <PageIntro
        eyebrow="Services"
        title="Development services designed to move ideas into production."
        description="I help founders, teams, and businesses turn needs into reliable digital experiences built on modern tools and clear engineering practices."
      />
      <section className="section section-surface">
        <div className="container">
          <div className="card-grid three-up">
            {site.services.map((service) => (
              <article className="feature-card" key={service.title}>
                <div className="feature-icon">✦</div>
                <h3>{service.title}</h3>
                <p>{service.description}</p>
                <ul>
                  {service.points.map((point) => (
                    <li key={point}>{point}</li>
                  ))}
                </ul>
              </article>
            ))}
          </div>
        </div>
      </section>
    </div>
  )
}

function ProjectsPage() {
  return (
    <div className="page-container">
      <PageIntro
        eyebrow="Projects"
        title="Selected work with a focus on clarity, usability, and impact."
        description="A mix of platform UI work, business dashboards, and practical systems that help teams work more effectively."
      />
      <section className="section section-surface">
        <div className="container">
          <Projects />
        </div>
      </section>
    </div>
  )
}

function ProjectDetailPage() {
  const { slug } = useParams()
  const project = site.projects.find((item) => item.slug === slug)

  if (!project) {
    return <NotFoundPage />
  }

  return (
    <div className="page-container">
      <section className="section section-surface">
        <div className="container detail-layout">
          <div className="detail-card">
            <span className="eyebrow eyebrow-dark">Project</span>
            <h1>{project.title}</h1>
            <p>{project.summary}</p>
            <div className="tag-cloud compact">
              {project.techStack.split(',').map((tech) => (
                <span className="tag" key={tech}>{tech.trim()}</span>
              ))}
            </div>
            <div className="detail-actions">
              {project.liveUrl && <a className="btn btn-primary" href={project.liveUrl}>View Demo</a>}
              {project.link && <a className="btn btn-ghost-dark" href={project.link}>View Source</a>}
            </div>
          </div>
          <div className="info-panel">
            <h3>Highlights</h3>
            <ul className="stack-list">
              <li>Product-focused design decisions</li>
              <li>Clean, scalable technical implementation</li>
              <li>Responsive and maintainable frontend experience</li>
            </ul>
          </div>
        </div>
      </section>
    </div>
  )
}

function CoursesPage() {
  const { courses, loading, error } = useCourses()

  return (
    <div className="page-container">
      <PageIntro
        eyebrow="Courses"
        title="Learning resources built for developers who want practical skills."
        description="Short, focused learning tracks covering modern web development, APIs, and database design for real-world software work."
      />
      <section className="section section-surface">
        <div className="container">
          {loading && <p className="status">Loading courses…</p>}
          {error && <p className="status status-error" role="alert">Couldn&apos;t load live courses ({error}); showing the local snapshot.</p>}
          <div className="card-grid three-up">
            {courses.map((course) => (
              <article className="course-card" key={course.slug}>
                <span className="pill">{course.level}</span>
                <h3>{course.title}</h3>
                <p>{course.description}</p>
                <div className="meta-row">
                  <span>{course.duration}</span>
                  <span>{course.lessons} lessons</span>
                </div>
                <Link to={`/courses/${course.slug}`} className="inline-link">
                  Learn more
                </Link>
              </article>
            ))}
          </div>
        </div>
      </section>
    </div>
  )
}

function CourseDetailPage() {
  const { slug } = useParams()
  const course = site.courses.find((item) => item.slug === slug)

  if (!course) {
    return <NotFoundPage />
  }

  return (
    <div className="page-container">
      <section className="section section-surface">
        <div className="container detail-layout">
          <div className="detail-card">
            <span className="eyebrow eyebrow-dark">Course</span>
            <h1>{course.title}</h1>
            <p>{course.description}</p>
            <div className="meta-row detail-meta">
              <span>{course.level}</span>
              <span>{course.duration}</span>
              <span>{course.lessons} lessons</span>
            </div>
            <div className="detail-actions">
              <Link className="btn btn-primary" to="/courses">
                Back to courses
              </Link>
            </div>
          </div>
          <div className="info-panel">
            <h3>What you’ll cover</h3>
            <ul className="stack-list">
              <li>Modern development principles</li>
              <li>Application and API structure</li>
              <li>Database and business logic best practices</li>
              <li>Practical implementation and iteration</li>
            </ul>
          </div>
        </div>
      </section>
    </div>
  )
}

function ResourceDetailPage() {
  const { id } = useParams()
  const resource = site.resources.find((item) => item.id === id)

  if (!resource) {
    return <NotFoundPage />
  }

  return (
    <div className="page-container">
      <section className="section section-surface">
        <div className="container detail-layout">
          <div className="detail-card">
            <span className="eyebrow eyebrow-dark">Resource</span>
            <h1>{resource.title}</h1>
            <p>{resource.description}</p>
            <div className="meta-row detail-meta">
              <span>{resource.type}</span>
              <span>{resource.pageCount} pages</span>
            </div>
            <div className="detail-actions">
              <Link className="btn btn-primary" to="/courses">
                Explore more resources
              </Link>
            </div>
          </div>
          <div className="info-panel">
            <h3>Resource overview</h3>
            <ul className="stack-list">
              <li>Quick reference material</li>
              <li>Useful for learning and implementation</li>
              <li>Designed for practical application</li>
            </ul>
          </div>
        </div>
      </section>
    </div>
  )
}

function ContactPage() {
  const [status, setStatus] = useState('')
  const [error, setError] = useState('')
  const [busy, setBusy] = useState(false)

  const submitContact = async (event) => {
    event.preventDefault()
    setBusy(true)
    setStatus('')
    setError('')
    const form = new FormData(event.currentTarget)

    try {
      const response = await fetch(apiUrl('/api/contact'), {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          name: form.get('name'),
          email: form.get('email'),
          message: form.get('message'),
        }),
      })
      const result = await response.json()
      if (!response.ok) throw new Error(result.message || 'Could not send your message.')
      event.currentTarget.reset()
      setStatus('Thanks — your message has been sent successfully.')
    } catch (reason) {
      setError(reason.message || 'Could not send your message.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="page-container">
      <PageIntro
        eyebrow="Contact"
        title="Let’s talk about your next idea, product, or platform."
        description="Whether you need a web application, .NET solution, or support with a technical product, I’d be happy to connect."
      />
      <section className="section section-surface">
        <div className="container contact-layout">
          <div className="content-card">
            <h2>Reach out</h2>
            <p>
              I’m available for freelance work, technical collaboration, product consulting, and project discussions.
            </p>
            <div className="contact-list">
              <a href={`mailto:${site.email}`}>{site.email}</a>
              <a href={site.social.github} target="_blank" rel="noreferrer">GitHub</a>
              <a href={site.social.linkedin} target="_blank" rel="noreferrer">LinkedIn</a>
            </div>
          </div>
          <div className="content-card contact-form-card">
            <h2>Quick message</h2>
            <form className="contact-form" onSubmit={submitContact}>
              <label>
                Name
                <input name="name" type="text" placeholder="Your name" required />
              </label>
              <label>
                Email
                <input name="email" type="email" placeholder="you@example.com" required />
              </label>
              <label>
                Project details
                <textarea name="message" rows="5" placeholder="Tell me a little about your idea..." required />
              </label>
              {status && <p className="status status-success" role="status">{status}</p>}
              {error && <p className="status status-error" role="alert">{error}</p>}
              <button type="submit" className="btn btn-primary" disabled={busy}>{busy ? 'Sending…' : 'Send inquiry'}</button>
            </form>
          </div>
        </div>
      </section>
    </div>
  )
}

function NotFoundPage() {
  return (
    <div className="page-container">
      <section className="section">
        <div className="container not-found-shell">
          <span className="eyebrow eyebrow-dark">404</span>
          <h1>Page not found.</h1>
          <p>The page you’re looking for doesn’t exist or may have moved.</p>
          <Link className="btn btn-primary" to="/">
            Back to home
          </Link>
        </div>
      </section>
    </div>
  )
}

function AppLayout() {
  return (
    <>
      <a className="skip-link" href="#main-content">
        Skip to content
      </a>
      <Header />
      <main id="main-content">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/about" element={<AboutPage />} />
          <Route path="/services" element={<ServicesPage />} />
          <Route path="/projects" element={<ProjectsPage />} />
          <Route path="/projects/:slug" element={<ProjectDetailPage />} />
          <Route path="/courses" element={<CoursesPage />} />
          <Route path="/courses/:slug" element={<CourseDetailPage />} />
          <Route path="/resources/:id" element={<ResourceDetailPage />} />
          <Route path="/contact" element={<ContactPage />} />
          <Route path="/login" element={<AdminPage />} />
          <Route path="/admin" element={<AdminPage />} />
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </main>
      <Footer />
    </>
  )
}

export default function App() {
  return (
    <BrowserRouter>
      <ScrollToTop />
      <AppLayout />
    </BrowserRouter>
  )
}