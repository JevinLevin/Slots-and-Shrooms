using System.Collections;
using PrimeTween;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Transform recoilRoot;
    [SerializeField] private GunSO gunData;
    public GunSO GunData => gunData;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private LayerMask shotBlockingLayer;
    [SerializeField] private LayerMask shotHitLayer;
    [SerializeField] private GameObject cameraTarget;
    

    private Tween shootDelayTween;
    
    public float GetCurrentWeaponDamage => GunData.baseDamage;
    public bool CanShoot => !shootDelayTween.isAlive;


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

        muzzleFlash.Play();
        shootDelayTween = Tween.Delay(GunData.ShotDelay);
        StartCoroutine(nameof(ApplyRecoil));

        return true;
        
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
