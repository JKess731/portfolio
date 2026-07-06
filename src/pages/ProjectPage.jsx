import { useParams, Link } from 'react-router-dom'
import { getProjectBySlug } from '../data/projects'
import { ACCENTS } from '../data/accents'
import MediaSlideshow from '../components/MediaSlideshow'
import Divider from '../components/Divider'
import CodeSnippet from '../components/CodeSnippet'

function formatText(text) {
  return text.split(/\*\*(.+?)\*\*/g).map((part, i) =>
    i % 2 === 1 ? (
      <strong key={i} className="font-semibold text-neutral-100">
        {part}
      </strong>
    ) : (
      part
    )
  )
}

function PointList({ points, className }) {
  return (
    <ul className={className}>
      {points.map((point, i) => {
        const hasSubPoints = typeof point === 'object' && point.subPoints
        const text = typeof point === 'object' ? point.text : point
        return (
          <li key={i}>
            {formatText(text)}
            {hasSubPoints && (
              <PointList
                points={point.subPoints}
                className="mt-1 list-[circle] space-y-1 pl-5"
              />
            )}
          </li>
        )
      })}
    </ul>
  )
}

export default function ProjectPage() {
  const { slug } = useParams()
  const project = getProjectBySlug(slug)
  const accent = ACCENTS[project?.accent] ?? ACCENTS.purple

  if (!project) {
    return (
      <div className="mx-auto max-w-5xl px-6 py-16">
        <p className="text-neutral-400">Project not found.</p>
        <Link to="/" className="text-purple-400">
          Back home
        </Link>
      </div>
    )
  }

  return (
    <div className="mx-auto max-w-5xl px-6 py-16">
      <Link to="/" className="text-sm text-neutral-500 hover:text-neutral-300">
        &larr; Back
      </Link>

      <header className="mt-4 text-center">
        <h1 className="text-7xl font-semibold text-neutral-100">
          {project.title}
        </h1>
        <Divider className="mt-6" color={accent.divider} />
        <div className="mx-auto mt-8 flex flex-col gap-6 text-left sm:flex-row">
          <dl className="flex shrink-0 flex-col gap-3 text-base text-neutral-500 sm:w-52">
            <div>
              <dt className={`inline ${accent.text}`}>
                {project.roles.length > 1 ? 'Roles' : 'Role'}:{' '}
              </dt>
              <dd className="inline text-neutral-100">
                {project.roles.join(', ')}
              </dd>
            </div>
            <div>
              <dt className={`inline ${accent.text}`}>Timeframe: </dt>
              <dd className="inline text-neutral-100">{project.timeframe}</dd>
            </div>
            {project.releaseDate && (
              <div>
                <dt className={`inline ${accent.text}`}>Released: </dt>
                <dd className="inline text-neutral-100">
                  {project.releaseDate}
                </dd>
              </div>
            )}
          </dl>
          <p className="flex-1 text-white">{project.hook}</p>
        </div>
        {project.externalLink && (
          <a
            href={project.externalLink}
            target="_blank"
            rel="noopener noreferrer"
            className={`mt-6 inline-block rounded-full border px-4 py-1.5 text-sm transition ${accent.pill}`}
          >
            View on Steam
          </a>
        )}
      </header>

      <MediaSlideshow media={project.media} dotColor={accent.dot} />

      <section className="mt-16">
        <h2 className="text-center text-4xl font-medium text-neutral-100">
          Systems breakdown
        </h2>
        <Divider className="mt-4" color={accent.divider} />
        <div className="mt-4 space-y-6">
          {project.systems.map((system, i) => (
            <div key={system.name}>
              {i > 0 && (
                <Divider className="mb-6" color={accent.divider} width="w-32" />
              )}
              <h3 className={`text-center text-2xl font-medium ${accent.text}`}>
                {system.name}
              </h3>
              {system.media && (
                system.media.type === 'video' ? (
                  <video
                    src={system.media.src}
                    className={`mt-4 rounded-lg ${system.mediaWidth ?? 'w-full'}`}
                    autoPlay
                    muted
                    loop
                    playsInline
                  />
                ) : (
                  <img
                    src={system.media.src}
                    alt={`${system.name} demo`}
                    className={`mt-4 rounded-lg ${system.mediaWidth ?? 'w-full'}`}
                  />
                )
              )}
              <PointList
                points={system.points}
                className="mt-4 list-disc space-y-1 pl-5 text-left text-white"
              />
              {system.gallery && (
                <div className="mt-6 grid grid-cols-[1.3fr_1fr_1fr] grid-rows-2 gap-x-4 gap-y-2">
                  <img
                    src={system.gallery[0].src}
                    alt={`${system.name} detail 1`}
                    className="col-start-1 row-span-2 row-start-1 h-full w-full object-contain"
                  />
                  <img
                    src={system.gallery[1].src}
                    alt={`${system.name} detail 2`}
                    className="col-start-2 row-start-1 h-full w-full object-contain"
                  />
                  <img
                    src={system.gallery[2].src}
                    alt={`${system.name} detail 3`}
                    className="col-start-2 row-start-2 h-full w-full object-contain"
                  />
                  <img
                    src={system.gallery[3].src}
                    alt={`${system.name} detail 4`}
                    className="col-start-3 row-span-2 row-start-1 h-full w-full object-contain"
                  />
                </div>
              )}
              {system.why && (
                <p className="mt-4 text-left text-sm text-neutral-400 italic">
                  {formatText(system.why)}
                </p>
              )}
              {system.snippets?.map((snippet) => (
                <CodeSnippet
                  key={snippet.title}
                  language={snippet.language}
                  title={snippet.title}
                  code={snippet.code}
                />
              ))}
            </div>
          ))}
        </div>
      </section>

      {project.challenges?.length > 0 && (
        <section className="mt-16">
          <h2 className="text-center text-4xl font-medium text-neutral-100">
            Challenges & retrospective
          </h2>
          <Divider className="mt-4" color={accent.divider} />
          <div className="mt-4 space-y-4">
            {project.challenges.map((paragraph, i) => (
              <p key={i} className="text-left text-white">
                {formatText(paragraph)}
              </p>
            ))}
          </div>
        </section>
      )}

      {project.pitchDeck?.length > 0 && (
        <section className="mt-16">
          <h2 className="text-center text-4xl font-medium text-neutral-100">
            Pitch Deck
          </h2>
          <Divider className="mt-4" color={accent.divider} />
          <MediaSlideshow
            media={project.pitchDeck}
            dotColor={accent.dot}
            autoAdvance={false}
          />
        </section>
      )}
    </div>
  )
}
