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
    public float magSize = 16;
    public int bulletsPerShot = 1;
    public float bulletHipfireSpreadAngleMax = 0;
    public float bulletAimingSpreadAngleMax = 0;
    public float bulletMaxRange = 100;
    public float recoilVerticalAngle = 30f;
    public float recoilRecoveryTime = 0.5f;
    public AnimationCurve recoilCurve;

    public float ShotDelay => 1 / fireRate;
}
