import ProjectLinks from './ProjectLinks.jsx'

export default function ProjectCard({ project }) {
  const tags = (project.techStack || '')
    .split(',')
    .map((t) => t.trim())
    .filter(Boolean)

  return (
    <article className="card project-card">
      {project.isFeatured && <span className="badge">Featured</span>}
      <h3>{project.title}</h3>
      <p>{project.summary}</p>
      {tags.length > 0 && (
        <ul className="tags">
          {tags.map((t) => (
            <li key={t}>{t}</li>
          ))}
        </ul>
      )}
      <ProjectLinks project={project} />
    </article>
  )
}