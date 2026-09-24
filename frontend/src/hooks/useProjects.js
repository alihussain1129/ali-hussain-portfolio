import { useEffect, useState } from 'react'
import { site } from '../config/site.js'
import { apiUrl } from '../config/api.js'

const fallbackProjects = site.projects

function normalizeProject(project) {
  return {
    ...project,
    summary: project.summary || project.shortDescription || '',
    techStack: project.techStack || (project.technologies || []).join(', '),
    link: project.link || project.githubUrl || '',
    liveUrl: project.liveUrl || project.liveDemoUrl || '',
  }
}

export function useProjects() {
  const [state, setState] = useState({ projects: fallbackProjects, loading: true, error: null })

  useEffect(() => {
    const controller = new AbortController()

    fetch(apiUrl('/api/projects'), { signal: controller.signal })
      .then((res) => {
        if (!res.ok) throw new Error(`Request failed (${res.status})`)
        return res.json()
      })
      .then((projects) => setState({ projects: projects.map(normalizeProject), loading: false, error: null }))
      .catch((err) => {
        if (err.name !== 'AbortError') {
          setState({ projects: fallbackProjects, loading: false, error: err.message })
        }
      })

    return () => controller.abort()
  }, [])

  return state
}