using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseClass : MonoBehaviour, IHasHealth
{
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    private bool inRange; 
    [SerializeField] private float attackCD;
    private float currentAttackCD; 

    [SerializeField] private GameObject attackCollider; 

    [SerializeField] private NavMeshAgent agent;
    private Transform player;

    private void Awake()
    {
        attackCollider.SetActive(false);
        currentAttackCD = 0;
        player = GameObject.FindWithTag("Player").transform; 
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.position) <= range) inRange = true;
        else
        {
            inRange = false;
            if(!agent.hasPath) MoveTowardsPlayer(); 
        }

        if (!inRange) return;
        agent.ResetPath();
        currentAttackCD += Time.deltaTime;
        if(currentAttackCD >= attackCD)
        {
            Attack();
            currentAttackCD = 0; 
        }
    }

    protected virtual void MoveTowardsPlayer() => agent.SetDestination(player.position);
    protected virtual void Attack()
    {
        StartCoroutine(BaseAttack());
    }

    private IEnumerator BaseAttack()
    {
        attackCollider.SetActive(true); 
        yield return new WaitForSeconds(0.2f);
        attackCollider.SetActive(false);
    }

    public virtual void OnHit(int damage)
    {
        health -= damage;
        if (health <= 0) Die(); 
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
