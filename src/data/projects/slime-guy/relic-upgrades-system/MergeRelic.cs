using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MergeRelic : RelicSO
{
    [SerializeField] protected List<RelicSO> required = new List<RelicSO>();
    [SerializeField] protected List<MergeStats> stats = new List<MergeStats>();

    public List<RelicSO> RequiredRelics { get { return required; } }

    public abstract void InitMerge(StatSO playerstats);
}

[Serializable]
public class MergeStats
{
    public Enum_Stats stat;
    public float val;
    public StatsType type;
}
