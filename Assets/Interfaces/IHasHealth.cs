using UnityEngine;

public interface IHasHealth
{
    public void OnHit(int damage, GameObject attacker);
    public void Die();
}
