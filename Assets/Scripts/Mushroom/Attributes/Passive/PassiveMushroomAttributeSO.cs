using UnityEngine;

public abstract class PassiveMushroomAttributeSO : MushroomAttributeSO
{
    [SerializeField] float timer;
    private float time = 0;

    public override void OnTick()
    {
        time += 0.1f;
        if (time < timer) return;
        PassiveAbillity(); 
        time = 0;
    }

    public abstract void PassiveAbillity();
}