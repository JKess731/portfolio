using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class DeckManager : MonoBehaviour, IManager
{
    public static DeckManager Instance { get; private set; }
    private TurnManager _tm;
    private GameManager _gameManager;
    private UIManager _uiManager;

    [SerializeField] private int _totalEachCardType = 3;
    [SerializeField] private int _cardsToDraw = 5;

    private List<GameCard> _deck;
    private List<GameCard> _allCards;
    private int _cardsInHand = 0;

    private Queue<GameCard> _deckQueue;

    private GameObject _currentSelectedCard;
    private Vector2 _currentSelectedCardPreviousPos;
    private bool _isDraggingCard = false;

    private Transform[] _handPositions;
    private List<HandCardPosition> _handPositionItems;
    private Camera _camera;

    public GameObject CurrentCard { get { return _currentSelectedCard; } }
    public Queue<GameCard> DeckQueue { get { return _deckQueue; } }
    public int CardsInHand { get { return _cardsInHand; } set { _cardsInHand = value; } }
    public int CardsToDraw { get { return _cardsToDraw; } }
    public Transform[] Positions { get { return _handPositions; } }
    public List<HandCardPosition> PositionItems { get { return _handPositionItems; } }
    public List<GameCard> AllCards { get { return _allCards; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        _camera = Camera.main;
    }

    public IEnumerator Initialize(GameContext ctx)
    {
        Inject(ctx);

        _allCards = LoadCardsFromResources();
        _deck = new List<GameCard>();
        _deckQueue = new Queue<GameCard>();

        yield return _handPositions = UpdatePositionsArray();
        _handPositionItems = new List<HandCardPosition>();
        for (int i = 0; i < _handPositions.Length; ++i)
            _handPositionItems.Add(new HandCardPosition(false, i));

        CreateDeck();
        ShuffleDeck();

        yield return null;
    }

    public void Inject(GameContext ctx)
    {
        _gameManager = ctx.GameManager;
        _tm = ctx.TurnManager;
        _uiManager = ctx.UIManager;
    }

    void Update()
    {
        if (_gameManager != null)
            IsClickingCard();
    }

    #region STAGE 0 - LOAD

    public void CreateDeck()
    {
        for (int i = 0; i < _allCards.Count; ++i)
        {
            for (int j = 0; j < _totalEachCardType; ++j)
                _deck.Add(Instantiate(_allCards[i]));
        }
    }

    private List<GameCard> LoadCardsFromResources()
    {
        List<GameCard> cards = Resources.LoadAll<GameCard>("Scriptable Objects/GameCards").ToList<GameCard>();
        cards.RemoveAll(c => c.GameCardName == GameCardName.COMBO_PUNCH_KICK);
        return cards;
    }

    public void ShuffleDeck()
    {
        System.Random r = new System.Random();
        int n = _deck.Count;
        while (n > 1)
        {
            n--;
            int k = r.Next(n + 1);
            (_deck[n], _deck[k]) = (_deck[k], _deck[n]);
        }

        for (int i = 0; i < _deck.Count; ++i)
            _deckQueue.Enqueue(_deck[i]);
    }

    #endregion

    #region STAGE 1 - INTERACTION

    private void IsClickingCard()
    {
        //if (_camera == null || !_camera.isActiveAndEnabled) _camera = Camera.main;
        if (_tm.IsTurnStarted)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.value);
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);
                RaycastHit2D hit = System.Array.Find(hits, h => h.collider != null && h.collider.CompareTag(TagManager.GetTag(EnumTag.GameCard)));

                if (hit.collider != null)
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    _currentSelectedCard = clickedObject;
                    _currentSelectedCardPreviousPos = clickedObject.transform.position;
                    _isDraggingCard = true;
                    DragCardWithMouse();
                }
            }
            else if (Mouse.current.leftButton.isPressed && _currentSelectedCard != null)
            {
                DragCardWithMouse();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame && _currentSelectedCard != null)
            {
                PlayCurrentCard();
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && _currentSelectedCard != null)
        {
            _currentSelectedCard.transform.position = _currentSelectedCardPreviousPos;
            _currentSelectedCard = null;
        }
    }

    private void DragCardWithMouse()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.value);
        _currentSelectedCard.transform.position = mousePos;
        if (!_isDraggingCard) _isDraggingCard = true;
    }

    private void PlayCurrentCard()
    {
        if (_tm.IsCardPlayable)
        {
            GameCard card = _currentSelectedCard.GetComponent<CardVisual>().GameCard;
            CardQueueItem newItem = new CardQueueItem(card);
            _tm.QueueCard(newItem);

            int posIndex = _currentSelectedCard.GetComponent<CardVisual>().PositionIndex;
            UpdateHandPositionItem(posIndex, false, null);

            ComboCardDetector.Instance.RegisterCard(card.GameCardName);

            Destroy(_currentSelectedCard);
            _currentSelectedCard = null;
            _cardsInHand--;
        }
        _isDraggingCard = false;
        _tm.IsCardPlayable = false;
    }

    public GameObject DealCard(GameCard card, Vector2 pos, int posIndex)
    {
        GameObject cardPrefab = card.CardPrefab;
        GameObject instance = Instantiate(cardPrefab, pos, Quaternion.identity);
        CardVisual visual = instance.GetComponent<CardVisual>();
        visual.Initialize(card);
        visual.PositionIndex = posIndex;
        _cardsInHand++;
        UpdateHandPositionItem(posIndex, true, card);

        _uiManager.UpdateDeckCountText("Cards Left: " + _deckQueue.Count.ToString());
        return cardPrefab;
    }

    #endregion

    #region Hand Positions

    private Transform[] UpdatePositionsArray()
    {
        GameObject hand = null;
        foreach (Transform t in transform)
        {
            if (t.CompareTag(TagManager.GetTag(EnumTag.CurrentHand)))
            {
                hand = t.gameObject;
                break;
            }
        }

        if (hand == null)
        {
            Debug.LogError("No hand object found in DeckManager children.");
            return null;
        }

        return hand.transform.Cast<Transform>().ToArray();
    }

    private void UpdateHandPositionItem(int posIndex, bool hasCard, GameCard card)
    {
        _handPositionItems[posIndex].HasCard = hasCard;
        _handPositionItems[posIndex].Card = card;
    }

    public void ResetHandPositions()
    {
        for (int i = 0; i < _handPositionItems.Count; ++i)
        {
            _handPositionItems[i].HasCard = false;
            _handPositionItems[i].Card = null;
        }
    }

    public int FindOpenHandPos()
    {
        for (int i = 0; i < PositionItems.Count; ++i)
        {
            HandCardPosition item = PositionItems[i];
            if (!item.HasCard)
            {
                return item.PositionIndex;
            }
        }

        return -1;
    }

    #endregion
}

public class HandCardPosition
{
    public bool HasCard { get; set; }
    public GameCard Card { get; set; }
    public int PositionIndex { get; set; }

    public HandCardPosition(bool hasCard, int positionIndex)
    {
        HasCard = hasCard;
        PositionIndex = positionIndex;
    }
}
