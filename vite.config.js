import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import fs from 'node:fs'
import path from 'node:path'

// Dev-only stand-in for the contact form's backend. Appends submissions to a
// local text file instead of sending an email — swap for a real serverless
// function (e.g. a Vercel function using Resend) once the site is deployed.
function contactFormDevPlugin() {
  return {
    name: 'contact-form-dev',
    configureServer(server) {
      server.middlewares.use('/api/contact', (req, res) => {
        if (req.method !== 'POST') {
          res.statusCode = 405
          res.end('Method Not Allowed')
          return
        }

        let body = ''
        req.on('data', (chunk) => {
          body += chunk
        })
        req.on('end', () => {
          res.setHeader('Content-Type', 'application/json')
          try {
            const { name, email, message } = JSON.parse(body)
            const oneLine = (s) => String(s ?? '').replace(/[\r\n]+/g, ' ').trim()
            const entry = `--- ${new Date().toISOString()} ---\nName: ${oneLine(name)}\nEmail: ${oneLine(email)}\nMessage: ${String(message ?? '').trim()}\n\n`
            fs.appendFileSync(
              path.resolve(process.cwd(), 'contact-submissions.txt'),
              entry
            )
            res.statusCode = 200
            res.end(JSON.stringify({ ok: true }))
          } catch {
            res.statusCode = 400
            res.end(JSON.stringify({ ok: false, error: 'Invalid request' }))
          }
        })
      })
    },
  }
}

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss(), contactFormDevPlugin()],
})
