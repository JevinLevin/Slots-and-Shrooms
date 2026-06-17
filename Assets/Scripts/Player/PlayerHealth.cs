using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHasHealth
{
    [SerializeField] private PlayerStatsHolderSO statHolder; 
    [SerializeField] private float maxHealth;
    [SerializeField] private float regenRate; 
    private float health;

    private void Awake()
    {
        maxHealth = statHolder.ReadStat(StatType.MaxHeath).value;
        regenRate = statHolder.ReadStat(StatType.RegenRate).value;

        health = maxHealth;

        EventManager.Instance.statsUpdated += UpdateStats;
    }

    private void UpdateStats(PlayerStat stat)
    {
        if(stat.type == StatType.MaxHeath) maxHealth = statHolder.ReadStat(StatType.MaxHeath).value;
        if(stat.type == StatType.RegenRate) regenRate = statHolder.ReadStat(StatType.RegenRate).value;


    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void OnHit(float damage, GameObject attacker)
    {
        throw new System.NotImplementedException();
    }
}
