import { proceduralRoomGeneration } from './procedural-room-generation/procedural-room-generation.js'
import { relicUpgradesSystem } from './relic-upgrades-system/relic-upgrades-system.js'

export const slimeGuy = {
  slug: 'slime-guy',
  title: 'Slime Guy',
  status: 'Released',
  accent: 'green',
  hook: 'Slime Guy is a roguelite dungeon-crawler, except you play as a seemingly weak slime with powerful slime abilities. Powerup by obtaining new abilities and relics to build out your strengths, traverse through procedurally generated levels, and battle your way to save your kidnapped family.',
  roles: ['Lead Producer', 'Gameplay Programmer'],
  timeframe: '2023 – 2025',
  releaseDate: 'December 18, 2025',
  externalLink: "https://store.steampowered.com/app/3572850/Slime_Guy/", // Steam/itch.io link goes here
  thumbnail: null,
  // Each entry: { type: 'video', src: '/media/slime-guy/combat.webm' }
  // or { type: 'image', src: '/media/slime-guy/upgrade-screen.png' }
  media: [
    { type: 'video', src: '/media/slime-guy/Video/slimeguy1.mp4' },
    { type: 'video', src: '/media/slime-guy/Video/slimeguy2.mp4' },
    { type: 'video', src: '/media/slime-guy/Video/slimeguy3.mp4' },
    { type: 'video', src: '/media/slime-guy/Video/slimeguy4.mp4' },
    { type: 'image', src: '/media/slime-guy/Image/image1.png' },
    { type: 'image', src: '/media/slime-guy/Image/image2.png' },
  ],
  systems: [proceduralRoomGeneration, relicUpgradesSystem],
  challenges: [
    "Slime Guy was a project which started as a small idea on a one page pitch document. Originally designed to be a top-down rpg with combat elements, the game quickly was tuned into a top-down fast-paced roguelite experience. The focus for the main mechanic being the slime-based abilities the player could unlock and endlessly battle with. At Indiana University in the fall of 2023, Slime Guy's pitch deck evolved as members were added to the team and got slowly crafted and tweaked until finally at the end of that semester, Slime Guy was greenlit for further development, following the program's \"Shark Tank\" pitch process.",
    "At the start of 2024, rapid prototyping development was started on Slime Guy. In the beginning I wanted the focus to be on developing Puddles, the actual slime in the game the players would play as. I wanted the experience to feel good playing as Puddles, and we spent a lot of time together designing abilities and the code to go along with them. It was at this time when I learned how critical it is to have multiple artists on the team. We only had 1, and Slime Guy's design and code was outpacing the art output.",
    "The biggest challenges was our code architecture as well as scope. I had grossly overestimated the amount of time we had and the amount of time it would take us to implement our designs, a mistake I would not make again. As a result we had to scope down a lot and take designs out of the game and change ideas due to the time contraists of needing to have the game finished by graduation. Early on we didn't have a strong pattern in place for initialization and dependency management, so as new features came in we kept bolting them onto existing systems rather than designing for them upfront. That produced a lot of spaghetti code, and a good chunk of our development time went into refactoring systems we'd already built to accommodate things we hadn't planned for. We eventually cleaned this up by moving to a Single Entry Point pattern and fixing our dependency injection, which made the codebase far more maintainable — but it came later than it should have. On top of that, our creative ambitions outpaced our experience: we set out to hit a very specific, detailed design vision, and while we got there, the gameplay ended up feeling clunkier and less polished than a game of this scope really needed. We ended up with more bandaids in places keeping the code together than what we should have had.",
    "If I were starting over, I'd invest in a scalable code architecture from day one instead of retrofitting one when more than half of the game is already coded. Establishing patterns like Single Entry Point and proper dependency injection before piling on systems, not after. I'd also be more disciplined about scope: matching what we designed to what a small, first-time team could realistically execute and polish, rather than committing to the full vision up front and cutting corners later to hit the deadline.",
  ],
  pitchDeck: [
    { type: 'image', src: '/media/slime-guy/pitch-deck/slide-01.png', title: 'Slime Guy' },
    { type: 'image', src: '/media/slime-guy/pitch-deck/slide-02.png', title: 'What is Slime Guy?' },
    { type: 'image', src: '/media/slime-guy/pitch-deck/slide-03.png', title: 'Selling Points' },
    {
      type: 'image',
      src: '/media/slime-guy/pitch-deck/slide-04.png',
      title: 'Gameplay Preview',
      overlays: [
        {
          src: '/media/slime-guy/pitch-deck/gifs/slide-04.gif',
          left: '0%',
          top: '0%',
          width: '100%',
          height: '100%',
        },
      ],
    },
    {
      type: 'image',
      src: '/media/slime-guy/pitch-deck/slide-05.png',
      title: 'Absorption (Light Attack)',
      overlays: [
        {
          src: '/media/slime-guy/pitch-deck/gifs/slide-05.gif',
          left: '17.32%',
          top: '31.51%',
          width: '65.36%',
          height: '59.34%',
        },
      ],
    },
    {
      type: 'image',
      src: '/media/slime-guy/pitch-deck/slide-06.png',
      title: 'Slime Slam (Heavy Attack)',
      overlays: [
        {
          src: '/media/slime-guy/pitch-deck/gifs/slide-06-a.gif',
          left: '20.81%',
          top: '31.66%',
          width: '56.72%',
          height: '27.70%',
        },
        {
          src: '/media/slime-guy/pitch-deck/gifs/slide-06-b.gif',
          left: '20.81%',
          top: '62.96%',
          width: '43.49%',
          height: '27.56%',
        },
      ],
    },
    {
      type: 'image',
      src: '/media/slime-guy/pitch-deck/slide-07.png',
      title: 'Slime Trail (Passive)',
      overlays: [
        {
          src: '/media/slime-guy/pitch-deck/gifs/slide-07.gif',
          left: '17.74%',
          top: '30.54%',
          width: '64.30%',
          height: '60.22%',
        },
      ],
    },
    {
      type: 'image',
      src: '/media/slime-guy/pitch-deck/slide-08.png',
      title: 'Slime Split (Dodge)',
      overlays: [
        {
          src: '/media/slime-guy/pitch-deck/gifs/slide-08-a.gif',
          left: '23.74%',
          top: '26.65%',
          width: '44.29%',
          height: '38.67%',
        },
        {
          src: '/media/slime-guy/pitch-deck/gifs/slide-08-b.gif',
          left: '24.10%',
          top: '68.89%',
          width: '43.18%',
          height: '27.55%',
        },
      ],
    },
    {
      type: 'image',
      src: '/media/slime-guy/pitch-deck/slide-09.png',
      title: 'Dash (Movement)',
      overlays: [
        {
          src: '/media/slime-guy/pitch-deck/gifs/slide-09.gif',
          left: '13.45%',
          top: '47.42%',
          width: '75.89%',
          height: '33.73%',
        },
      ],
    },
    { type: 'image', src: '/media/slime-guy/pitch-deck/slide-10.png', title: 'Game Loop' },
    { type: 'image', src: '/media/slime-guy/pitch-deck/slide-11.png', title: 'Evolutions', notes: 'Evolutions & Vitamins would be merged into \"Relics\".'},
    { type: 'image', src: '/media/slime-guy/pitch-deck/slide-12.png', title: 'Game Web', notes: 'Shown is the games in which Slime Guy takes inspiration.'},
    { type: 'image', src: '/media/slime-guy/pitch-deck/slide-13.png', title: 'Market' },
    { type: 'image', src: '/media/slime-guy/pitch-deck/slide-14.png', title: 'Story' },
    { type: 'image', src: '/media/slime-guy/pitch-deck/slide-15.png', title: 'Scope', notes: 'The scope was very overestimated, and would end up heavily cut down and changed by the time Slime Guy was released.'},
    { type: 'image', src: '/media/slime-guy/pitch-deck/slide-16.png', title: 'Game Details' },
  ],
}
