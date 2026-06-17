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
    [SerializeField] private Gun pistol;
    [SerializeField] private Gun shotgun;

    [Header("Attributes")]
    [SerializeField] private float aimingFOV = 45f;
    
    private Gun currentGun;
    
    public bool IsAiming { get; private set; }
    public bool IsHoldingAim => Input.GetMouseButton(1);
    public bool IsHoldingShoot => Input.GetMouseButton(0);
    public bool IsSwitchingPrimary => !HandsDisabled && (Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(KeyCode.Alpha1));
    public bool IsSwitchingSecondary => !HandsDisabled && (Input.mouseScrollDelta.y > 0 || Input.GetKeyDown(KeyCode.Alpha2));

    public bool HandsDisabled { get; private set; }


    private void Start()
    {
        pistol.ToggleGun(false);
        shotgun.ToggleGun(true);
        currentGun = shotgun;
    }

    private void OnEnable()
    {
        SlotMachine.OnSlotMachineStartSpinning += DisableHands;
        SlotMachine.OnSlotMachineStopSpinning += EnableHands;
    }
    private void OnDisable()
    {
        SlotMachine.OnSlotMachineStartSpinning -= DisableHands;
        SlotMachine.OnSlotMachineStopSpinning -= EnableHands;
    }

    private void Update()
    {
        // Weapon swapping
        if(IsSwitchingPrimary)
            SwapWeapon(shotgun);
        if(IsSwitchingSecondary)
            SwapWeapon(pistol);
        
        
        if(IsHoldingAim && !IsAiming)
            StartAiming();
        else if(!IsHoldingAim && IsAiming)
            StopAiming();

        if (IsHoldingShoot)
            TryShoot();

    }

    private void TryShoot()
    {
        bool shootSuccessful = currentGun.TryShoot(IsAiming);

        if (shootSuccessful)
        {
            playerAnimator.PlayShoot(IsAiming);
        }
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

    private void SwapWeapon(Gun newGun)
    {
        if (newGun == currentGun)
            return;
        
        currentGun.ToggleGun(false);

        currentGun = newGun;
        currentGun.ToggleGun(true);
    }

    private void DisableHands()
    {
        currentGun.ToggleGun(false);
        HandsDisabled = true;
    }

    private void EnableHands()
    {
        currentGun.ToggleGun(true);
        HandsDisabled = false;
    }


}
