using UnityEngine;

public class EnemyWeaponBase : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    private void OnTriggerEnter(Collider other)
    {
        IHasHealth playerHealth = other.GetComponent<IHasHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnHit(attackDamage, gameObject); 
        }
    }
}