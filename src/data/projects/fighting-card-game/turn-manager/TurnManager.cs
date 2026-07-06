using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour, IManager
{
    public static TurnManager Instance;

    [SerializeField] private GameObject _wideCamera;
    [SerializeField] private float _drawPhaseTimer = 2f;
    [SerializeField] private float _playCardTimer = 5f;

    private TurnPhase _currentPhase;
    private int _turnCount = 0;

    private GameManager _gameManager;
    private EventManager _em;
    private DeckManager _dm;
    private EnemyDeckManager _edm;
    private UIManager _uim;
    private PlayerAnimationControl _pac;
    private EnemyAnimationControl _eac;
    private ComboCardDetector _ccd;

    private Transform _playerTransform;
    private Transform _enemyTransform;

    private float _turnTimer = 0f;
    private bool _initialized = false;
    private bool _isTurnStarted = false;
    private bool _isCardPlayable = false;
    private Coroutine _enemyPlayRoutine;

    private Queue<CardQueueItem> _cardQueue;

    public UIManager UIManager { get { return _uim; } }
    public TurnPhase CurrentPhase { get { return _currentPhase; } }
    public bool IsCardPlayable { get { return _isCardPlayable; } set { _isCardPlayable = value; } }
    public bool IsTurnStarted { get { return _isTurnStarted; } }
    public float TurnTimer { get { return _turnTimer; } }
    public Transform EnemyTransform { get { return _enemyTransform; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    private void Start()
    {
        _turnCount = 0;
        //StartCoroutine(InitializeGame());
    }

    private void Update()
    {
        if (_initialized && _currentPhase == TurnPhase.PLAY)
        {
            if (_cardQueue.Count > 0 && _pac.CurrentState == AnimationState.IDLE)
                ExecuteNextCard();
        }
    }

    #region Initialization

    public IEnumerator Initialize(GameContext ctx)
    {
        Inject(ctx);

        _em.OnTurnStart.AddListener(TurnStart);
        _em.OnTurnEnd.AddListener(TurnEnd);

        _cardQueue = new Queue<CardQueueItem>();

        _wideCamera = GameObject.Find("Wide Camera");
        _initialized = true;
        yield return null;
    }

    public void Inject(GameContext ctx)
    {
        _gameManager = ctx.GameManager;
        _dm = ctx.DeckManager;
        _edm = ctx.EnemyDeckManager;
        _em = ctx.EventManager;
        _pac = ctx.PlayerAnimationControl;
        _ccd = ctx.ComboCardDetector;
        _eac = ctx.EnemyAnimationControl;
        _uim = ctx.UIManager;

        _playerTransform = _pac.transform;
        _enemyTransform = _eac.transform;
    }

    #endregion

    #region Turn Event Logic

    private void TurnStart()
    {
        if (_dm.DeckQueue.Count <= 0)
            _dm.ShuffleDeck();

        _turnCount++;
        StartCoroutine(RunRound());
    }

    private void TurnEnd()
    {
        _ccd?.ClearCards();
        _cardQueue.Clear();
        _em.OnTurnStart?.Invoke();
    }

    #endregion

    #region Turn Control

    public IEnumerator RunRound()
    {
        if (_gameManager.DebugMode == false)
            yield return StartCoroutine(DrawPhase());
        yield return StartCoroutine(PlayPhase());
        yield return StartCoroutine(ResolvePhase());

        _em.OnTurnEnd?.Invoke();
    }

    private IEnumerator DrawPhase()
    {
        _currentPhase = TurnPhase.DRAW;
        _isTurnStarted = false;
        _isCardPlayable = false;
        _turnTimer = _drawPhaseTimer;
        _uim.UpdateTimerText(_turnTimer.ToString("F2"));

        if (_dm.CardsInHand > 0)
            yield return RemoveCardsFromHand();

        if (_dm.DeckQueue.Count <= 0)
            _dm.ShuffleDeck();

        if (_edm != null)
            _edm.DrawHand(_dm.CardsToDraw);

        _wideCamera.SetActive(false);
        yield return StartCoroutine(DealPlayerHand());

        float elapsed = 0f;
        while (elapsed < _drawPhaseTimer)
        {
            yield return null;
            elapsed += Time.deltaTime;
            _turnTimer = Mathf.Max(0f, _drawPhaseTimer - elapsed);
            _uim.UpdateTimerText(_turnTimer.ToString("F2"));
        }
    }

    private IEnumerator PlayPhase()
    {
        _currentPhase = TurnPhase.PLAY;
        _isTurnStarted = true;
        _isCardPlayable = true;
        _turnTimer = _playCardTimer;
        _uim.UpdateTimerText(_turnTimer.ToString("F2"));

        if (_turnCount == 1)
        {
            _pac.OnLungeStart(); // Trigger lunge anim
            _eac?.OnLungeStart();

        }

        if (_edm != null)
            _enemyPlayRoutine = StartCoroutine(_edm.PlayHandRandomly());

        if (_gameManager.DebugMode == false)
        {

            float elapsed = 0f;
            while (elapsed < _playCardTimer)
            {
                yield return null;
                elapsed += Time.deltaTime;
                _turnTimer = Mathf.Max(0f, _playCardTimer - elapsed);
                _uim.UpdateTimerText(_turnTimer.ToString("F2"));
            }

            _isTurnStarted = false;
            _isCardPlayable = false;
            _turnTimer = 0f;
            _uim.UpdateTimerText(_turnTimer.ToString("F2"));
        }
        else
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private IEnumerator ResolvePhase()
    {
        _currentPhase = TurnPhase.RESOLVE;
        yield return ResolvePlayerCards();

        if (_enemyPlayRoutine != null)
        {
            yield return _enemyPlayRoutine;
            _enemyPlayRoutine = null;
        }
    }

    private IEnumerator ResolvePlayerCards()
    {
        while (_cardQueue.Count > 0 || _pac.CurrentState == AnimationState.ANIMATING)
        {
            if (_cardQueue.Count > 0 && _pac.CurrentState == AnimationState.IDLE)
                ExecuteNextCard();
            yield return null;
        }
    }

    private IEnumerator DealPlayerHand()
    {
        Transform[] positions = _dm.Positions;
        int count = Mathf.Min(_dm.CardsToDraw, positions.Length);
        for (int i = 0; i < count; ++i)
        {
            if (_dm.DeckQueue.Count == 0) break;
            GameCard card = _dm.DeckQueue.Dequeue();
            _dm.DealCard(card, positions[i].position, i);
            yield return new WaitForSeconds(0.25f);
        }
    }

    public IEnumerator RemoveCardsFromHand()
    {
        GameObject[] cards = GameObject.FindGameObjectsWithTag(TagManager.GetTag(EnumTag.GameCard));
        foreach (GameObject c in cards) { Destroy(c); }
        _dm.CardsInHand = 0;
        _dm.ResetHandPositions();
        yield return null;
    }

    #endregion

    #region Queue Control

    public void QueueCard(CardQueueItem item)
    {
        _cardQueue.Enqueue(item);
    }

    public void ExecuteNextCard()
    {
        if (_pac.CurrentState != AnimationState.IDLE) return;
        if (_cardQueue.Count == 0) return;

        Debug.Log(Vector2.Distance(_playerTransform.position, _enemyTransform.position));
        if (Vector2.Distance(_playerTransform.position, _enemyTransform.position) > 3f)
        {
            _pac.ComboMaxMoveFrames = 6;
            _pac.ComboMaxMoveFrames = 1;
            _pac.StartMoving(3f);
            return;
        }

        CardQueueItem item = _cardQueue.Dequeue();
        item.card.ExecuteEffect();
        StartCoroutine(_pac.Animate(item.animationType));
    }

    #endregion
}

public enum TurnPhase
{
    DRAW,
    PLAY,
    RESOLVE
}

public class CardQueueItem
{
    public AnimationType animationType;
    public GameCard card;

    public CardQueueItem(GameCard c)
    {
        animationType = c.CardAnimationType;
        card = c;
    }
}
