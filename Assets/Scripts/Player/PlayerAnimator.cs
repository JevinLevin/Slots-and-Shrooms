using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public static readonly int IsAiming = Animator.StringToHash("IsAiming");
    public static readonly int IsWalking = Animator.StringToHash("IsWalking");
    public static readonly int IsRunning = Animator.StringToHash("IsRunning");
    public static readonly int IsSliding = Animator.StringToHash("IsSliding");
    private static readonly int IsReloading = Animator.StringToHash("IsReloading");

    [SerializeField] private Animator pistolAnimator;
    [SerializeField] private Animator shotgunAnimator;
    [SerializeField] private Animator legsAnimator;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDie += PlayDie;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDie -= PlayDie;
    }
    public void ToggleAiming(bool value)
    {
        if(pistolAnimator.gameObject.activeInHierarchy)
            pistolAnimator.SetBool(IsAiming, value);
        if(shotgunAnimator.gameObject.activeInHierarchy)
            shotgunAnimator.SetBool(IsAiming, value);
    }

    public void ToggleWalking(bool value)
    {
        if(pistolAnimator.gameObject.activeInHierarchy)
            pistolAnimator.SetBool(IsWalking, value);
        if(shotgunAnimator.gameObject.activeInHierarchy)
            shotgunAnimator.SetBool(IsWalking, value);
    }

    public void ToggleRunning(bool value)
    {
        if(pistolAnimator.gameObject.activeInHierarchy)
            pistolAnimator.SetBool(IsRunning, value);
        if(shotgunAnimator.gameObject.activeInHierarchy)
            shotgunAnimator.SetBool(IsRunning, value);
    }

    public void ToggleSliding(bool value)
    {
        legsAnimator.SetBool(IsSliding, value);
    }

    public void PlayShoot(bool aiming)
    {
        if(pistolAnimator.gameObject.activeInHierarchy)
        {
            if (aiming)
                pistolAnimator.CrossFadeInFixedTime("Shoot", 0.1f);
            else
                pistolAnimator.CrossFadeInFixedTime("ShootHip", 0.1f);
        }
        if(shotgunAnimator.gameObject.activeInHierarchy)
        {
            if (aiming)
                shotgunAnimator.CrossFadeInFixedTime("Shoot", 0.1f);
            else
                shotgunAnimator.CrossFadeInFixedTime("ShootHip", 0.1f);
        }
    }
    public void PlayReload()
    {
        if (pistolAnimator.gameObject.activeInHierarchy)
        {
            pistolAnimator.SetBool(IsReloading, true);
            pistolAnimator.CrossFadeInFixedTime("ReloadStart", 0.1f);
        }

        if (shotgunAnimator.gameObject.activeInHierarchy)
        {
            shotgunAnimator.SetBool(IsReloading, true);
            shotgunAnimator.CrossFadeInFixedTime("ReloadStart", 0.1f);
        }
    }
    public void StopReload()
    {
        if(pistolAnimator.gameObject.activeInHierarchy)
            pistolAnimator.SetBool(IsReloading, false);
        if(shotgunAnimator.gameObject.activeInHierarchy)
            shotgunAnimator.SetBool(IsReloading, false);
    }


    private void PlayDie()
    {
        if (pistolAnimator.gameObject.activeInHierarchy)
            pistolAnimator.CrossFadeInFixedTime("Death", 0.1f);
        if (shotgunAnimator.gameObject.activeInHierarchy)
            shotgunAnimator.CrossFadeInFixedTime("Death", 0.1f);
    }
}
