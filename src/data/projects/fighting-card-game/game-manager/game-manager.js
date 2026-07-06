import gameManagerCode from './GameManager.cs?raw'
import gameContextCode from './GameContext.cs?raw'
import iManagerCode from './IManager.cs?raw'

export const gameManager = {
  name: 'Game Manager',
  media: null,
  points: [
    'Single entry point for the whole scene: loads every manager prefab and resource, instantiates them, and drives their initialization in a specific dependency order',
    {
      text: 'Initialization runs as a sequenced coroutine',
      subPoints: [
        'Loads manager/prefab references from Resources',
        'Instantiates the player and enemy, then grabs their animation-control components',
        'Instantiates every manager and injects a shared `GameContext` into each one',
      ],
    },
    {
      text: 'Uses a shared \'GameContext\' object to handle dependency injection',
      subPoints: [
        'Managers pull the specific references they need (e.g. TurnManager needs DeckManager, EventManager, etc.) straight off the shared context instead of finding each other independently',
      ],
    }
  ],
  why: "When prototyping started every system was a singleton object that could be accessed from anywhere. I also needed an easy way to initialize everything in my game to prevent null reference exceptions. My GameManager system uses a single entry point concept to load everything needed for the scene, and to inject all the needed dependencies into every manager. Allowing the fighting action to start and perform seamlessly.",
  snippets: [
    {
      language: 'csharp',
      title: 'GameManager.cs',
      code: gameManagerCode,
    },
    {
      language: 'csharp',
      title: 'GameContext.cs',
      code: gameContextCode,
    },
    {
      language: 'csharp',
      title: 'IManager.cs',
      code: iManagerCode,
    },
  ],
}
