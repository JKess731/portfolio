using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RelicManager : MonoBehaviour, IManager
{
    // Singleton Pattern
    public static RelicManager Instance;
    [SerializeField] private int maxRelics = 10;

    // Data Structures
    private HashSet<RelicSO> _relicHashSet = new HashSet<RelicSO>();
    [SerializeField] private RelicSO[] _relicArray;

    // In order for the merge relics to be registered correctly, the relic manager needs to be able to
    // send out an event to every merge relic when a relic is picked up, this list takes each merge relic
    // and initializes it with this manager instance in the current scene
    [SerializeField] private List<MergeRelic> mergeRelicList;

    // These lists are the starting relic pools for this istance per rarity; These are the relics that the player can
    // get when encountering a shop or opening chest
    [Header("Regular Relic Pool")]
    [SerializeField] private RelicPool _commons;
    [SerializeField] private RelicPool _uncommons;
    [SerializeField] private RelicPool _rares;
    [SerializeField] private RelicPool _legendaries;

    // The All Relics relic pool provide easy access to references for all the relics in the game
    [Header("All Relic Pools")]
    [SerializeField] private RelicPool _commonsUI;
    [SerializeField] private RelicPool _uncommonsUI;
    [SerializeField] private RelicPool _raresUI;
    [SerializeField] private RelicPool _legendariesUI;
    [SerializeField] private RelicPool _mergeUI;
    [SerializeField] private RelicPool _evolutionUI;

    private List<RelicSO> _nonPool = new List<RelicSO>();
    private StatSO _stats;

    #region Properties

    // All properties provide references to other scripts

    public HashSet<RelicSO> RelicHashSet { get { return _relicHashSet; } }
    public RelicSO[] RelicArray { get { return _relicArray; } }
    public int RelicCount { get { return _relicHashSet.Count; } }

    public int MaxCount { get { return maxRelics; } }

    public RelicPool AllCommomns { get { return _commonsUI; } }
    public RelicPool AllUncommons { get { return _uncommonsUI; } }
    public RelicPool AllRares { get { return _raresUI; } }
    public RelicPool AllLegendaries { get { return _legendariesUI; } }
    public RelicPool AllMerge { get { return _mergeUI; } }
    public RelicPool AllEvolution { get { return _evolutionUI; } }

    public RelicPool ShopCommons { get { return _commons; } }
    public RelicPool ShopUncommons { get { return _uncommons; } }
    public RelicPool ShopRares { get { return _rares; } }
    public RelicPool ShopLegendaries { get{ return _legendaries; } }
    public List<RelicSO> NonPool { get { return _nonPool; } }

    #endregion

    #region Events
    // This Event gets called whenever a new Relic is picked up and added to the inventory
    [HideInInspector] public UnityEvent OnRelicPickup;
    #endregion

    #region IManager
    public bool Initialized { get => Instance != null; private set { } }

    public void InjectResolver(ObjectResolver resolver)
    {
        _stats = resolver.Get<PlayerController>().PlayerStateMachine.StatSO;
        Initialized = true;
    }

    /// <summary>
    /// Initializes this instance
    /// </summary>
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("RelicManager instance already exists. Destroying duplicate.");
            Destroy(gameObject);
        }

        _relicArray = new RelicSO[maxRelics];

        foreach (MergeRelic mergeRelic in mergeRelicList)
        {
            // Initialize each merge relic with the Stats SO
            mergeRelic.InitMerge(_stats);
        }
    }

    public void OnDestroy()
    {
    }

    public void Reset()
    {
        for (int i = 0; i < _relicArray.Length; i++)
        {
            try
            {
                _relicArray[i].RelicDescription = _relicArray[i].RelicDescriptionOriginal;
                try
                {
                    _relicArray[i].OnDrop();
                    try
                    {
                        _relicArray[i].ResetRelic();
                    } catch {  }
                }
                catch { }
            }
            catch (NullReferenceException)
            {
                // Do nothing, this is to prevent console errors
            }
        }
        _relicArray = new RelicSO[maxRelics];
        _relicHashSet.Clear();
        InventoryManager.Instance.UpdateUI();
        _nonPool.Clear();
    }

    #endregion

    /// <summary>
    /// Calls all neccesary functions and events needed when a relic is picked up
    /// </summary>
    /// <param name="r"> Relic scriptable object</param>
    public void RelicPickup(RelicSO r)
    {
        // Add the relic to the HashSet and the Array for the UI
        if (_relicHashSet.Count < maxRelics)
        {
            AddRelic(r);
            r.OnPickup();
            AddToNonPool(r);
            // Invoke the Pickup Event so all listeners get called
            OnRelicPickup.Invoke();
        }
    }

    /// <summary>
    /// Adds a relic directly into the inventory if possible
    /// </summary>
    /// <param name="r"> Relic scriptable object</param>
    public void AddRelic(RelicSO r)
    {
        if (!_relicHashSet.Contains(r) && _relicHashSet.Count < maxRelics)
        {
            _relicHashSet.Add(r);
            if (r.Stats == null)
            {
                StatSO stats = PlayerController.Instance.PlayerStateMachine.StatSO;
                r.Initialize(stats);
            }

            // Place this relic at the first null position in the array
            for (int i = 0; i < _relicArray.Length; i++)
            {
                if (_relicArray[i] != null)
                {
                    continue;
                }

                _relicArray[i] = r;
                break;
            }

            // Update the Inventory UI if the inventory is open and is in the scene while pickup is happening
            if (InventoryManager.Instance != null && InventoryManager.Instance.IsOpen)
            {
                InventoryManager.Instance.UpdateUI();
            }
        }
    }

    /// <summary>
    /// Removes a relic and replaces null at it's position in the array
    /// </summary>
    /// <param name="r"> Relic scriptable object</param>
    public void RemoveRelic(RelicSO r)
    {
        for (int i = 0; i < _relicArray.Length; i++)
        {
            if (_relicArray[i] != r)
            {
                continue;
            }

            _relicArray[i] = null;
            //UiManager.instance?.UpdateRelicImage(r, i, false);
            break;
        }

        _relicHashSet.Remove(r);
        if (InventoryManager.Instance != null && InventoryManager.Instance.IsOpen)
        {
            InventoryManager.Instance.UpdateUI();
        }
    }

    /// <summary>
    /// Returns a random relic from the pool
    /// </summary>
    public RelicSO RandomRelic()
    {

        List<RelicSO> _cRs = new List<RelicSO>();
        _cRs.AddRange(_commons.Relic);
        _cRs.RemoveAll(r => _relicHashSet.Contains(r));

        List<RelicSO> _uCRs = new List<RelicSO>();
        _uCRs.AddRange(_uncommons.Relic);
        _uCRs.RemoveAll(r => _relicHashSet.Contains(r));

        List<RelicSO> _rRs = new List<RelicSO>();
        _rRs.AddRange(_rares.Relic);
        _rRs.RemoveAll(r => _relicHashSet.Contains(r));

        List<RelicSO> _lRs = new List<RelicSO>();
        _lRs.AddRange(_legendaries.Relic);
        _lRs.RemoveAll(r => _relicHashSet.Contains(r));

        int uncommonChance = 30;
        int rareChance = 15;
        int legendaryChance = 5;

        int chance = UnityEngine.Random.Range(0, 101);
        if (chance <= legendaryChance)
        {
            if (_lRs.Count <= 0)
            {
                int r = UnityEngine.Random.Range(0, _cRs.Count);
                return _cRs[r];
            }

            int rand = UnityEngine.Random.Range(0, _lRs.Count);
            return _lRs[rand];
        }
        else if (chance <= rareChance)
        {
            int rand = UnityEngine.Random.Range(0, _rRs.Count);
            return _rRs[rand];
        }
        else if (chance <= uncommonChance)
        {
            int rand = UnityEngine.Random.Range(0, _uCRs.Count);
            return _uCRs[rand];
        }
        else
        {
            int rand = UnityEngine.Random.Range(0, _cRs.Count);
            return _cRs[rand];
        }
    }

    /// <summary>
    /// Adds a given relic to the pool of relics which can't be gotten again
    /// </summary>
    /// <returns></returns>
    public void AddToNonPool(RelicSO r)
    {
        if (!_nonPool.Contains(r))
            _nonPool.Add(r);
    }
}
