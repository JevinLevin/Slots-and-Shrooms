using UnityEngine;

public class PlayerStatsHolder : MonoBehaviour
{
    #region Singleton
    public static PlayerStatsHolder Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    [SerializeField] private PlayerStats playerBaseStats;
    public PlayerStats PlayerBaseStats => playerBaseStats; 
    private int health;
    public int Health => health;

    private float speed;
    public float Speed => speed;

    private float attackDamage;
    public float AttackDamage => attackDamage;

    private float attackSpeed;
    public float AttackSpeed => attackSpeed;

    public void AddToHealth(int additionalHealth) { health += additionalHealth; }
    public void AddToSpeed(int additionalSpeed) { speed += additionalSpeed; }
    public void AddToAttackDamage(int additionalAttackDamage) { attackDamage += additionalAttackDamage; }
    public void AddToAttackSpeed(int additionalAttackSpeed) { attackSpeed += additionalAttackSpeed; }
}
