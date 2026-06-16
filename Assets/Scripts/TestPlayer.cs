using UnityEngine;

public class TestPlayer : MonoBehaviour, IHasHealth
{
    public int health = 6;

    public void Die()
    {
        Destroy(gameObject);
    }

    public void OnHit(int damage)
    {
        EventManager.Instance.OnHit(gameObject); 
        health -= damage;
        if(health <= 0) Die();
    }
}
