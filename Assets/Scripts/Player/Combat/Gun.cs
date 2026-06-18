using System;
using System.Collections;
using PrimeTween;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    private static readonly int OverheatProgress1 = Shader.PropertyToID("_OverheatProgress");
    
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Transform recoilRoot;
    [SerializeField] private GunSO gunData;
    public GunSO GunData => gunData;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private LayerMask shotBlockingLayer;
    [SerializeField] private LayerMask shotHitLayer;
    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private Material gunMaterial;
    [SerializeField] private ParticleSystem overheatParticle;
    [SerializeField] private ParticleSystem bulletImpactParticle;


    private Tween shootDelayTween;
    private int magAmmo;
    private int totalAmmo;
    private float overheatValue;
    
    public float GetCurrentWeaponDamage => GunData.baseDamage;
    public bool CanShoot => !shootDelayTween.isAlive && !IsOverheated;
    public bool HasAmmo => gunData.magSize == -1 || magAmmo > 0;
    public int GetMagAmmo => magAmmo;
    public int GetTotalAmmo => totalAmmo;
    public float OverheatProgress => overheatValue / gunData.overheatMax;
    public bool IsOverheated { get; private set; }
    
    public static Action<int, int> OnGunAmmoChanged;
    public static Action OnGunOverheatStart;
    public static Action OnGunOverheatEnd;
    public static Action<float> OnGunOverheatUpdate;

    private void Awake()
    {
        totalAmmo = gunData.startingAmmo - gunData.magSize;
        magAmmo = gunData.magSize;
    }

    private void Update()
    {
        if(gunData && gunData.overheat)
            UpdateOverheat();
    }

    private void UpdateOverheat()
    {
        float drainValue = IsOverheated ? gunData.overheatedDrainPerSecond : gunData.overheatDrainPerSecond;
        overheatValue = Mathf.Max(0, overheatValue - (drainValue * Time.deltaTime));
        gunMaterial.SetFloat(OverheatProgress1, OverheatProgress);
        OnGunOverheatUpdate?.Invoke(OverheatProgress);
    }

    public void ToggleGun(bool value)
    {
        transform.localScale = value ? Vector3.one : Vector3.zero;
        cameraTarget.SetActive(value);
    }

    public bool TryShoot(bool isAiming)
    {
        if (!CanShoot)
            return false;

        if (GunData.bulletsPerShot == 1)
        {
            ShootBullet();
        }
        else
        {
            int shotsLeft = GunData.bulletsPerShot;
            while (shotsLeft > 0)
            {
                float spreadAngle = isAiming ? GunData.bulletAimingSpreadAngleMax : GunData.bulletHipfireSpreadAngleMax;
                ShootBullet(spreadAngle);
                shotsLeft--;
            }
        }

        if(!gunData.overheat)
            AdjustMagAmmo(-1);
        else
            AddOverheat();
        muzzleFlash.Play();
        shootDelayTween = Tween.Delay(GunData.ShotDelay);
        StartCoroutine(nameof(ApplyRecoil));

        return true;
        
    }

    public void AdjustMagAmmo(int amount)
    {
        if (amount < 0)
        {
            magAmmo += amount;
        }
        else
        {
            if (amount > totalAmmo)
            {
                magAmmo += totalAmmo;
                totalAmmo -= totalAmmo;
            }
            else
            {
                magAmmo += amount;
                totalAmmo -= amount;
            }
        }
        OnGunAmmoChanged?.Invoke(magAmmo, totalAmmo);
    }

    public void AddTotalAmmo(int amount)
    {
        totalAmmo += amount;
        OnGunAmmoChanged?.Invoke(magAmmo, totalAmmo);

    }

    public void AddOverheat()
    {
        overheatValue += gunData.overheatPerShot;
        if (OverheatProgress >= 1)
        {
            StartCoroutine(nameof(Overheated));
        }
    }

    private IEnumerator Overheated()
    {
        IsOverheated = true;
        OnGunOverheatStart?.Invoke();

        overheatParticle.Play();

        while (overheatValue > 0)
        {
            yield return null;
        }
        
        IsOverheated = false;
        OnGunOverheatEnd?.Invoke();
    }

    private void ShootBullet(float spreadAngleMax = 0)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = gunData.shootSoundEffect;
        audioSource.Play();

        Vector3 bulletDirection = playerCamera.transform.forward;
        if (spreadAngleMax > 0)
        {
            Vector2 spreadAngle = Random.insideUnitCircle * Random.Range(-spreadAngleMax, spreadAngleMax);

        
            // Add spread
            bulletDirection = Quaternion.AngleAxis(spreadAngle.x, playerCamera.transform.up) * bulletDirection;
            bulletDirection = Quaternion.AngleAxis(spreadAngle.y, playerCamera.transform.right) * bulletDirection;
        }


        Vector3 bulletOrigin = playerCamera.transform.position;
        
        // Draw the raycast for debugging
        Debug.DrawLine(bulletOrigin, bulletOrigin + (bulletDirection*GunData.bulletMaxRange), Color.red, 3);
        
        Ray bulletRay = new Ray(bulletOrigin, bulletDirection);
        if (Physics.Raycast(bulletRay, out var bulletHit, GunData.bulletMaxRange, shotHitLayer))
        {

            // Check for blocking objects
            if (Physics.Linecast(bulletOrigin, bulletHit.point, shotBlockingLayer))
                return;

            if (bulletHit.collider.TryGetComponent<IHasHealth>(out var enemyHealth))
            {
                enemyHealth.OnHit(GetCurrentWeaponDamage, playerCamera.gameObject);
                Instantiate(bulletImpactParticle, bulletHit.point, Quaternion.identity, null);
            }
        }
    }

    private IEnumerator ApplyRecoil()
    {
        float recoilDuration = Mathf.Max(GunData.recoilRecoveryTime, GunData.ShotDelay);
        float recoilTime = 0;

        while(recoilTime < recoilDuration)
        {
            float t = recoilTime / recoilDuration;
            float recoilPower = GunData.recoilCurve.Evaluate(t) * GunData.recoilVerticalAngle;

            // Apply current recoil this frame to tranform
            recoilRoot.localRotation = Quaternion.AngleAxis(recoilPower, Vector3.left);

            recoilTime += Time.deltaTime;

            yield return null;
        }
    }
}
