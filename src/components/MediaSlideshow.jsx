import { useEffect, useRef, useState } from 'react'

const IMAGE_DURATION_MS = 4000

const isGif = (item) => item.type === 'image' && item.src.toLowerCase().endsWith('.gif')

export default function MediaSlideshow({ media = [], dotColor = 'bg-purple-400', autoAdvance = true }) {
  const [index, setIndex] = useState(0)
  const [paused, setPaused] = useState(false)
  const timerRef = useRef(null)

  const goNext = () => setIndex((prev) => (prev + 1) % media.length)
  const goPrev = () => setIndex((prev) => (prev - 1 + media.length) % media.length)

  useEffect(() => {
    clearTimeout(timerRef.current)
    if (media.length <= 1 || !autoAdvance || paused) return undefined

    if (media[index].type === 'image' && !isGif(media[index])) {
      timerRef.current = setTimeout(goNext, IMAGE_DURATION_MS)
    }
    return () => clearTimeout(timerRef.current)
  }, [index, media, autoAdvance, paused])

  if (media.length === 0) {
    return <div className="mt-8 aspect-video rounded-lg bg-neutral-900" />
  }

  const current = media[index]
  const hasMultiple = media.length > 1

  return (
    <div className="mt-8">
      <div className="flex items-center gap-4">
        {hasMultiple && (
          <button
            type="button"
            onClick={goPrev}
            aria-label="Previous slide"
            className="shrink-0 p-2.5 text-white transition active:scale-90 hover:text-neutral-400"
          >
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" className="h-8 w-8">
              <path strokeLinecap="round" strokeLinejoin="round" d="M15 18l-6-6 6-6" />
            </svg>
          </button>
        )}

        <div className="relative aspect-video flex-1 overflow-hidden rounded-lg bg-neutral-900">
          {current.type === 'video' ? (
            <video
              key={current.src}
              className="h-full w-full object-cover"
              src={current.src}
              autoPlay
              muted
              playsInline
              loop={media.length === 1 || paused}
              onEnded={() => autoAdvance && !paused && goNext()}
            />
          ) : (
            <img
              key={current.src}
              className="h-full w-full object-cover"
              src={current.src}
              alt=""
            />
          )}

          {current.overlays?.map((overlay) => (
            <img
              key={overlay.src}
              src={overlay.src}
              alt=""
              className="absolute object-cover"
              style={{
                left: overlay.left,
                top: overlay.top,
                width: overlay.width,
                height: overlay.height,
              }}
            />
          ))}
        </div>

        {hasMultiple && (
          <button
            type="button"
            onClick={goNext}
            aria-label="Skip to next slide"
            className="shrink-0 p-2.5 text-white transition active:scale-90 hover:text-neutral-400"
          >
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" className="h-8 w-8">
              <path strokeLinecap="round" strokeLinejoin="round" d="M9 18l6-6-6-6" />
            </svg>
          </button>
        )}
      </div>

      <div className="mt-3 flex items-center justify-center gap-3">
        {hasMultiple && (
          <div className="group relative">
            <button
              type="button"
              onClick={() => setPaused((p) => !p)}
              aria-label={paused ? 'Play' : 'Pause auto-advance'}
              className="flex shrink-0 items-center justify-center rounded-full p-1.5 text-white transition active:scale-90 hover:text-neutral-400"
            >
              {paused ? (
                <svg viewBox="0 0 24 24" fill="currentColor" className="h-5 w-5">
                  <path d="M8 5v14l11-7z" />
                </svg>
              ) : (
                <svg viewBox="0 0 24 24" fill="currentColor" className="h-5 w-5">
                  <rect x="6" y="5" width="4" height="14" rx="1" />
                  <rect x="14" y="5" width="4" height="14" rx="1" />
                </svg>
              )}
            </button>
            <span className="pointer-events-none absolute bottom-full left-1/2 mb-2 -translate-x-1/2 whitespace-nowrap rounded bg-neutral-800 px-2 py-1 text-xs text-neutral-300 opacity-0 transition group-hover:opacity-100">
              {paused ? 'Resume auto-advance' : 'Pause auto-advance'}
            </span>
          </div>
        )}

        {hasMultiple && (
          <div className="flex gap-2">
            {media.map((_, i) => (
              <button
                key={i}
                type="button"
                onClick={() => setIndex(i)}
                aria-label={`Go to slide ${i + 1}`}
                className={`h-1.5 w-1.5 rounded-full transition ${
                  i === index ? dotColor : 'bg-white/30'
                }`}
              />
            ))}
          </div>
        )}
      </div>

      {current.title && (
        <div className="mt-4 text-center">
          <h3 className="text-lg font-medium text-neutral-100">{current.title}</h3>
          {current.notes && (
            <p className="mt-1 text-sm text-neutral-400">{current.notes}</p>
          )}
        </div>
      )}
    </div>
  )
}
