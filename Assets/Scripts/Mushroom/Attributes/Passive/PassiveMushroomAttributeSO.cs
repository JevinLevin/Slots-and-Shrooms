using UnityEngine;

public abstract class PassiveMushroomAttributeSO : MushroomAttributeSO
{
    private void Awake()
    {
        EventManager.Instance.onTick += PassiveAbillity;
    }

    public abstract void PassiveAbillity();
}