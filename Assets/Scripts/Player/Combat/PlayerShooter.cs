using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Gun gun;
    [SerializeField] private float aimingFOV = 45f;
    [SerializeField] private LayerMask shotBlockingLayer;
    [SerializeField] private LayerMask shotHitLayer;
    [SerializeField] private GunSO currentGun;


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
    }

    private void StopAiming()
    {
        IsAiming = false;
        playerCamera.ResetFOVOverTime();
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
                ShootBullet(currentGun.bulletSpreadAngleMax);
                shotsLeft--;
            }
        }
        
    }

    private void ShootBullet(float spreadAngleMax = 0)
    {
        Debug.Log("shoot");

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
}
