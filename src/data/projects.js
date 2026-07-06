import { slimeGuy } from './projects/slime-guy/slime-guy.js'
import { fightingCardGame } from './projects/fighting-card-game/fighting-card-game.js'

export const projects = [slimeGuy, fightingCardGame]

export function getProjectBySlug(slug) {
  return projects.find((p) => p.slug === slug)
}
