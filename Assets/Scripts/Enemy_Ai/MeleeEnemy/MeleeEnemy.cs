using System.Collections;
using UnityEngine;

public class MeleeEnemy : EnemyBaseClass
{
    [SerializeField] private EnemyWeaponBase weaponBase;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackDuration = 1;
    [SerializeField, Range(0, 1)] private float attackImpact = 0.5f;
    [SerializeField] private float attackCooldown = 0.5f;

    public bool IsAttacking { get; private set; }

    public override void TryAttack()
    {
        if (IsAttacking)
            return;


        StartCoroutine(BaseAttack());
    }

    private IEnumerator BaseAttack()
    {
        IsAttacking = true;
        enemyAnimator.PlayAttack();

        yield return new WaitForSeconds(attackDuration * attackImpact);

        AttackImpact();

        yield return new WaitForSeconds(1-(attackDuration * attackImpact));

        yield return new WaitForSeconds(attackCooldown);

        IsAttacking = false;


    }

    private void AttackImpact()
    {
        foreach(var other in weaponBase.activeColliders)
        {
            IHasHealth playerHealth = other.GetComponent<IHasHealth>();
            if (playerHealth != null)
            {
                playerHealth.OnHit(attackDamage, gameObject);
            }
        }
    }

}
