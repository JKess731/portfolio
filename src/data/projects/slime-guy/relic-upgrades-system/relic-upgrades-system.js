import relicManagerCode from './RelicManager.cs?raw'
import relicSOCode from './RelicSO.cs?raw'
import mergeRelicCode from './MergeRelic.cs?raw'
import multiplicativeStatChangeRelicCode from './MultiplicativeStatChangeRelic.cs?raw'
import evolvingRelicCode from './EvolvingRelic.cs?raw'

export const relicUpgradesSystem = {
  name: 'Relic Upgrades System',
  media: null,
  gallery: [
    { type: 'image', src: '/media/slime-guy/Image/relic1.avif' },
    { type: 'image', src: '/media/slime-guy/Image/relic2.avif' },
    { type: 'image', src: '/media/slime-guy/Image/relic3.avif' },
    { type: 'image', src: '/media/slime-guy/Image/relic4.avif' },
  ],
  points: [
    'Uses parent abstract Relic Class which is a Scriptable Object with functions that every relic needs to have',
    {
      text: 'Different types of relics have separate classes',
      subPoints: [
        '**Stat-based** relics contain the stats that are to be changed in the code',
        '**Status augment** relics contain a reference to the status effect that needs to be augmented'
      ]
    },
    {
      text: 'Some relics when all collected merge into another powerful relic',
      subPoints:
      [
        {
          text: 'The Relic Manager has a reference to each merge relic and the merge relic itself has a reference to each required relic to merge',
          subPoints: [
            'Whenever a relic is picked up or acquired, an event OnPickup fires and tells the merge relic to check if a required relic is equipped',
            'This is done by keeping a reference to all merge relic Scriptable Objects when the game is loaded, and subscribing a function from the merge relic to an event'
          ]
        }
      ]
    }
  ],
  why: "I created this scriptable object system for the project to be scalable and allow for fast implementation of new upgrades in the future, as some of the designs given to me are similar, so I opted to create an implementation that could allow me to code 1 script for 3 relics, or 1 script for 10 relics in some extreme cases. I also had to keep in mind that I needed to easily access data for the relics, such as their UI Icon and name which I needed to display in multiple different UI menus. ",
  snippets: [
    {
      language: 'csharp',
      title: 'RelicManager.cs',
      code: relicManagerCode,
    },
    {
      language: 'csharp',
      title: 'RelicSO.cs',
      code: relicSOCode,
    },
    {
      language: 'csharp',
      title: 'MergeRelic.cs',
      code: mergeRelicCode,
    },
    {
      language: 'csharp',
      title: 'MultiplicativeStatChangeRelic.cs',
      code: multiplicativeStatChangeRelicCode,
    },
    {
      language: 'csharp',
      title: 'EvolvingRelic.cs',
      code: evolvingRelicCode,
    },
  ],
}
