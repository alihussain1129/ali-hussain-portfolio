import { Link } from 'react-router-dom'
import { useProjects } from '../hooks/useProjects.js'

export default function Projects({ limit }) {
  const { projects, loading, error } = useProjects()
  const visibleProjects = typeof limit === 'number' ? projects.slice(0, limit) : projects

  return (
    <>
      {loading && <p className="status">Loading projects…</p>}
      {error && (
        <p className="status status-error" role="alert">
          Couldn&apos;t load projects ({error}). Using the local project snapshot.
        </p>
      )}
      {!loading && !error && visibleProjects.length === 0 && (
        <p className="status">No projects yet. Check back soon.</p>
      )}

      <div className="grid">
        {visibleProjects.map((project) => (
          <article className={`card project-card${project.isFeatured ? ' project-card-featured' : ''}`} key={project.slug || project.id}>
            {project.isFeatured && <span className="badge">Featured</span>}
            <div className="project-mark" aria-hidden="true">
              <span>{project.title.slice(0, 1)}</span>
              <small>CASE STUDY / 01</small>
            </div>
            <h3>{project.title}</h3>
            <p>{project.summary}</p>
            <ul className="tags">
              {project.techStack
                .split(',')
                .map((tech) => tech.trim())
                .filter(Boolean)
                .map((tech) => (
                  <li key={`${project.slug}-${tech}`}>{tech}</li>
                ))}
            </ul>
            <div className="project-links">
              <Link to={`/projects/${project.slug}`}>View details</Link>
              <a href={project.link || '#'}>Source</a>
            </div>
          </article>
        ))}
      </div>
    </>
  )
}