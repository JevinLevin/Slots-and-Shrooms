using UnityEngine;

[CreateAssetMenu(fileName = "StatShroom", menuName = "MushroomSOs/StatShroom")]
public class StatMushroomAttributesSO : MushroomAttributeSO
{
    [Header("Stat Settings")]
    public StatType stat;
    public Vector2 valueRange; 

    public override void OnSelected()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTick()
    {
        throw new System.NotImplementedException();
    }
}
