using UnityEngine;

[CreateAssetMenu(fileName = "TestOnHitShroom", menuName = "MushroomSOs/TestOnHitShroom")]
public class TestOnHit : OnHitMushroomAttributeSO
{
    public override void OnHit(GameObject objHit)
    {
        Debug.Log($"Hit {objHit.name}");
    }

    public override void OnSelected()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTick()
    {
        throw new System.NotImplementedException();
    }
}
