using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public static readonly int IsAiming = Animator.StringToHash("IsAiming");
    public static readonly int IsWalking = Animator.StringToHash("IsWalking");
    public static readonly int IsRunning = Animator.StringToHash("IsRunning");
    public static readonly int IsSliding = Animator.StringToHash("IsSliding");

    [SerializeField] private Animator handsAnimator;
    [SerializeField] private Animator legsAnimator;


    public void ToggleAiming(bool value)
    {
        handsAnimator.SetBool(IsAiming, value);
    }

    public void ToggleWalking(bool value)
    {
        handsAnimator.SetBool(IsWalking, value);
    }

    public void ToggleRunning(bool value)
    {
        handsAnimator.SetBool(IsRunning, value);
    }

    public void ToggleSliding(bool value)
    {
        legsAnimator.SetBool(IsSliding, value);
    }

    public void PlayShoot(bool aiming)
    {
        if(aiming)
            handsAnimator.CrossFadeInFixedTime("Shoot", 0.1f);
        else
            handsAnimator.CrossFadeInFixedTime("ShootHip", 0.1f);
    }
}
