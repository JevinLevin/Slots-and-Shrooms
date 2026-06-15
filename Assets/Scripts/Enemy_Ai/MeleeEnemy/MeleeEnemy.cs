using System.Collections;
using UnityEngine;

public class MeleeEnemy : EnemyBaseClass
{
    [SerializeField] private GameObject attackCollider;

    protected override void Attack()
    {
        StartCoroutine(BaseAttack());
    }

    private IEnumerator BaseAttack()
    {
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackCollider.SetActive(false);
    }
}
