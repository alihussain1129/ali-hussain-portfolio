import { useEffect, useState } from 'react'
import { site } from '../config/site.js'
import { apiUrl } from '../config/api.js'

const fallbackCourses = site.courses

function normalizeCourse(course) {
  return {
    ...course,
    slug: course.slug,
    level: course.level || course.category || 'Practical',
    duration: course.duration || 'Self-paced',
    lessons: course.lessons || course.resources?.length || 0,
  }
}

export function useCourses() {
  const [state, setState] = useState({ courses: fallbackCourses, loading: true, error: null })

  useEffect(() => {
    const controller = new AbortController()

    fetch(apiUrl('/api/courses'), { signal: controller.signal })
      .then((response) => {
        if (!response.ok) throw new Error(`Request failed (${response.status})`)
        return response.json()
      })
      .then((courses) => setState({ courses: courses.map(normalizeCourse), loading: false, error: null }))
      .catch((reason) => {
        if (reason.name !== 'AbortError') {
          setState({ courses: fallbackCourses, loading: false, error: reason.message })
        }
      })

    return () => controller.abort()
  }, [])

  return state
}
