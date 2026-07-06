import { useRef, useState } from 'react'
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter'
import { vscDarkPlus } from 'react-syntax-highlighter/dist/esm/styles/prism'

export default function CodeSnippet({ language = 'csharp', title, code }) {
  const [expanded, setExpanded] = useState(false)
  const containerRef = useRef(null)
  const lineCount = code.split('\n').length

  const collapse = () => {
    setExpanded(false)
    const el = containerRef.current
    if (el) {
      const targetY =
        window.scrollY + el.getBoundingClientRect().top - window.innerHeight * 0.25
      window.scrollTo({ top: targetY, behavior: 'instant' })
    }
  }

  return (
    <div
      ref={containerRef}
      className="mt-3 overflow-hidden rounded-lg border border-neutral-500"
    >
      <div className="flex items-center justify-between gap-4 bg-neutral-800 px-4 py-2">
        {title && (
          <span className="font-mono text-xs text-neutral-300">{title}</span>
        )}
        <button
          type="button"
          onClick={() => setExpanded((v) => !v)}
          className="ml-auto shrink-0 text-xs text-neutral-300 transition hover:text-neutral-100"
        >
          {expanded ? 'Show less' : `Show full snippet (${lineCount} lines)`}
        </button>
      </div>
      {expanded && (
        <>
          <div className="overflow-x-auto">
            <SyntaxHighlighter
              language={language}
              style={vscDarkPlus}
              customStyle={{
                margin: 0,
                background: '#0a0a0a',
                fontSize: '0.8rem',
                padding: '1rem',
              }}
            >
              {code}
            </SyntaxHighlighter>
          </div>
          <button
            type="button"
            onClick={collapse}
            className="w-full border-t border-neutral-700 bg-neutral-800 py-2 text-xs text-neutral-300 transition hover:text-neutral-100"
          >
            Show less
          </button>
        </>
      )}
    </div>
  )
}
