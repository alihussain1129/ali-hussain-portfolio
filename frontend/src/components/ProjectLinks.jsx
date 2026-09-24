import { FaGithub, FaGlobe, FaBookOpen } from 'react-icons/fa'
import { isHttpUrl, linkProps } from '../config/links.js'

const definitions = [
  ['githubUrl', 'Code', FaGithub],
  ['liveDemoUrl', 'Live demo', FaGlobe],
  ['caseStudyUrl', 'Case study', FaBookOpen],
]

export default function ProjectLinks({ project }) {
  const items = definitions.filter(([key]) => isHttpUrl(project[key]))
  if (items.length === 0) return null

  return (
    <ul className="project-links">
      {items.map(([key, label, Icon]) => (
        <li key={key}>
          <a href={project[key]} {...linkProps(project[key])}>
            <Icon aria-hidden="true" />
            <span>{label}</span>
          </a>
        </li>
      ))}
    </ul>
  )
}