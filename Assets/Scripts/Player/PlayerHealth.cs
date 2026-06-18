using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHasHealth
{
    [SerializeField] private PlayerStatsHolderSO playerStats;
    [SerializeField] private PlayerStatsHolderSO statHolder;
    [SerializeField] private float baseMaxHealth = 25;
    [SerializeField] private float baseRegenRate = 1;
    [SerializeField] private float baseRegenDelay = 1;
    private float maxHealth => baseMaxHealth * playerStats.GetStatAsMultiplier(StatType.MaxHeath);
    private float regenRate => baseRegenRate * playerStats.GetStatAsMultiplier(StatType.RegenRate);
    private float regenDelay => baseRegenDelay * playerStats.GetStatAsMultiplierInverse(StatType.RegenStartDelay);

    public float HealthProgress => Health / maxHealth;

    private bool dead;
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
    public static Action OnPlayerDie;
    public static Action OnTakeDamage;


    private void Start()
    {
        Health = maxHealth;
    }
    

    private void Update()
    {
        regenTimer += Time.deltaTime; 
        if(regenTimer > regenDelay)
        {
            Health = Mathf.Min(maxHealth, health + regenRate);
            regenTimer = 0;
        }
    }

    public void Die()
    {
        OnTakeDamage?.Invoke();
        OnPlayerDie?.Invoke();
        dead = true;
    }

    public void OnHit(float damage, GameObject attacker)
    {
        if (dead)
            return;

        Health -= damage;
        if (Health < 0) Die(); 
        
        OnTakeDamage?.Invoke();
    }
}
