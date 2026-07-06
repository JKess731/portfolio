import { useState } from 'react'

export default function ContactForm() {
  const [form, setForm] = useState({ name: '', email: '', message: '' })
  const [status, setStatus] = useState('idle') // idle | sending | sent | error

  const handleChange = (e) => {
    setForm((f) => ({ ...f, [e.target.name]: e.target.value }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setStatus('sending')
    try {
      const res = await fetch('/api/contact', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(form),
      })
      if (!res.ok) throw new Error('Request failed')
      setStatus('sent')
      setForm({ name: '', email: '', message: '' })
    } catch {
      setStatus('error')
    }
  }

  const inputClass =
    'w-full rounded-lg border border-neutral-800 bg-neutral-900 px-4 py-2 text-neutral-100 placeholder:text-neutral-500 focus:border-purple-500/50 focus:outline-none'

  return (
    <form onSubmit={handleSubmit} className="flex w-full flex-col gap-4">
      <input
        type="text"
        name="name"
        placeholder="Name"
        required
        value={form.name}
        onChange={handleChange}
        className={inputClass}
      />
      <input
        type="email"
        name="email"
        placeholder="Email"
        required
        value={form.email}
        onChange={handleChange}
        className={inputClass}
      />
      <textarea
        name="message"
        placeholder="Message"
        required
        rows={7}
        value={form.message}
        onChange={handleChange}
        className={`${inputClass} min-h-56 resize-y`}
      />
      <button
        type="submit"
        disabled={status === 'sending'}
        className="self-start rounded-full border border-purple-500/50 px-4 py-1.5 text-sm text-purple-400 transition hover:bg-purple-500/10 disabled:opacity-50"
      >
        {status === 'sending' ? 'Sending...' : 'Send'}
      </button>

      {status === 'sent' && (
        <p className="text-sm text-green-400">Thanks — message sent!</p>
      )}
      {status === 'error' && (
        <p className="text-sm text-red-400">
          Something went wrong. Please try again.
        </p>
      )}
    </form>
  )
}
