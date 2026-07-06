import { NavLink, useLocation } from 'react-router-dom'
import { projects } from '../data/projects'

const links = [
  { to: '/about', label: 'About' },
  { to: '/about#contact', label: 'Contact' },
]

const ACCENT_HOVER = {
  purple: 'hover:text-purple-400',
  green: 'hover:text-green-400',
  red: 'hover:text-red-400',
}

export default function Nav() {
  const { pathname, hash } = useLocation()
  const currentProject = projects.find((p) => pathname === `/projects/${p.slug}`)
  const currentAccentHover = ACCENT_HOVER[currentProject?.accent] ?? ACCENT_HOVER.purple

  return (
    <header className="border-b border-neutral-800">
      <nav className="mx-auto flex max-w-7xl items-center justify-between px-6 py-4">
        <NavLink to="/" className="font-mono text-lg text-neutral-100">
          Jared Kessler
        </NavLink>
        <ul className="flex divide-x divide-neutral-800">
          <li className="group relative pr-6">
            <NavLink
              to="/"
              end
              className={({ isActive }) =>
                isActive
                  ? 'text-purple-400'
                  : 'text-neutral-400 hover:text-neutral-100'
              }
            >
              Home
            </NavLink>
            <div className="invisible absolute -left-4 top-full pt-[18px] opacity-0 transition group-hover:visible group-hover:opacity-100">
              <ul className="w-48 divide-y divide-neutral-800 border border-neutral-800 bg-neutral-950 py-2">
                {projects.map((project) => (
                  <li key={project.slug}>
                    <NavLink
                      to={`/projects/${project.slug}`}
                      className={`block px-4 py-2 text-sm text-neutral-400 hover:bg-neutral-800 ${currentAccentHover}`}
                    >
                      {project.title}
                    </NavLink>
                  </li>
                ))}
              </ul>
            </div>
          </li>
          {links.map(({ to, label }) => {
            const [linkPath, linkHash] = to.split('#')
            const isActive =
              pathname === linkPath && hash === (linkHash ? `#${linkHash}` : '')
            return (
              <li key={to} className="px-6">
                <NavLink
                  to={to}
                  className={
                    isActive
                      ? 'text-purple-400'
                      : 'text-neutral-400 hover:text-neutral-100'
                  }
                >
                  {label}
                </NavLink>
              </li>
            )
          })}
        </ul>
      </nav>
    </header>
  )
}
