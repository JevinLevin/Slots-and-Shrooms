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
    public int magSize = -1;
    public float reloadStartDelay = 0.375f;
    public float reloadDuration = 1f;
    [Tooltip("What % of the reload loop animation to increase ammo count")]
    [Range(0, 1)] public float reloadAddPercentage = 0.5f;
    public int bulletsPerShot = 1;
    public float bulletHipfireSpreadAngleMax = 0;
    public float bulletAimingSpreadAngleMax = 0;
    public float bulletMaxRange = 100;
    public float recoilVerticalAngle = 30f;
    public float recoilRecoveryTime = 0.5f;
    public AnimationCurve recoilCurve;

    public float ShotDelay => 1 / fireRate;
}
