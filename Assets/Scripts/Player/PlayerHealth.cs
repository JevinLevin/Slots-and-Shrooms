using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHasHealth
{
    [SerializeField] private PlayerStatsHolderSO statHolder; 
    private float maxHealth;
    private float regenRate;

    public float HealthProgress => Health / maxHealth;

    private float health;
    public float Health
    {
        get => health;
        set
        {
            health = value;
            OnHealthUpdated?.Invoke(HealthProgress);
        }
    }

    private float regenTimer = 0;

    public static Action<float> OnHealthUpdated;

    private void Start()
    {
        // maxHealth = statHolder.ReadStat(StatType.MaxHeath).value;
        // regenRate = statHolder.ReadStat(StatType.RegenRate).value;

        maxHealth = 100;
        Health = 100;

        //EventManager.Instance.statsUpdated += UpdateStats;
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
            Health += regenRate;
            regenTimer = 0;
        }
    }

    public void Die()
    {
        
    }

    public void OnHit(float damage, GameObject attacker)
    {
        Health -= damage;
        if (Health < 0) Die(); 
    }
}
