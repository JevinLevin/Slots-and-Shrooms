using UnityEngine;

public interface IHasHealth
{
    public void OnHit(int damage);
    public void Die();
}
