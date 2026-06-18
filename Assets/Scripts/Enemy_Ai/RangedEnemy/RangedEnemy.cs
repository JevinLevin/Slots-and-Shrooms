using UnityEngine;

public class RangedEnemy : EnemyBaseClass
{
    [Header("Ranged Settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private int projectileDamage; 
    [SerializeField] private float projectileSpeed;

    public override void TryAttack()
    {
        EnemyProjectile enemyProjectile = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<EnemyProjectile>();
        enemyProjectile.SetObjFiredFrom(gameObject); 
        enemyProjectile.SetFowardDirection(transform.forward);
        enemyProjectile.SetProjectileDamadge(projectileDamage);
        enemyProjectile.SetProjectileSpeed(projectileSpeed);
    }
}
