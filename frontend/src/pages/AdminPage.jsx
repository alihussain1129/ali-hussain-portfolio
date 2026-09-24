import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { apiUrl } from '../config/api.js'

const initialCourse = { title: '', description: '', category: 'C#', author: 'Ali Hussain', isPublished: true }

async function readResponse(response, fallback) {
  const result = await response.json().catch(() => ({}))
  if (!response.ok) throw new Error(result.message || fallback)
  return result
}

export default function AdminPage() {
  const [token, setToken] = useState(() => localStorage.getItem('portfolio_admin_token') || '')
  const [user, setUser] = useState(() => JSON.parse(localStorage.getItem('portfolio_admin_user') || 'null'))
  const [courses, setCourses] = useState([])
  const [messages, setMessages] = useState([])
  const [stats, setStats] = useState(null)
  const [course, setCourse] = useState(initialCourse)
  const [upload, setUpload] = useState({ title: '', description: '', courseId: '', chapterNumber: 1, lessonNumber: 1, file: null })
  const [message, setMessage] = useState('')
  const [error, setError] = useState('')
  const [busy, setBusy] = useState(false)

  useEffect(() => {
    if (!token) return
    const headers = { Authorization: `Bearer ${token}` }
    Promise.all([
      fetch(apiUrl('/api/courses'), { headers }),
      fetch(apiUrl('/api/contact'), { headers }),
      fetch(apiUrl('/api/admin/dashboard'), { headers }),
    ])
      .then(async ([coursesResponse, messagesResponse, dashboardResponse]) => {
        if (!coursesResponse.ok || !messagesResponse.ok || !dashboardResponse.ok) {
          throw new Error('Could not load admin data. Please sign in again.')
        }
        return Promise.all([coursesResponse.json(), messagesResponse.json(), dashboardResponse.json()])
      })
      .then(([courseResult, messageResult, dashboardResult]) => {
        setCourses(courseResult)
        setMessages(messageResult)
        setStats(dashboardResult)
      })
      .catch((reason) => setError(reason.message))
  }, [token])

  const login = async (event) => {
    event.preventDefault()
    setBusy(true)
    setError('')
    const form = new FormData(event.currentTarget)
    try {
      const response = await fetch(apiUrl('/api/auth/login'), {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email: form.get('email'), password: form.get('password') }),
      })
      const result = await readResponse(response, 'Login failed.')
      localStorage.setItem('portfolio_admin_token', result.token)
      localStorage.setItem('portfolio_admin_user', JSON.stringify(result.user))
      setToken(result.token)
      setUser(result.user)
    } catch (reason) {
      setError(reason.message)
    } finally {
      setBusy(false)
    }
  }

  const createCourse = async (event) => {
    event.preventDefault()
    setBusy(true)
    setMessage('')
    setError('')
    try {
      const response = await fetch(apiUrl('/api/courses'), {
        method: 'POST',
        headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' },
        body: JSON.stringify(course),
      })
      const result = await readResponse(response, 'Course creation failed.')
      setCourses((current) => [...current, result])
      setUpload((current) => ({ ...current, courseId: String(result.id) }))
      setCourse(initialCourse)
      setMessage(`Course "${result.title}" created. You can upload a PDF for it now.`)
    } catch (reason) {
      setError(reason.message)
    } finally {
      setBusy(false)
    }
  }

  const uploadPdf = async (event) => {
    event.preventDefault()
    if (!upload.file) {
      setError('Choose a PDF file before uploading.')
      return
    }
    setBusy(true)
    setMessage('')
    setError('')
    const form = new FormData()
    Object.entries(upload).forEach(([key, value]) => {
      if (key !== 'file' && value !== null) form.append(key, String(value))
    })
    form.append('File', upload.file)
    try {
      const response = await fetch(apiUrl('/api/resources/upload'), {
        method: 'POST',
        headers: { Authorization: `Bearer ${token}` },
        body: form,
      })
      const result = await readResponse(response, 'PDF upload failed.')
      setUpload({ title: '', description: '', courseId: upload.courseId, chapterNumber: 1, lessonNumber: 1, file: null })
      event.currentTarget.reset()
      setMessage(`"${result.title}" uploaded successfully.`)
    } catch (reason) {
      setError(reason.message)
    } finally {
      setBusy(false)
    }
  }

  const markMessageRead = async (id) => {
    const response = await fetch(apiUrl(`/api/contact/${id}/read`), { method: 'PATCH', headers: { Authorization: `Bearer ${token}` } })
    if (!response.ok) {
      setError('Could not update that message.')
      return
    }
    const updated = await response.json()
    setMessages((current) => current.map((item) => (item.id === updated.id ? updated : item)))
  }

  const deleteMessage = async (id) => {
    if (!window.confirm('Delete this contact message?')) return
    const response = await fetch(apiUrl(`/api/contact/${id}`), { method: 'DELETE', headers: { Authorization: `Bearer ${token}` } })
    if (!response.ok) {
      setError('Could not delete that message.')
      return
    }
    setMessages((current) => current.filter((item) => item.id !== id))
  }

  const logout = () => {
    localStorage.removeItem('portfolio_admin_token')
    localStorage.removeItem('portfolio_admin_user')
    setToken('')
    setUser(null)
  }

  if (!token) {
    return (
      <div className="page-container"><section className="section"><div className="container login-shell">
        <form className="login-card contact-form" onSubmit={login}>
          <span className="eyebrow eyebrow-dark">Private area</span><h1>Admin login</h1>
          <p>Manage courses and upload learning documents securely.</p>
          <label>Email<input name="email" type="email" required placeholder="admin@alihussain.dev" /></label>
          <label>Password<input name="password" type="password" required /></label>
          {error && <p className="status status-error" role="alert">{error}</p>}
          <button className="btn btn-primary btn-block" disabled={busy}>{busy ? 'Signing in…' : 'Sign in'}</button>
          <Link className="inline-link" to="/">Back to portfolio</Link>
        </form>
      </div></section></div>
    )
  }

  return (
    <div className="page-container admin-page"><div className="container">
      <div className="admin-heading"><div><span className="eyebrow eyebrow-dark">Admin workspace</span><h1>Content management</h1><p>Signed in as {user?.email || 'administrator'}.</p></div><button className="btn btn-ghost-dark" onClick={logout}>Sign out</button></div>
      {message && <p className="status status-success" role="status">{message}</p>}
      {error && <p className="status status-error" role="alert">{error}</p>}
      {stats && <div className="admin-stats" aria-label="Dashboard summary">
        <div><strong>{stats.totalProjects}</strong><span>Projects</span></div>
        <div><strong>{stats.totalCourses}</strong><span>Courses</span></div>
        <div><strong>{stats.totalResources}</strong><span>Resources</span></div>
        <div><strong>{stats.unreadMessages}</strong><span>Unread messages</span></div>
      </div>}
      <div className="admin-grid">
        <form className="content-card contact-form" onSubmit={createCourse}>
          <div className="admin-card-heading"><span>01</span><h2>Create a course</h2></div>
          <label>Course title<input value={course.title} onChange={(event) => setCourse({ ...course, title: event.target.value })} required /></label>
          <label>Description<textarea rows="4" value={course.description} onChange={(event) => setCourse({ ...course, description: event.target.value })} required /></label>
          <label>Category<input value={course.category} onChange={(event) => setCourse({ ...course, category: event.target.value })} required /></label>
          <button className="btn btn-primary" disabled={busy}>{busy ? 'Saving…' : 'Create course'}</button>
        </form>
        <form className="content-card contact-form" onSubmit={uploadPdf}>
          <div className="admin-card-heading"><span>02</span><h2>Upload a PDF</h2></div>
          <label>Course<select value={upload.courseId} onChange={(event) => setUpload({ ...upload, courseId: event.target.value })} required><option value="">Select a course</option>{courses.map((item) => <option value={item.id} key={item.id}>{item.title}</option>)}</select></label>
          <label>Document title<input value={upload.title} onChange={(event) => setUpload({ ...upload, title: event.target.value })} required /></label>
          <label>Description<textarea rows="3" value={upload.description} onChange={(event) => setUpload({ ...upload, description: event.target.value })} /></label>
          <div className="form-row"><label>Chapter<input type="number" min="1" value={upload.chapterNumber} onChange={(event) => setUpload({ ...upload, chapterNumber: event.target.value })} /></label><label>Lesson<input type="number" min="1" value={upload.lessonNumber} onChange={(event) => setUpload({ ...upload, lessonNumber: event.target.value })} /></label></div>
          <label>PDF file<input type="file" accept="application/pdf,.pdf" onChange={(event) => setUpload({ ...upload, file: event.target.files?.[0] || null })} required /></label>
          <button className="btn btn-primary" disabled={busy}>{busy ? 'Uploading…' : 'Upload PDF'}</button><small className="form-hint">PDF only, maximum 25 MB.</small>
        </form>
      </div>
      <section className="content-card admin-messages">
        <div className="admin-card-heading"><span>03</span><h2>Contact messages</h2></div>
        {messages.length === 0 && <p className="status">No contact messages yet.</p>}
        {messages.map((item) => <article className={`admin-message${item.isRead ? '' : ' unread'}`} key={item.id}>
          <div><strong>{item.name}</strong><span>{item.email}</span><small>{new Date(item.createdAt).toLocaleString()}</small></div>
          <p>{item.message}</p>
          <div className="admin-message-actions">
            {!item.isRead && <button type="button" className="btn btn-ghost-dark" onClick={() => markMessageRead(item.id)}>Mark read</button>}
            <button type="button" className="btn btn-danger" onClick={() => deleteMessage(item.id)}>Delete</button>
          </div>
        </article>)}
      </section>
    </div></div>
  )
}
