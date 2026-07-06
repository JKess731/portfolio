import { gameManager } from './game-manager/game-manager.js'
import { turnManager } from './turn-manager/turn-manager.js'
import { deckManager } from './deck-manager/deck-manager.js'
import { comboCardDetector } from './combo-card-detector/combo-card-detector.js'

export const fightingCardGame = {
  slug: "fighting-card-game",
  title: "Untitled Fighting Game",
  status: 'In Progress',
  accent: 'red',
  hook: 'The Untitled Fighting Game is a real-time fighting deckbuilder where the player plays to their card in real-time fast-paced combat. Create unique combos, unlock new cards, and defeat the other fighters in the prison gladiator fighting pits.',
  roles: ['Producer', 'Gameplay Programmer', 'Game Designer'],
  timeframe: '2025 – Present',
  releaseDate: null,
  externalLink: null,
  thumbnail: null,
  media: [
    { type: 'video', src: '/media/fighting-card-game/Video/video2.mp4' },
    { type: 'video', src: '/media/fighting-card-game/Video/combocarddetector.mp4' },
    { type: 'video', src: '/media/fighting-card-game/Video/video3.mp4' },
  ],
  systems: [gameManager, turnManager, deckManager, comboCardDetector],
}
