using UnityEngine;
using System.Collections.Generic;

public class EnemyWeaponBase : MonoBehaviour
{
    [SerializeField] private MeleeEnemy mainEnemy;
    [SerializeField] private LayerMask targetLayer;

    public List<Collider> activeColliders = new();

    private void OnTriggerEnter(Collider other)
    {

        // If not target layer
        if ((targetLayer & (1 << other.gameObject.layer)) == 0)
            return;

        activeColliders.Add(other);

        mainEnemy.TryAttack(other.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        // If not target layer
        if ((targetLayer & (1 << other.gameObject.layer)) == 0)
            return;

        mainEnemy.TryAttack(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (activeColliders.Contains(other))
            activeColliders.Remove(other);
    }
}