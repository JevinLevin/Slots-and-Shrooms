using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using System.Collections;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private Transform recoilRoot;
    [SerializeField] private Gun gun;
    [SerializeField] private GunSO currentGun;

    [Header("Attributes")]
    [SerializeField] private float aimingFOV = 45f;
    [SerializeField] private LayerMask shotBlockingLayer;
    [SerializeField] private LayerMask shotHitLayer;


    private Tween shootDelayTween;
    
    public bool IsAiming { get; private set; }
    public bool IsHoldingAim => Input.GetMouseButton(1);
    public bool IsHoldingShoot => Input.GetMouseButton(0);

    public bool CanShoot => !shootDelayTween.isAlive;
    
    private void Update()
    {
        if(IsHoldingAim && !IsAiming)
            StartAiming();
        else if(!IsHoldingAim && IsAiming)
            StopAiming();

        if (IsHoldingShoot && CanShoot)
            Shoot();

    }

    private void StartAiming()
    {
        IsAiming = true;
        playerCamera.SetFOVOverTime(aimingFOV, 0f);
        playerAnimator.ToggleAiming(true);
    }

    private void StopAiming()
    {
        IsAiming = false;
        playerCamera.ResetFOVOverTime();
        playerAnimator.ToggleAiming(false);
    }

    private void Shoot()
    {
        if (currentGun.bulletsPerShot == 1)
        {
            ShootBullet();
        }
        else
        {
            int shotsLeft = currentGun.bulletsPerShot;
            while (shotsLeft > 0)
            {
                float spreadAngle = IsAiming ? currentGun.bulletAimingSpreadAngleMax : currentGun.bulletHipfireSpreadAngleMax;
                ShootBullet(spreadAngle);
                shotsLeft--;
            }
        }

        shootDelayTween = Tween.Delay(currentGun.ShotDelay);
        StartCoroutine(nameof(ApplyRecoil));
        playerAnimator.PlayShoot(IsAiming);
        
    }

    private void ShootBullet(float spreadAngleMax = 0)
    {
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
        Debug.DrawLine(bulletOrigin, bulletOrigin + (bulletDirection*currentGun.bulletMaxRange), Color.red, 3);
        
        Ray bulletRay = new Ray(bulletOrigin, bulletDirection);
        if (Physics.Raycast(bulletRay, out var bulletHit, currentGun.bulletMaxRange, shotHitLayer))
        {
            // Check for blocking objects
            if (Physics.Linecast(bulletOrigin, bulletHit.point, shotBlockingLayer))
                return;
        }
    }

    private IEnumerator ApplyRecoil()
    {
        float recoilDuration = Mathf.Max(currentGun.recoilRecoveryTime, currentGun.ShotDelay);
        float recoilTime = 0;

        while(recoilTime < recoilDuration)
        {
            float t = recoilTime / recoilDuration;
            float recoilPower = currentGun.recoilCurve.Evaluate(t) * currentGun.recoilVerticalAngle;

            // Apply current recoil this frame to tranform
            recoilRoot.localRotation = Quaternion.AngleAxis(recoilPower, Vector3.left);

            recoilTime += Time.deltaTime;

            yield return null;
        }
    }
}
