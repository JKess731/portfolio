import { Link } from 'react-router-dom'

const statusStyles = {
  Released: 'bg-green-500/10 text-green-400 border-green-500/30',
  'In Progress': 'bg-blue-500/10 text-blue-400 border-blue-500/30',
}

export default function ProjectCard({ project }) {
  const firstVideo = project.media?.find((m) => m.type === 'video')

  return (
    <Link
      to={`/projects/${project.slug}`}
      className="block rounded-lg border border-neutral-800 p-5 transition hover:border-purple-500/50"
    >
      {firstVideo ? (
        <video
          className="mb-3 aspect-video w-full rounded bg-neutral-900 object-cover"
          src={firstVideo.src}
          autoPlay
          muted
          loop
          playsInline
        />
      ) : (
        <div className="mb-3 aspect-video rounded bg-neutral-900" />
      )}
      <div className="mb-2 flex items-center justify-between">
        <h3 className="text-lg font-medium text-neutral-100">
          {project.title}
        </h3>
        <span
          className={`rounded-full border px-2 py-0.5 text-xs ${statusStyles[project.status]}`}
        >
          {project.status}
        </span>
      </div>
      <p className="mb-2 text-xs text-neutral-500">
        {project.timeframe}
        {project.releaseDate && ` • Released ${project.releaseDate}`}
      </p>
      <p className="text-sm text-neutral-400">{project.hook}</p>
      <ul className="mt-3 flex flex-wrap gap-1">
        {project.roles.map((role) => (
          <li
            key={role}
            className="rounded-full border border-neutral-800 px-2.5 py-1 text-[10.5px] text-neutral-400"
          >
            {role}
          </li>
        ))}
      </ul>
    </Link>
  )
}
