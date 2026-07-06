import deckManagerCode from './DeckManager.cs?raw'

export const deckManager = {
  name: 'Deck Manager',
  media: null,
  points: [
    'Builds the runtime deck at match start by loading every \'GameCard\' ScriptableObject from Resources and duplicating each one a set number of times, then shuffling with a Fisher–Yates shuffle into a queue',
    {
      text: 'Card selection uses a multi-hit raycast plus a tag filter rather than a single raycast',
      subPoints: [
        "This is specifically so the player's own animation/body collider can't block a click on a card that overlaps it visually",
      ],
    },
    {
      text: 'Click-and-drag interaction',
      subPoints: [
        'Mouse down selects a card and starts following the cursor in world space every frame',
        "Mouse up either plays the card (if the turn's play window is open) or snaps it back to its original hand position",
      ],
    },
    "DeckManager wraps the currently selected card and hands it to TurnManager's card queue, and separately notifies the Combo Card Detector in case the play completes a combo",
  ],
  why: "I kept DeckManager focused purely on input and hand-state (what's in the player's hand, what they're trying to play) and pushed actual card-effect execution into TurnManager's queue, so there was only one place in the codebase that ran card effects instead of the logic being duplicated between the two systems.",
  snippets: [
    {
      language: 'csharp',
      title: 'DeckManager.cs',
      code: deckManagerCode,
    },
  ],
}
