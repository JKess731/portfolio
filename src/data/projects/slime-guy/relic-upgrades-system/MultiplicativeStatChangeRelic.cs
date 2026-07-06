using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Multiplicative Relic", menuName = "NewRelics/MultiplicativeRelics")]
public class MultiplicativeStatChangeRelic : RelicSO
{
    [SerializeField] List<Enum_Stats> changedStats;
    [SerializeField] List<float> changedValues;

    public override void OnPickup()
    {
        ActivateBuffs();
    }

    public override void OnDrop()
    {
        DeactivateBuffs();
    }

    public override void ActivateBuffs()
    {
        for (int i = 0; i < changedStats.Count; i++)
        {
            if(changedStats[i] == Enum_Stats.HEALTH)
            {
                _playerStats.AddStat(Enum_Stats.HEALTH, changedValues[i]);
                UiManager.Instance.UpdateHealthBar(_playerStats.GetStat(Enum_Stats.HEALTH), _playerStats.ModifiedStatValue(Enum_Stats.MAXHEALTH));
            }
            Debug.Log(_playerStats);
            _playerStats.RegisterStat(changedStats[i], StatsType.MULTIPLICATIVE, changedValues[i] / 100);
        }
    }

    public void DeactivateBuffs()
    {
        for (int i = 0; i < changedStats.Count; i++)
        {
            if (changedStats[i] == Enum_Stats.HEALTH)
            {
                _playerStats.SubtractStat(Enum_Stats.HEALTH, changedValues[i]);
                UiManager.Instance.UpdateHealthBar(_playerStats.GetStat(Enum_Stats.HEALTH), _playerStats.ModifiedStatValue(Enum_Stats.MAXHEALTH));
            }
            _playerStats.RegisterStat(changedStats[i], StatsType.MULTIPLICATIVE, -changedValues[i] / 100);
            Debug.Log(_playerStats.GetStat(changedStats[i]));
        }
    }

    public override void ResetRelic()
    {
        throw new System.NotImplementedException();
    }
}
