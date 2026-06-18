using EditorAttributes;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun")]
public class GunSO : ScriptableObject
{
    [Header("Visuals")]
    public Sprite gunIcon;
    public string gunName;
    public string gunDesc;

    [Header("SoundEffects")]
    public AudioClip shootSoundEffect;


    [Header("Functionality")]
    public float baseDamage = 1;
    [Tooltip("Bullets fired per second")]
    public float fireRate = 10;

    public int startingAmmo = 15;
    public int magSize = -1;
    public bool overheat;
    [ShowField(nameof(overheat))] public float overheatMax = 20;
    [ShowField(nameof(overheat))] public float overheatPerShot = 2;
    [ShowField(nameof(overheat))] public float overheatDrainPerSecond = 2;
    [ShowField(nameof(overheat))] public float overheatedDrainPerSecond = 3;
    [HideField(nameof(overheat))] public float reloadStartDelay = 0.375f;
    [HideField(nameof(overheat))] public float reloadDuration = 1f;
    [Tooltip("What % of the reload loop animation to increase ammo count")]
    [HideField(nameof(overheat))][Range(0, 1)] public float reloadAddPercentage = 0.5f;
    public int bulletsPerShot = 1;
    public float bulletHipfireSpreadAngleMax = 0;
    public float bulletAimingSpreadAngleMax = 0;
    public float bulletMaxRange = 100;
    public float recoilVerticalAngle = 30f;
    public float recoilRecoveryTime = 0.5f;
    public AnimationCurve recoilCurve;

    public float ShotDelay => (1 / fireRate ) * GetFireRateMultiplier;

    [Header("Stats")] 
    public PlayerStatsHolderSO playerStats;
    public StatType damageStat;
    public StatType fireRateStat;
    public StatType recoilStat;
    public StatType overheatMaxStat;
    public StatType overheatRegenStat;
    public StatType spreadStat;
    public StatType bulletCountStat;
    public StatType magStat;
    public float GetDamageMultiplier => playerStats.GetStatAsMultiplier(damageStat);
    public float GetFireRateMultiplier => playerStats.GetStatAsMultiplierInverse(fireRateStat);
    public float GetRecoilMultiplier => playerStats.GetStatAsMultiplier(recoilStat);
    public float GetOverheatMaxMultiplier => playerStats.GetStatAsMultiplier(overheatMaxStat);
    public float GetOverheatRegenMultiplier => playerStats.GetStatAsMultiplier(overheatRegenStat);
    public float GetSpreadMultiplier => playerStats.GetStatAsMultiplier(spreadStat);
    public float GetBulletCountMultiplier => playerStats.GetStatAsMultiplier(bulletCountStat);
    public float GetMagMultiplier => playerStats.GetStatAsMultiplier(magStat);
}
