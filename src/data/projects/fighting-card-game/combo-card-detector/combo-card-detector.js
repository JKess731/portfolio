import comboCardDetectorCode from './ComboCardDetector.cs?raw'
import comboTreeNodeCode from './ComboTreeNode.cs?raw'

export const comboCardDetector = {
  name: 'Combo Card Detector',
  media: { type: 'video', src: '/media/fighting-card-game/Video/combocarddetector.mp4' },
  mediaWidth: 'w-180 mx-auto',
  points: [
    'Combos are modeled as a tree of follow-up cards per starting card, not a flat list of exact sequences',
    {
      text: 'Each combo tree asset holds a root card and a nested tree of children',
      subPoints: [
        'Every child entry has a card name, its own further children, and an optional combo-result card if that exact branch completes a combo',
      ],
    },
    {
      text: 'Tracks the cards played this turn in a queue, and re-searches the tree from the root every time a new card is played',
      subPoints: [
        "A combo only fires at a node whose result is set AND the played sequence matches that depth exactly, so a longer unrelated sequence sharing the same prefix can't accidentally trigger it",
      ],
    },
    'A short buffer timer restarts after every successful combo, giving the player a window to chain into an even longer combo before the played-card queue resets',
  ],
  why: "I built combos as a designer-authorable tree of ScriptableObjects instead of hardcoding sequences in a script, so a single starting card could branch into many different combos depending on what's played after it, and new combos could be added without touching code. This paired well with our GameCards, which were scriptable objects as well. Allowing us to rapidly iterate creating cards and combos and see what sticks.",
  snippets: [
    {
      language: 'csharp',
      title: 'ComboCardDetector.cs',
      code: comboCardDetectorCode,
    },
    {
      language: 'csharp',
      title: 'ComboTreeNode.cs',
      code: comboTreeNodeCode,
    },
  ],
}
