# Jared Kessler — Portfolio Site Plan

## Goal
Replace the Wix site with a custom-built React portfolio aimed at junior game/software developer roles. The site itself should function as a work sample — clean code, thoughtful UX, dark-mode/code-forward aesthetic. It should be a clear step up from the current Wix site, not a reskin of it.

## Key Requirements
- **Minimal-click navigation** — a visitor should be able to reach any project's gifs, writeup, and code from the home page in 1–2 clicks max. No burying content behind multiple nav layers like the current site does with its separate Projects/Prototypes/Code Snippets split.
- **No standalone Code Snippets page** — snippets live inline on each project's case study page, next to the system they belong to, not siloed elsewhere.
- **Fully responsive** — needs to look and work well on mobile as well as desktop. Recruiters may click through on their phone. This affects layout (single-column stacking on mobile), gif/video sizing, and especially the code snippet blocks (horizontal scroll instead of wrapping/breaking on small screens).
- **Build handoff:** actual implementation will happen later via Claude Code in the terminal on your PC. This document is the spec/plan Claude Code will work from — keep it detailed enough to hand off directly.

## Tech Stack
- **React 19 + Vite** (matches what you already know from the portfolio-site learning track)
- **React Router 7** for page navigation (Home, Project pages, About, Contact)
- **Tailwind CSS v4** via the `@tailwindcss/vite` plugin (CSS-first config, no separate `tailwind.config.js`/PostCSS setup needed) — fast to theme dark mode consistently, and its responsive utilities (`sm:`/`md:`/`lg:` breakpoints) make mobile-first layout straightforward
- **Fonts:** Inter + JetBrains Mono, self-hosted via `@fontsource`/`@fontsource/jetbrains-mono` (no external font requests). Tailwind's `--font-sans`/`--font-mono` theme tokens are overridden in `index.css` to point at these. Currently the whole site defaults to JetBrains Mono (`font-mono` on `body`) for a consistent code-forward look across titles and body text; Inter is installed and available via `font-sans` if we want to split treatments later.
- **Syntax highlighting:** `react-syntax-highlighter` or `shiki` for code snippet blocks, with collapsible `<details>`-style expand/collapse for full listings — **not yet implemented**
- **Deployment:** Vercel, connected to a GitHub repo (also gives you a public repo showing clean commit history — good signal on its own). **Holding off on deploying until the site is ready to go public** — developing locally via `npm run dev` for now.
- **Media:** GIFs/images optimized and stored in `/public` or a CDN (Vercel handles static assets fine at this scale). All "gif" clips should actually be implemented as `<video autoplay loop muted playsinline>` in `.webm` (with `.mp4` fallback) — this looks and behaves exactly like a gif (plays automatically on load/scroll into view, no click, no visible controls) but at roughly 10-20% of the file size. No user click should ever be required to see gameplay motion. Implemented as a `MediaSlideshow` component (see Progress Log) — note videos currently do **not** loop individually; they play once and auto-advance to the next slide instead.

## Site Structure
Flat nav, everything reachable in 1–2 clicks: **Home | Projects (or straight to individual project links) | About | Contact**. No separate Prototypes or Code Snippets pages — fold prototypes into project pages or a small "other work" strip on Home if worth keeping, and fold snippets into each project page (see below).

1. **Home** — short intro, tech stack badges, project cards that link straight into each case study (gif thumbnail + one-line hook + a status tag — **"In Progress"** or **"Released"** — not just a title), contact/resume/LinkedIn. Only these two tags exist; no "Completed" or similar, since a shipped game still gets updates/patches and is never really "done." Slime Guy is on Steam and shipped, so it gets **"Released."** This page should double as the project index — no extra "Projects" landing page in between if it can be avoided.
2. **Project Case Study pages** (one per game, this is the core of the site):
   - **Header block** (top of page, above the fold): project name, elevator pitch (1-2 sentence description), your role(s) on the project, timeframe (start date – release date, or start date – "In Progress"), and a link to the Steam/itch.io page if the project has one
   - Hero gif/video of gameplay (auto-advancing slideshow, see Progress Log)
   - Overview: tools/tech used, scope of the project, and any context the header didn't cover
   - **Systems breakdown with embedded code snippets, inline** — short highlighted excerpt + expandable full block, placed directly next to the system it implements. This replaces the old standalone Code Snippets page entirely.
     - **Revised:** there is no standalone "Design decisions" section anymore. Each system's *why* (the design-decision rationale) is written directly underneath that system's *how it works* breakdown, so a reader gets the mechanism and the reasoning back-to-back instead of cross-referencing two separate sections. A lightweight project-wide "Design decisions" section (or folding project-wide calls into Overview) is still on the table for decisions that aren't tied to one specific system (e.g. "why Unity," "why roguelite genre") — revisit if that becomes needed.
   - Screenshots/gifs interspersed near the relevant text, not dumped in a gallery
   - Challenges/what you'd do differently (shows self-awareness, reads well to hiring managers)
3. **About** — background, currently targeting junior dev roles, brief personal note
4. **Contact** — email, LinkedIn, resume download

## Content Needed From You (per project)
For **Slime Guy** first:
- [x] Role(s): Lead Producer & Gameplay Programmer
- [x] Timeframe: 2023 – 2025; Released December 18, 2025; Steam link added
- [x] One-line hook / elevator pitch
- [x] First system written up: **Procedural Room Generation** (semi-procedural, Wave Function Collapse-inspired room placement) — how-it-works bullets + why-I-built-it-this-way rationale
- [ ] 3–6 gifs/short clips of key gameplay moments (combat, progression, a bug-turned-feature if any) — needed to populate `media: []` in `src/data/projects.js`
- [ ] Remaining systems (2–4 total expected, e.g. enemy AI, roguelite run/upgrade system, save system) — each needs a `name`, `points` (with optional nested `subPoints`), and a `why`
- [ ] Actual code snippets to embed alongside each system (syntax highlighting not yet implemented — see Tech Stack)
- [ ] Overview section copy (currently placeholder "Content coming soon")
- [ ] Challenges/retrospective copy (currently placeholder "Content coming soon")

Fighting card game (Timewarper-adjacent) slots in as a second case study once it's further along — I can pull context from what Claude Code has on that project when you're ready.

## Build Phases
1. **Scaffold** — Vite + React + Router + Tailwind setup ✅. Deploy to Vercel deliberately **on hold** until ready to go public (originally planned for day one — changed based on your call to stay local for now).
2. **Layout system** — reusable Project Case Study page template, nav, dark theme tokens ✅ (in progress, core structure and styling done — see Progress Log)
3. **Slime Guy case study** — build out full content using the template — 🔶 in progress (metadata + first system done, remaining systems/media/overview/challenges copy still needed)
4. **Code snippet component** — syntax highlighting + collapsible full view, reused across projects — not started
5. **Home/About/Contact** — tie it together — basic skeleton pages exist, content still placeholder
6. **Polish pass** — responsive check, load performance (especially gifs), meta tags/SEO, custom domain (point jared-kessler.com at Vercel)
7. **Second case study** (card game) once ready

## Progress Log
- **Scaffolded:** Vite + React 19 + React Router 7 + Tailwind v4, running locally via `npm run dev`. Not yet deployed anywhere (deliberate — staying local until ready to go public).
- **File structure:** `src/components/` (Nav, Footer, ProjectCard, MediaSlideshow, Divider), `src/pages/` (Home, ProjectPage, About, Contact), `src/data/projects.js` (single source of truth for all project content, looked up by slug).
- **Layout decisions:**
  - Page container width: `max-w-5xl` (1024px), consistent across Nav and all pages.
  - Project page header: centered title (`text-7xl`), a purple divider (`Divider` component, `h-0.5 w-200 bg-purple-500/40`) under the title, then a two-column row below — Roles/Timeframe/Released stacked vertically in a fixed-width left column (`sm:w-52`), description text filling the remaining width via `flex-1` (single-column stack on mobile).
  - Roles is an array (`roles: [...]`) — label auto-pluralizes to "Role"/"Roles" based on count.
  - `releaseDate` and `externalLink` (Steam/itch) are optional fields, only rendered when present — safe for in-progress projects that don't have them yet.
  - Section headers (Overview, Systems breakdown, Challenges & retrospective) are centered, `text-3xl`, each with its own `Divider` directly underneath (`mt-4` on both sides), `mt-16` between sections.
- **`MediaSlideshow` component:** takes `media: [{ type: 'video'|'image', src }]`. Videos autoplay muted/inline and advance via `onEnded` (no loop, except a single-item array which loops). Images hold for 4 seconds (`IMAGE_DURATION_MS`) then auto-advance. Purple dot indicators shown/clickable when there's more than one item. Falls back to a plain gray placeholder box when `media` is empty (current state for Slime Guy — no clips gathered yet).
- **Systems breakdown data shape:** `systems: [{ name, points, why }]`. `points` is a mixed array — plain strings render as flat bullets, or `{ text, subPoints: [...] }` objects render a nested sub-bullet list under that point. `why` renders as a paragraph directly below the bullet list (see Site Structure revision above).

## Open Decisions for Later
- Whether to add a blog/devlog section eventually (ties into your YouTube channel concept)
- Resume: embedded PDF viewer vs. just a download link

## Handoff Note
This plan is meant to be pasted into or referenced by Claude Code on your PC to kick off the actual build. Gather the Slime Guy content (gifs, snippets, writeup) first, then hand this doc + content over to start scaffolding.
