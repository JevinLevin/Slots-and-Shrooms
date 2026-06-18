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
    public bool IsHoldingShoot => !HandsDisabled && Input.GetMouseButton(0);
    public bool IsSwitchingPrimary => !HandsDisabled && (Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(KeyCode.Alpha1));
    public bool IsSwitchingSecondary => !HandsDisabled && (Input.mouseScrollDelta.y > 0 || Input.GetKeyDown(KeyCode.Alpha2));
    public bool IsPressingReload => Input.GetKeyDown(KeyCode.R);
    public bool IsReloading { get; private set; }

    public bool HandsDisabled { get; private set; }

    public static Action<Gun, int, int> OnGunSwapped;
    


    private void Start()
    {
        pistol.ToggleGun(false);
        shotgun.ToggleGun(true);
        currentGun = shotgun;
        OnGunSwapped?.Invoke(currentGun, currentGun.GetMagAmmo, currentGun.GetTotalAmmo);
    }

    private void OnEnable()
    {
        SlotMachine.OnSlotMachineStartSpinning += DisableHands;
        SlotMachine.OnSlotMachineDeactivate += EnableHands;
    }
    private void OnDisable()
    {
        SlotMachine.OnSlotMachineStartSpinning -= DisableHands;
        SlotMachine.OnSlotMachineDeactivate -= EnableHands;
    }

    private void Update()
    {
        // Weapon swapping
        if(IsSwitchingPrimary)
            SwapWeapon(shotgun);
        if(IsSwitchingSecondary)
            SwapWeapon(pistol);
        
        
        if(IsHoldingAim && !IsAiming && !IsReloading)
            StartAiming();
        else if(!IsHoldingAim && IsAiming)
            StopAiming();

        if (IsHoldingShoot && !IsReloading)
            TryShoot();
        
        if(IsPressingReload && !IsReloading && currentGun.GetMagAmmo < currentGun.GetMagSize)
            StartReload();

    }

    private void TryShoot()
    {

        // If they have ammo
        if(currentGun.HasAmmo)
        {
            bool shootSuccessful = currentGun.TryShoot(IsAiming);

            if (shootSuccessful)
            {
                
                
                playerAnimator.PlayShoot(IsAiming, currentGun.GunData.GetFireRateMultiplier);
                EventManager.Instance.OnShoot(gameObject);
            }
        }
        else if(!currentGun.GunData.overheat)
        {
            StartReload();
        }
    }

    private void StartReload()
    {
        StartCoroutine(nameof(Reloading));
    }

    private IEnumerator Reloading()
    {
        IsReloading = true;        
        playerAnimator.PlayReload();

        yield return new WaitForSeconds(currentGun.GunData.reloadStartDelay);

        while (currentGun.GetMagAmmo < currentGun.GetMagSize && currentGun.GetTotalAmmo > 0)
        {

            yield return new WaitForSeconds(currentGun.GunData.reloadDuration * currentGun.GunData.reloadAddPercentage);
            
            currentGun.AdjustMagAmmo(1);
            
            yield return new WaitForSeconds(currentGun.GunData.reloadDuration * (1-currentGun.GunData.reloadAddPercentage));
        }

        playerAnimator.StopReload();
        IsReloading = false;        
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
        if (newGun == currentGun || IsReloading)
            return;
        
        currentGun.ToggleGun(false);

        currentGun = newGun;
        currentGun.ToggleGun(true);
        OnGunSwapped?.Invoke(currentGun, currentGun.GetMagAmmo, currentGun.GetTotalAmmo);
    }

    private void DisableHands()
    {
        if(currentGun)
            currentGun.ToggleGun(false);
        HandsDisabled = true;
    }

    private void EnableHands()
    {
        if(currentGun)
            currentGun.ToggleGun(true);
        HandsDisabled = false;
    }

    private void AddAmmo(int count)
    {
        shotgun.AddTotalAmmo(count);
    }
    
    // This should NOT be here but game jam moment
    public void PickupLoot(PickupPrefab pickup)
    {
        AddAmmo(pickup.GetCount);
    }


}
