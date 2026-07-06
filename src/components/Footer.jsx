export default function Footer() {
  return (
    <footer className="flex flex-col items-center gap-3 border-t border-neutral-800 py-6 text-center text-sm text-neutral-500">
      <a
        href="https://www.linkedin.com/in/jared-kessler/"
        target="_blank"
        rel="noopener noreferrer"
        aria-label="LinkedIn"
        className="text-neutral-400 transition hover:text-neutral-100"
      >
        <svg viewBox="0 0 24 24" fill="currentColor" className="h-5 w-5">
          <path d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.446-2.136 2.941v5.665H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433a2.062 2.062 0 1 1 0-4.124 2.062 2.062 0 0 1 0 4.124zM7.114 20.452H3.558V9h3.556v11.452z" />
        </svg>
      </a>
      <p>&copy; {new Date().getFullYear()} Jared Kessler</p>
    </footer>
  )
}
