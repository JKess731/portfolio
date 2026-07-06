import proceduralManagerCode from './ProceduralManager.cs?raw'
import roomGeneratorCode from './RoomGenerator.cs?raw'
import nodeGeneratorCode from './NodeGenerator.cs?raw'

export const proceduralRoomGeneration = {
  name: 'Procedural Room Generation',
  media: { type: 'image', src: '/media/slime-guy/Video/procgen.gif' },
  points: [
    'Loosely based on the Wave Function Collapse Algorithm',
    'Given a list of room types (Wave, Event, Shop, etc...) and generates a random order in a queue',
    {
      text: 'Starts by placing nodes for each room in the scene',
      subPoints: [
        'Each new spawned node checks for existing neighbor nodes, and walks in a random empty direction',
        'Nodes are stored in a 2D array with dimensions set in the inspector',
      ],
    },
    {
      text: 'After all nodes are placed, the algorithm places prefab rooms from scriptable object lists on top of the nodes',
      subPoints: [
        'When a room is placed on top of a node, it checks the neighbors of that node so it knows which room to place, and takes the room type from the top of the queue generated at the beginning and goes into the corresponding scriptable object list to grab the room',
      ],
    },
  ],
  why: 'I chose to do the algorithm as more of a Semi-Procedural, almost lego block placing algorithm because of the time constraints on the project at the time. We knew we really only had 2 years to work on the project, as this was a capstone project for our game design program: And at the time we were only 6 months into development in 2024 we were originally supposed to have an expected release in April or May in 2025. Because of this fact, I opted to implement a solution that would generate random maps for us but with pre-designed handcrafted pieces that could be put together by checking which rooms can be placed next to each other and also checking the bounds set for the map.',
  snippets: [
    {
      language: 'csharp',
      title: 'ProceduralManager.cs',
      code: proceduralManagerCode,
    },
    {
      language: 'csharp',
      title: 'RoomGenerator.cs',
      code: roomGeneratorCode,
    },
    {
      language: 'csharp',
      title: 'NodeGenerator.cs',
      code: nodeGeneratorCode,
    },
  ],
}
