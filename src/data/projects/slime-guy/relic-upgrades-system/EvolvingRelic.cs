using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EvolvingRelic : RelicSO
{
    [SerializeField] protected RelicSO evolvesInto;
    protected PlayerStateMachine psm;

    public override void Initialize(StatSO playerstats)
    {
        _playerStats = playerstats;
        relicDescriptionOrig = relicDescription;
        psm = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
    }

    protected void Evolve()
    {
        RelicManager.Instance.RemoveRelic(this);
        RelicSO evolve = evolvesInto;
        RelicManager.Instance.AddRelic(evolve);
        RelicManager.Instance.AddToNonPool(evolve);
        evolve.OnPickup();
    }
}
