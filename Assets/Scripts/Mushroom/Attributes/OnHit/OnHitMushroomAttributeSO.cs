using UnityEngine;

public abstract class OnHitMushroomAttributeSO : MushroomAttributeSO
{
    private void Awake()
    {
        EventManager.Instance.onHit += OnHit; 
    }

    public abstract void OnHit(GameObject objHit);
}