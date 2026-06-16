using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    public static readonly int IsAiming = Animator.StringToHash("IsAiming");
    public static readonly int IsWalking = Animator.StringToHash("IsWalking");
    public static readonly int IsRunning = Animator.StringToHash("IsRunning");

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleAiming(bool value)
    {
        animator.SetBool(IsAiming, value);
    }

    public void ToggleWalking(bool value)
    {
        animator.SetBool(IsWalking, value);
    }

    public void ToggleRunning(bool value)
    {
        animator.SetBool(IsRunning, value);
    }

    public void PlayShoot(bool aiming)
    {
        if(aiming)
            animator.CrossFadeInFixedTime("Shoot", 0.1f);
        else
            animator.CrossFadeInFixedTime("ShootHip", 0.1f);
    }
}
