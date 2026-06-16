using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Gun gun;
    [SerializeField] private float aimingFOV = 45f;
    private GunSO currentGun;

    public bool IsHoldingAim => Input.GetMouseButton(1);
    public bool IsHoldingShoot => Input.GetMouseButton(0);

    public bool IsAiming { get; private set; }

    private void Update()
    {
        if(IsHoldingAim && !IsAiming)
            StartAiming();
        else if(!IsHoldingAim && IsAiming)
            StopAiming();
            
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
}
