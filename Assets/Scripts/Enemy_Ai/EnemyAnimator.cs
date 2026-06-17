using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int NormalisedMoveSpeed = Animator.StringToHash("NormalisedMoveSpeed");
    [SerializeField] private Animator animator;


    public void SetMovement(bool isMoving, float normalisedSpeed)
    {
        animator.SetBool(IsMoving, isMoving);
        animator.SetFloat(NormalisedMoveSpeed, normalisedSpeed);
    }

    public void PlayAttack()
    {
        animator.CrossFadeInFixedTime("Attack", 0.1f, 2);
    }

    public void PlayAttacked()
    {
        animator.CrossFadeInFixedTime("Attacked", 0.1f, 2);
    }

    public void PlayDeath(float duration)
    {
        animator.CrossFadeInFixedTime("Death", 0.1f, 0);
        animator.CrossFadeInFixedTime("None", 0.1f, 1);
        animator.CrossFadeInFixedTime("None", 0.1f, 2);

    }
}
