using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHasHealth
{
    [SerializeField] private PlayerStatsHolderSO statHolder; 
    private float maxHealth;
    private float regenRate; 
    private float health;

    private float regenTimer = 0; 

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

    private void Update()
    {
        regenTimer += Time.deltaTime; 
        if(regenTimer > 1)
        {
            health += regenRate;
            regenTimer = 0;
        }
    }

    public void Die()
    {
        
    }

    public void OnHit(float damage, GameObject attacker)
    {
        health -= damage;
        if (health < 0) Die(); 
    }
}
