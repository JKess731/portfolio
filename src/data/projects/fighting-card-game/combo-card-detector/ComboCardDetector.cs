using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCardDetector : MonoBehaviour, IManager
{
    public static ComboCardDetector Instance;

    [SerializeField] private float _comboBufferTime = .25f;
    [SerializeField] private List<ComboDictionary> _comboTrees = new List<ComboDictionary>();

    private Queue<GameCardName> _playedCardQueue;
    private Dictionary<GameCardName, ComboTreeNode> _comboDictionary;

    private GameManager _gameManager;
    private DeckManager _deckManager;

    [System.Serializable]
    public class ComboDictionary
    {
        public GameCardName cardName;
        public ComboTreeNode comboTree;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public IEnumerator Initialize(GameContext ctx)
    {
        Inject(ctx);

        _playedCardQueue = new Queue<GameCardName>();
        _comboDictionary = new Dictionary<GameCardName, ComboTreeNode>();

        PopulateDictionary();

        yield return null;
    }

    public void Inject(GameContext ctx)
    {
        _gameManager = ctx.GameManager;
        _deckManager = ctx.DeckManager;
    }

    private void PopulateDictionary()
    {
        for (int i = 0; i < _comboTrees.Count; ++i)
        {
            _comboDictionary.Add(_comboTrees[i].cardName, _comboTrees[i].comboTree);
        }
    }

    public void RegisterCard(GameCardName gcn)
    {
        _playedCardQueue.Enqueue(gcn);

        GameCard comboCard = SearchGameCardTree();
        if (comboCard != null)
        {
            StopCoroutine(StartBufferTimer());
            AddComboCardToHand(comboCard);
            StartBufferTimer();
            //ClearCards();
        }
    }

    public GameCard SearchGameCardTree()
    {
        GameCardName[] cards = _playedCardQueue.ToArray();
        if (_playedCardQueue.Count == 0) return null;
        if (!_comboDictionary.ContainsKey(cards[0])) return null;
        return SearchTreeChildren(_comboDictionary[cards[0]].Tree, cards, 1);
    }

    public GameCard SearchTreeChildren(List<ComboTreeNode.ComboTreeNodeChild> tree, GameCardName[] cards, int depth)
    {
        // Loop through children
        for (int i = 0; i < tree.Count; ++i)
        {
            if (depth <= cards.Length - 1 && tree[i].cardName == cards[depth])
            {
                if (tree[i].comboResult != null && _playedCardQueue.Count == depth + 1) return tree[i].comboResult;
                GameCard card = SearchTreeChildren(tree[i].tree, cards, depth + 1);
                if (card != null) return card;
            }
        }

        return null;
    }

    public void ClearCards()
    {
        _playedCardQueue.Clear();
    }

    public void AddComboCardToHand(GameCard card)
    {
        int posIndex = _deckManager.FindOpenHandPos();
        if (posIndex >= 0)
        {
            Transform[] positions = _deckManager.Positions;
            GameObject posObj = positions[posIndex].gameObject;
            _deckManager.DealCard(card, posObj.transform.position, posIndex);
        }
    }

    private IEnumerator StartBufferTimer()
    {
        yield return new WaitForSeconds(_comboBufferTime);
        _playedCardQueue.Clear();
    }
}
