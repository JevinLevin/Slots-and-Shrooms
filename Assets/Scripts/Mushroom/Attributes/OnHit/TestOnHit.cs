using UnityEngine;

[CreateAssetMenu(fileName = "TestOnHitShroom", menuName = "MushroomSOs/TestOnHitShroom")]
public class TestOnHit : OnHitMushroomAttributeSO
{
    public override void OnHit(GameObject objHit)
    {
        Debug.Log($"Hit");
    }

    public override void OnSelected()
    {
        
    }

    public override void OnTick()
    {
        throw new System.NotImplementedException();
    }
}
