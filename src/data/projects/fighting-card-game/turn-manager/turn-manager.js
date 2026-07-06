import turnManagerCode from './TurnManager.cs?raw'

export const turnManager = {
  name: 'Turn Manager',
  media: null,
  points: [
    'Drives the match as a strict three-phase state machine per round: Draw → Play → Resolve',
    {
      text: 'Draw Phase',
      subPoints: [
        'Clears any leftover cards from the previous hand and reshuffles the deck if the deck count is at 0',
        "Has the enemy draw its hand, then deals the player's hand one card at a time on a short delay rather than all at once",
      ],
    },
    {
      text: 'Play Phase',
      subPoints: [
        "Opens the window where the player's cards become playable, and starts the enemy's own card-playing routine at the same time so both fighters act concurrently",
        'Triggers a one-time "lunge" animation on turn 1 to close the starting distance between fighters',
      ],
    },
    {
      text: 'Resolve Phase',
      subPoints: [
        "Waits for the player's queued cards to finish executing and for the enemy's play routine to complete before ending the turn",
      ],
    },
    {
      text: 'Cards are queued, not executed instantly',
      subPoints: [
        "Playing a card adds it to a queue; the next one is only pulled once the player's animation state is back to IDLE, so effects play out one at a time without interrupting each other",
        'If the fighters are too far apart when a card is due to execute, TurnManager moves the player closer automatically instead of firing the card, so attacks read as actually connecting',
      ],
    },
  ],
  why: 'The three-phase structure exists because this is a hybrid of turn-based deckbuilding and real-time combat. The enemy\'s concurrent routine and animation-driven card queue keep the actual fights feeling live instead of strictly alternating. We added the check to close the distance gap as a way to make the combat feel fluid and to keep the game feel exciting and snappy.',
  snippets: [
    {
      language: 'csharp',
      title: 'TurnManager.cs',
      code: turnManagerCode,
    },
  ],
}
