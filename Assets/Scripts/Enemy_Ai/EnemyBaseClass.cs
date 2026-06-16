using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBaseClass : MonoBehaviour, IHasHealth
{
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    private bool inRange; 
    [SerializeField] private float attackCD;
    private float currentAttackCD; 

    [SerializeField] protected NavMeshAgent agent;
    private Transform player;

    private void Awake()
    {
        currentAttackCD = 0;
        player = GameObject.FindWithTag("Player").transform; 
        agent.speed = speed;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.position) <= range) inRange = true;
        else
        {
            inRange = false;
            MoveTowardsPlayer(); 
        }

        if (!inRange) return;
        agent.ResetPath();

        Vector3 towardsPlayer = player.position - transform.position;
        towardsPlayer.y = 0;
        transform.rotation = Quaternion.LookRotation(towardsPlayer);

        currentAttackCD += Time.deltaTime;
        if(currentAttackCD >= attackCD)
        {
            Attack();
            currentAttackCD = 0; 
        }
    }

    protected virtual void MoveTowardsPlayer() => agent.SetDestination(player.position);
    protected abstract void Attack(); 

    public virtual void OnHit(int damage)
    {
        EventManager.Instance.OnHit(gameObject); 
        health -= damage;
        if (health <= 0) Die(); 
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
