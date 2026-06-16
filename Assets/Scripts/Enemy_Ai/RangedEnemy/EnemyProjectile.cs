using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private GameObject objFiredFrom; 
    private Vector3 foward;
    private float speed;
    private int damage;

    [SerializeField] float decayTime = 5f; 

    private void FixedUpdate()
    {
        transform.position += (foward * speed) * Time.deltaTime;
        decayTime -= Time.deltaTime;
        if(decayTime <= 0) Destroy(gameObject);
    }

    public void SetObjFiredFrom(GameObject objFiredFrom) { this.objFiredFrom = objFiredFrom; }
    public void SetFowardDirection(Vector3 direction) { foward = direction; }
    public void SetProjectileSpeed(float speed) { this.speed = speed; }
    public void SetProjectileDamadge(int damadge) { this.damage = damadge; }

    private void OnTriggerEnter(Collider other)
    {
        IHasHealth health = other.GetComponent<IHasHealth>();
        if (health != null && other.gameObject.CompareTag("Player"))
        {
            health.OnHit(damage, objFiredFrom);
            Destroy(gameObject);
        }
    }

}
