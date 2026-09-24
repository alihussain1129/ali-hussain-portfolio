import { site } from '../config/site.js'
import SocialLinks from '../components/SocialLinks.jsx'

export default function Footer() {
  return (
    <footer className="site-footer">
      <div className="container footer-inner">
        <p>
          © {new Date().getFullYear()} {site.name}
        </p>
        <SocialLinks variant="icons" />
      </div>
    </footer>
  )
}