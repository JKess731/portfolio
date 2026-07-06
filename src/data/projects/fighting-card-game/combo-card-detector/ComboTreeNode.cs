using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo Tree Node", menuName = "Turn/Combo Tree Node")]
public class ComboTreeNode : ScriptableObject
{
    [SerializeField] private GameCardName _rootCard;
    [SerializeField] private List<ComboTreeNodeChild> _tree = new List<ComboTreeNodeChild>();

    public List<ComboTreeNodeChild> Tree { get { return _tree; } }
    public GameCardName Root { get { return _rootCard; } }

    [System.Serializable]
    public class ComboTreeNodeChild
    {
        public GameCardName cardName;
        [SerializeReference] public List<ComboTreeNodeChild> tree = new List<ComboTreeNodeChild>();
        public GameCard comboResult = null;
    }
}
