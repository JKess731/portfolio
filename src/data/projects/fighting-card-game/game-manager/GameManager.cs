using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour, IManager
{
    private string _resourceManagerPath = "Prefabs/Managers/";
    private string _resourcePrefabPath = "Prefabs/";

    [SerializeField] private bool _debugMode = false;
    private bool _initialized = false;

    private GameContext _gameContext;

    private UIManager _uiManager;
    private GameObject _uiCanvas;

    private TurnManager _turnManager;
    private ComboCardDetector _comboCardDetector;
    private GameObject _deckObject;
    private DeckManager _deckManager;
    private EnemyDeckManager _enemyDeckManager;

    private PlayerAnimationControl _playerAnimationControl;
    private EnemyAnimationControl _enemyAnimationControl;
    private Transform _playerTransform;
    private Transform _enemyTransform;

    private EventManager _eventManager;
    private GameObject _cameras;
    private GameObject _background;

    private List<GameCard> _allCards;
    private Dictionary<GameCardName, GameCard> _cardDictionary = new Dictionary<GameCardName, GameCard>();
    private InputActionMap _debugMap;
    private Dictionary<GameCardName, InputAction> _debugInputActions = new Dictionary<GameCardName, InputAction>();

    public bool DebugMode { get { return _debugMode; } }
    public bool Initialized { get { return _initialized; } }

    void Start()
    {
        _gameContext = new GameContext();
        StartCoroutine(Initialize(_gameContext));
    }

    /// <summary>
    /// Initializes the game by loading managers, instantiating player and enemy, and setting up managers.
    /// </summary>
    public IEnumerator Initialize(GameContext ctx)
    {

        yield return StartCoroutine(LoadManagers());

        //--------------------------------------------------------

        _playerTransform = Instantiate(_playerTransform);
        _enemyTransform = Instantiate(_enemyTransform);

        InitializePlayerAndEnemy();

        //--------------------------------------------------------

        yield return StartCoroutine(InitializeManagers());
        _initialized = true;
        Debug.Log("GameManager: Initialization complete.");

        if (_debugMode)
        {
            _allCards = _deckManager.AllCards;
            _debugMap = GetDebugInput();
            for (int i = 0; i < _allCards.Count; ++i)
            {
                _cardDictionary.Add(_allCards[i].GameCardName, _allCards[i]);
            }
            RunDebugMode();
        }

        _eventManager.OnTurnStart?.Invoke();
    }

    public void Inject(GameContext ctx)
    {
        _gameContext.GameManager                = this;
        _gameContext.UICanvas                   = _uiCanvas;
        _gameContext.UIManager                  = _uiManager;
        _gameContext.TurnManager                = _turnManager;
        _gameContext.DeckManager                = _deckManager;
        _gameContext.EnemyDeckManager           = _enemyDeckManager;
        _gameContext.ComboCardDetector          = _comboCardDetector;
        _gameContext.PlayerAnimationControl     = _playerAnimationControl;
        _gameContext.EnemyAnimationControl      = _enemyAnimationControl;
        _gameContext.EventManager               = _eventManager;
    }

    #region Game Initialization Stage

    /// <summary>
    /// Loads the necessary managers and resources from the specified paths.
    /// </summary>
    public IEnumerator LoadManagers()
    {
        _uiManager          = Resources.Load<UIManager>(_resourceManagerPath + "UIManager");
        _eventManager       = Resources.Load<EventManager>(_resourceManagerPath + "EventManager");

        _deckObject         = Resources.Load<GameObject>(_resourceManagerPath + "Deck");
        _playerTransform    = Resources.Load<Transform>(_resourcePrefabPath + "Player");
        _enemyTransform     = Resources.Load<Transform>(_resourcePrefabPath + "Enemy");

        _cameras            = Resources.Load<GameObject>(_resourcePrefabPath + "Cameras");
        _background         = Resources.Load<GameObject>(_resourcePrefabPath + "Background");

        yield return null;
    }

    /// <summary>
    /// Initializes the managers by instantiating them and setting them up with the necessary references.
    /// </summary>
    public IEnumerator InitializeManagers()
    {
        InstantiateManagers();

        yield return StartCoroutine(_uiManager.Initialize(_gameContext));
        yield return StartCoroutine(_eventManager.Initialize(_gameContext));
        yield return StartCoroutine(_deckManager.Initialize(_gameContext));
        yield return StartCoroutine(_comboCardDetector.Initialize(_gameContext));
        yield return StartCoroutine(_enemyDeckManager.Initialize(_gameContext));
        yield return StartCoroutine(_turnManager.Initialize(_gameContext));

        yield return StartCoroutine(_playerAnimationControl.Initialize(_gameContext));
        yield return StartCoroutine(_enemyAnimationControl.Initialize(_gameContext));

        yield return null;
    }

    /// <summary>
    /// Initializes the player and enemy
    /// </summary>
    private void InitializePlayerAndEnemy()
    {
        _playerAnimationControl = _playerTransform.GetComponent<PlayerAnimationControl>();

        _enemyAnimationControl  = _enemyTransform.GetComponent<EnemyAnimationControl>();
    }

    /// <summary>
    /// Instantiates the managers in the scene
    /// </summary>
    private void InstantiateManagers()
    {
        _uiManager          = Instantiate(_uiManager);
        _uiCanvas           = _uiManager.gameObject;

        _eventManager       = Instantiate(_eventManager);

        _deckObject         = Instantiate(_deckObject);
        _deckManager        = _deckObject.GetComponent<DeckManager>();
        _enemyDeckManager   = _enemyTransform.GetComponent<EnemyDeckManager>();
        _comboCardDetector  = _deckObject.GetComponent<ComboCardDetector>();
        _turnManager        = _deckObject.transform.GetComponent<TurnManager>();

        if (Camera.main != null) Destroy(Camera.main.gameObject);
        _cameras = Instantiate(_cameras);
        _background = Instantiate(_background);
        Inject(_gameContext);
    }

    #endregion

    #region Debug Mode

    private void RunDebugMode()
    {
        DebugModeCommands();
    }

    private void DebugModeCommands()
    {
        _debugInputActions.TryGetValue(GameCardName.PUNCH, out InputAction spawnPunch);

        spawnPunch.performed += SpawnPunch;
        //SpawnKick();
    }

    private InputActionMap GetDebugInput()
    {
        InputActionAsset asset = Resources.Load<InputActionAsset>("InputSystem_Actions");

        _debugInputActions.Add(GameCardName.PUNCH, asset.FindAction("SpawnPunch"));

        return asset.FindActionMap("Debug");
    }

    private void SpawnPunch(InputAction.CallbackContext ctx)
    {
        _cardDictionary.TryGetValue(GameCardName.PUNCH, out GameCard punch);
        if (punch)
        {
            SpawnCardToHand(punch);
        }
    }

    private void SpawnKick(InputAction.CallbackContext ctx)
    {
        _cardDictionary.TryGetValue(GameCardName.KICK, out GameCard kick);
        if (kick)
        {
            SpawnCardToHand(kick);
        }
    }

    private void SpawnCardToHand(GameCard card)
    {
        int posIndex = _deckManager.FindOpenHandPos();
        if (posIndex >= 0)
        {
            Transform[] positions = _deckManager.Positions;
            GameObject posObj = positions[posIndex].gameObject;
            _deckManager.DealCard(card, posObj.transform.position, posIndex);
        }
    }

    #endregion
}
