using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameContext
{
    public GameManager GameManager;
    public UIManager UIManager;
    public GameObject UICanvas;

    public TurnManager TurnManager;
    public ComboCardDetector ComboCardDetector;
    public GameObject DeckObject;
    public DeckManager DeckManager;
    public EnemyDeckManager EnemyDeckManager;

    public PlayerAnimationControl PlayerAnimationControl;
    public EnemyAnimationControl EnemyAnimationControl;

    public EventManager EventManager;

}
