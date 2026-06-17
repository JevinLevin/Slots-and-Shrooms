using UnityEngine;

public interface IHasHealth
{
    public void OnHit(float damage, GameObject attacker);
    public void Die();
}
