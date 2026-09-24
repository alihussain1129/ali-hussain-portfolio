import { socialLinks, linkProps } from '../config/links.js'

export default function SocialLinks({ variant = 'icons' }) {
  return (
    <ul className={`social social-${variant}`}>
      {socialLinks
        .filter((l) => l.href)
        .map(({ id, label, href, icon: Icon }) => (
          <li key={id}>
            <a href={href} aria-label={variant === 'icons' ? label : undefined} {...linkProps(href)}>
              <Icon aria-hidden="true" />
              {variant === 'buttons' && <span>{label}</span>}
            </a>
          </li>
        ))}
    </ul>
  )
}