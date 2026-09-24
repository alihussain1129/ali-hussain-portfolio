import { FaGithub, FaLinkedinIn, FaEnvelope } from 'react-icons/fa'
import { site } from './site.js'

export const socialLinks = [
  { id: 'github', label: 'GitHub', href: site.social.github, icon: FaGithub },
  { id: 'linkedin', label: 'LinkedIn', href: site.social.linkedin, icon: FaLinkedinIn },
  {
    id: 'email',
    label: 'Email',
    href: site.email ? `mailto:${site.email}` : '',
    icon: FaEnvelope,
  },
]

export function isHttpUrl(value) {
  if (typeof value !== 'string') return false
  try {
    const u = new URL(value)
    return u.protocol === 'http:' || u.protocol === 'https:'
  } catch {
    return false
  }
}

export function linkProps(href) {
  return isHttpUrl(href) ? { target: '_blank', rel: 'noopener noreferrer' } : {}
}