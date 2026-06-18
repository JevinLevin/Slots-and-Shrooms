using PrimeTween;
using UnityEngine;

public class RangedEnemy : EnemyBaseClass
{
    [Header("Ranged Settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private int projectileDamage; 
    [SerializeField] private Vector2 projectileSpeed = new Vector2(3,6);

    public override void TryAttack(Transform target)
    {
        Tween.Delay(0.4f, () =>
        {
            EnemyProjectile enemyProjectile = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            enemyProjectile.SetObjFiredFrom(gameObject);
            Vector3 direction = (target.position + Vector3.up) - transform.position;
            enemyProjectile.SetFowardDirection(direction);
            enemyProjectile.SetProjectileDamadge(projectileDamage);
            enemyProjectile.SetProjectileSpeed(Random.Range(projectileSpeed.x, projectileSpeed.y));
        });
    }
}
