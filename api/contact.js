import { Resend } from 'resend'

const resend = new Resend(process.env.EMAIL_RESEND_API_KEY)

export default async function handler(req, res) {
  if (req.method !== 'POST') {
    res.status(405).json({ ok: false, error: 'Method not allowed' })
    return
  }

  const { name, email, message } = req.body ?? {}

  if (!name || !email || !message) {
    res.status(400).json({ ok: false, error: 'Missing required fields' })
    return
  }

  try {
    await resend.emails.send({
      from: `Portfolio Contact Form <contact@${process.env.EMAIL_RESEND_EMAIL_DOMAIN}>`,
      to: 'jkessler731@gmail.com',
      replyTo: email,
      subject: `New portfolio message from ${name}`,
      text: `From: ${name} <${email}>\n\n${message}`,
    })
    res.status(200).json({ ok: true })
  } catch (err) {
    console.error('Failed to send contact email', err)
    res.status(500).json({ ok: false, error: 'Failed to send email' })
  }
}
