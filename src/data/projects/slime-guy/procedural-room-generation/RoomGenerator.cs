using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField ] private ProceduralManager manager;
    [SerializeField ] private NodeGenerator nodeGenerator;

    // Data for room storage -> Gets passed down from ProceduralManager
    private int roomCount;
    private int arrRow;
    private int arrCol;
    private float spawnDelay;
    private GameObject[,] roomArray;
    private RoomNode[,] nodeArray;
    private Queue<RoomTag> roomTags;

    private RoomList emptyRoomList;
    private RoomList basicRoomList;
    private RoomList eventRoomList;
    private RoomList waveRoomList;
    private RoomList abilityShopRoomList;
    private RoomList relicShopRoomList;
    private RoomList bossRoomList;

    public UnityEvent OnRoomsFinished;

    #region Properties
    public int RoomCount { get { return roomCount; } set { roomCount = value; } }
    public int ArrayRow { get { return arrRow; } set { arrRow = value; } }
    public int ArrayCol { get { return arrCol; } set { arrCol = value; } }
    public float SpawnDelay {  get { return spawnDelay; } set { spawnDelay = value; } }
    public ProceduralManager Manager { get { return manager; } set { manager = value; } }

    #endregion

    #region Classes
    private class ArrayCoordinate
    {
        public int row = 0;
        public int col = 0;
    }

    #endregion

    private void Start()
    {
        nodeGenerator.OnNodesFinishes.AddListener(StartRoomGeneration);
    }

    #region Functions

    // Returns a new RoomNode 2D array to store Node
    private GameObject[,] CreateRoomArray(int r, int c)
    {
        return new GameObject[r, c];
    }

    public void InitRoomGenerator()
    {
        roomArray = CreateRoomArray(arrRow, arrCol);

        if (GameManager.Instance.Floor == 0) manager.Coins.DecreaseValue(manager.Coins.Value);

        if (GameManager.Instance.Floor < 3)
        {
            GameManager.Instance.Floor++;
        }

        if (GameManager.Instance.Floor == 3)
        {
            bossRoomList = manager.BossRooms;
        }
        else bossRoomList = manager.EndRooms;
    }

    private void StartRoomGeneration()
    {
        // Get Values needed after Nodes are finished generator
        nodeArray = nodeGenerator.NodeArray;
        roomTags = manager.TagQueue;

        emptyRoomList = manager.EmptyRooms;
        basicRoomList = manager.BasicRooms;
        waveRoomList = manager.WaveRooms;
        abilityShopRoomList = manager.AbilityShopRooms;
        relicShopRoomList = manager.RelicShopRooms;
        eventRoomList = manager.EventRooms;

        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        for (int i = 0; i < nodeGenerator.NodesPlaced.Count; i++)
        {
            yield return new WaitForSeconds(spawnDelay);
            // Get a reference to the current node
            RoomNode node = nodeGenerator.NodesPlaced[i];
            GameObject nodeObj = node.gameObject;
            ArrayCoordinate nodeCoords = GetNodeCoord(node);
            int row = nodeCoords.row;
            int col = nodeCoords.col;

            // Create a list of doors based on node's neighbors
            List<DoorTypes> doorsNeeded;
            doorsNeeded = GetNodeNeighbors(node, nodeCoords);

            RoomList rl = GetRoomListByRoomType(roomTags.Dequeue());
            //Debug.Log(rl.tag);

            GameObject room = GetRoomByDoorsNeeded(doorsNeeded, rl);
            try
            {
                GameObject spawnedRoom = Instantiate(room, nodeObj.transform.position, Quaternion.identity, transform);

                roomArray[row, col] = spawnedRoom;
                manager.RoomsPlaced.Add(spawnedRoom);
            }
            catch
            {
                Debug.Log("tried room: " + room.name);
                break;
            }
        }

        OnRoomsFinished?.Invoke();
        EventManager.Instance.EnterFloor.Invoke();
        PlayerController.Instance.EnableGameplay();
    }

    private List<DoorTypes> GetNodeNeighbors(RoomNode node, ArrayCoordinate nodeCoord)
    {
        List<DoorTypes> doorsNeeded = node.doors;

        return doorsNeeded;
    }

    private ArrayCoordinate GetNodeCoord(RoomNode node)
    {
        ArrayCoordinate coord = new ArrayCoordinate();

        for (int r = 0; r < nodeGenerator.NodeArray.GetLength(0); r++)
        {
            for (int c = 0; c < nodeGenerator.NodeArray.GetLength(1); c++)
            {
                if (nodeGenerator.NodeArray[r,c] == node)
                {
                    coord.row = r;
                    coord.col = c;
                }
            }
        }

        return coord;
    }

    private GameObject GetRoomByDoorsNeeded(List<DoorTypes> doorsNeeded, RoomList list)
    {
        List<GameObject> byCount = new List<GameObject>();
        //Debug.Log("TAKES: " + doorsNeeded.Count);

        foreach (GameObject r in list.rooms)
        {
            RoomTypes rt = r.GetComponent<RoomTypes>();
            if (rt.doors.Count == doorsNeeded.Count)
            {
                byCount.Add(r);
            }
        }

        List<GameObject> options = new List<GameObject>();

        foreach (GameObject r in byCount)
        {
            RoomTypes rt = r.GetComponent<RoomTypes>();
            int c = 0;

            foreach (DoorTypes d in doorsNeeded)
            {
                if (rt.doors.Contains(d))
                {
                    c++;
                }
            }
            if (c == doorsNeeded.Count)
            {
                options.Add(r);
            }
        }

        // Catch For Room Errors
        try
        {
            GameObject result = options[Random.Range(0, options.Count)];
            //Debug.Log("GIVES: " + result.name);
            return result;
        }
        catch (System.ArgumentOutOfRangeException)
        {
            string s = "NEEDED: ";
            foreach (DoorTypes d in doorsNeeded)
            {
                s += d + " ";
            }
            Debug.Log(s + " " + list.tag);
            return null;
        }
    }

    private RoomList GetRoomListByRoomType(RoomTag t)
    {
        if (t == RoomTag.EMPTY)
        {
            return emptyRoomList;
        }
        else if (t == RoomTag.BASIC)
        {
            return basicRoomList;
        }
        else if (t == RoomTag.EVENT)
        {
            return eventRoomList;
        }
        else if (t == RoomTag.WAVE)
        {
            return waveRoomList;
        }
        else if (t == RoomTag.ABILITY_SHOP)
        {
            return abilityShopRoomList;
        }
        else if (t == RoomTag.RELIC_SHOP)
        {
            return relicShopRoomList;
        }
        else if (t == RoomTag.BOSS)
        {
            return bossRoomList;
        }
        else
        {
            return emptyRoomList;
        }
    }

    #endregion
}
