using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBaseClass : MonoBehaviour, IHasHealth
{
    [SerializeField] protected EnemyAnimator enemyAnimator;
    [SerializeField] private PickupDropper pickupDropper;
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    private bool inRange; 
    [SerializeField] private float attackCD;
    private float currentAttackCD;
    private bool canAttack = true;
    [SerializeField] private float deathDuration = 1f;

    [SerializeField] protected NavMeshAgent agent;
    private Transform player;

    [SerializeField] private AudioClip deathSound; 

    private void Awake()
    {
        currentAttackCD = attackCD;
        player = GameObject.FindWithTag("Player").transform; 
        agent.speed = speed;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.position) <= range)
        {
            inRange = true;
            enemyAnimator.SetMovement(false, 0);
        }
        else
        {
            inRange = false;
            MoveTowardsPlayer(); 
        }

        if (!inRange || !agent.isActiveAndEnabled || !agent.isOnNavMesh) return;
        agent.ResetPath();

        Vector3 towardsPlayer = player.position - transform.position;
        towardsPlayer.y = 0;
        transform.rotation = Quaternion.LookRotation(towardsPlayer);

        currentAttackCD += Time.deltaTime;
        if(currentAttackCD >= attackCD)
        {
            if (!canAttack) return; 
            TryAttack(player.transform);
            enemyAnimator.PlayAttack();
            currentAttackCD = 0; 
        }
    }

    protected virtual void MoveTowardsPlayer()
    {
        if (!agent.enabled)
            return;
        
        agent.SetDestination(player.position);
        enemyAnimator.SetMovement(true, 0);
    }
    public abstract void TryAttack(Transform target); 

    public virtual void OnHit(float damage, GameObject attacker)
    {
        if (!agent.enabled)
            return;
        
        //EventManager.Instance.OnHit(gameObject, attacker);  
        health -= damage;
        // Debug.Log($"{gameObject.name} hp = {health}");
        if (health <= 0) Die();
        EventManager.Instance.OnHit(gameObject, attacker);
        enemyAnimator.PlayAttacked();
    }

    public virtual void Die()
    {
        if(pickupDropper)
            pickupDropper.Trigger();

        EventManager.Instance.OnEnemyDies();

        canAttack = false;
        GetComponent<Collider>().enabled = false;
        agent.enabled = false;

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = deathSound; 
        audioSource.Play();

        enemyAnimator.PlayDeath(deathDuration);
        Destroy(gameObject, deathDuration);
    }

    public virtual void Despawn()
    {
        Destroy(gameObject);
    }
}
