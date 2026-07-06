using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProceduralManager : MonoBehaviour
{
    // Generators
    [SerializeField] private NodeGenerator _nodeGenerator;
    [SerializeField] private RoomGenerator _roomGenerator;

    [Header("Rooms")]
    [Space]
    [SerializeField] private List<LevelRoomInfo> _roomsTypesList = new List<LevelRoomInfo>();

    // Array Storage Info
    [Space]
    [Header("Settings")]
    [SerializeField] private float _spawnDelay = .25f;
    [SerializeField] private int _rowSize = 10;
    [SerializeField] private int _colSize = 10;

    // Room Lists
    //[SerializeField] private GameObject startRoom;
    [SerializeField] private RoomList _emptyRoomList;
    [SerializeField] private RoomList _eventRoomList;
    [SerializeField] private RoomList _waveRoomList;
    [SerializeField] private RoomList _basicRoomList;
    [SerializeField] private RoomList _bossRoomList;
    [SerializeField] private RoomList _abilityShopRoomList;
    [SerializeField] private RoomList _relicShopRoomList;
    [SerializeField] private RoomList _endRoomList;
    [SerializeField] private IntSO _coins;
    private Queue<RoomTag> _tagQueue = new Queue<RoomTag>();
    private List<GameObject> _roomsPlaced = new List<GameObject>();

    #region Properties

    public List<LevelRoomInfo> RoomsTypesList { get { return _roomsTypesList; } }
    public NodeGenerator NodeGenerator { get { return _nodeGenerator; } }
    public RoomGenerator RoomGenerator { get { return _roomGenerator; } }
    public Queue<RoomTag> TagQueue { get { return _tagQueue; } }

    public IntSO Coins { get { return _coins; } }
    public RoomList EmptyRooms { get { return _emptyRoomList; } }
    public RoomList BasicRooms {  get {  return _basicRoomList; } }
    public RoomList WaveRooms {  get { return _waveRoomList; } }
    public RoomList AbilityShopRooms { get { return _abilityShopRoomList; } }
    public RoomList RelicShopRooms { get { return _relicShopRoomList; } }
    public RoomList EventRooms { get { return _eventRoomList; } }
    public RoomList BossRooms { get { return _bossRoomList; } }
    public RoomList EndRooms { get { return _endRoomList; } }
    public List<GameObject> RoomsPlaced { get { return _roomsPlaced; } }

    #endregion

    #region Classes
    private class ArrayCoordinate
    {
        public int row = 0;
        public int col = 0;
    }

    [Serializable]
    public class LevelRoomInfo
    {
        public RoomTag rt;
        public int count;
    }

    #endregion

    #region Functions
    private void Awake()
    {
        if (GameManager.Instance.ProceduralManagerInstance == null)
        {
            GameManager.Instance.ProceduralManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private RoomTemplate LoadRooms()
    {
        switch (GameManager.Instance.Floor)
        {
            case 1:
                return Resources.Load<RoomTemplate>("Rooms/RoomTemplate1");
            case 2:
                return Resources.Load<RoomTemplate>("Rooms/RoomTemplate2");
            case 3:
                return Resources.Load<RoomTemplate>("Rooms/RoomTemplate3");
        }
        return null;
    }

    public void DestroyLevel()
    {
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        _roomsPlaced.Clear();
    }

    public void InitGenerators()
    {
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        _roomsPlaced.Clear();

        PlayerController.Instance?.DisableGameplay();

        // Initializes both Node & Room Generators with Needed data
        _nodeGenerator.ArrayRow = _rowSize;
        _nodeGenerator.ArrayCol = _colSize;

        // ----------------------------------
        _roomGenerator.ArrayRow = _rowSize;
        _roomGenerator.ArrayCol = _colSize;
        _roomGenerator.SpawnDelay = _spawnDelay;

        // Lets the room generator grab a reference to the node generator from here
        _roomGenerator.InitRoomGenerator();

        RoomTemplate t = LoadRooms();
        _roomsTypesList = t.GetRoomTemplate();
        //yield return new WaitForSeconds(1.5f);

        // Tell the Node Generator how many nodes it needs to spawn
        _nodeGenerator.NodeCount = GetRoomTotalCount();
        _roomGenerator.RoomCount = GetRoomTotalCount();

        _nodeGenerator.InitNodeGenerator();
        _nodeGenerator.StartGen();
        QueueTags(_roomsTypesList);
    }

    private int GetRoomTotalCount()
    {
        int i = 0;
        foreach (LevelRoomInfo lri in _roomsTypesList)
        {
            i += lri.count;
        }
        i++; // Add for boss room
        return i;
    }

    private void QueueTags(List<LevelRoomInfo> list)
    {
        List<RoomTag> newList = new List<RoomTag>();

        foreach (LevelRoomInfo i in list)
        {
            if (i.rt != RoomTag.EMPTY && i.rt != RoomTag.BOSS)
            {
                for (int n = i.count; n > 0; n--)
                {
                    newList.Add(i.rt);
                }
            }
        }

        Shuffle(newList);

        newList.Insert(0, RoomTag.EMPTY);
        foreach (RoomTag t in newList)
        {
            _tagQueue.Enqueue(t);
        }
        _tagQueue.Enqueue(RoomTag.BOSS);
        //Debug.Log(tagQueue.Count);
    }

    #endregion

    #region Utility
    private void Shuffle<T>(List<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
    #endregion
}
