import { projects } from '../data/projects'
import { ACCENTS } from '../data/accents'
import CodeSnippet from '../components/CodeSnippet'

export default function CodeSnippets() {
  return (
    <div className="mx-auto max-w-5xl px-6 py-16">
      <h1 className="text-3xl font-semibold text-neutral-100">Code Snippets</h1>
      <div className="mt-12 space-y-16">
        {projects.map((project) => {
          const systems = project.systems.filter((s) => s.snippets?.length)
          if (systems.length === 0) return null

          const accent = ACCENTS[project.accent] ?? ACCENTS.purple

          return (
            <section key={project.slug}>
              <h2 className={`text-2xl font-medium ${accent.text}`}>
                {project.title}
              </h2>
              <div className="mt-6 space-y-10">
                {systems.map((system) => (
                  <div key={system.name}>
                    <h3 className="text-lg font-medium text-neutral-100">
                      {system.name}
                    </h3>
                    {system.snippets.map((snippet) => (
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
          )
        })}
      </div>
    </div>
  )
}
