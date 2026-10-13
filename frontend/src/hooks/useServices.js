import { useEffect, useState } from 'react'
import { site } from '../config/site.js'
import { apiUrl } from '../config/api.js'

const fallbackServices = site.services
const fallbackServicesByTitle = new Map(fallbackServices.map((service) => [service.title, service]))

function normalizeService(service) {
  const fallback = fallbackServicesByTitle.get(service.title)
  return {
    ...fallback,
    ...service,
    points: fallback?.points || [],
  }
}

export function useServices() {
  const [state, setState] = useState({ services: fallbackServices, error: null })

  useEffect(() => {
    const controller = new AbortController()

    fetch(apiUrl('/api/services'), { signal: controller.signal })
      .then((response) => {
        if (!response.ok) throw new Error(`Request failed (${response.status})`)
        return response.json()
      })
      .then((services) => {
        if (!Array.isArray(services)) throw new Error('The services response was invalid.')
        setState({
          services: services.length ? services.map(normalizeService) : fallbackServices,
          error: null,
        })
      })
      .catch((reason) => {
        if (reason.name !== 'AbortError') {
          setState({ services: fallbackServices, error: reason.message })
        }
      })

    return () => controller.abort()
  }, [])

  return state
}
