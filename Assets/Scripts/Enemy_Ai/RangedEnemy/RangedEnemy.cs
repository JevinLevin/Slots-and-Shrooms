using System;
using PrimeTween;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangedEnemy : EnemyBaseClass
{
    [Header("Ranged Settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private int projectileDamage; 
    [SerializeField] private Vector2 projectileSpeed = new Vector2(3,6);

    private Tween delayTween;

    private void OnDisable()
    {
        if(delayTween.isAlive)
            delayTween.Stop();
    }

    public override void TryAttack(Transform target)
    {
        delayTween = Tween.Delay(0.4f, () =>
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
