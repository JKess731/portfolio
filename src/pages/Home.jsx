import { projects } from '../data/projects'
import ProjectCard from '../components/ProjectCard'

const stack = ['Unity', 'C#/.NET', 'JavaScript', 'Java']

export default function Home() {
  return (
    <div className="mx-auto max-w-5xl px-6 py-16">
      <h1 className="text-4xl font-semibold text-neutral-100">
        Jared Kessler
      </h1>
      <p className="mt-3 max-w-xl text-neutral-400">
        Game developer & software engineer.
      </p>
      <ul className="mt-6 flex flex-wrap gap-2">
        {stack.map((tech) => (
          <li
            key={tech}
            className="rounded-full border border-neutral-800 px-3 py-1 text-xs text-neutral-400"
          >
            {tech}
          </li>
        ))}
      </ul>

      <h2 className="mt-14 mb-4 text-xl font-medium text-neutral-100">
        Projects
      </h2>
      <div className="grid gap-5 sm:grid-cols-2">
        {projects.map((project) => (
          <ProjectCard key={project.slug} project={project} />
        ))}
      </div>
    </div>
  )
}
