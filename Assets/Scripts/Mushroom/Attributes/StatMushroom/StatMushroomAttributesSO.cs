using UnityEngine;

[CreateAssetMenu(fileName = "StatShroom", menuName = "MushroomSOs/StatShroom")]
public class StatMushroomAttributesSO : MushroomAttributeSO
{
    [Header("Stat Settings")]
    public StatType statType;
    public Vector2 valueRange;

    private float setValue; 
    public float SetValue => setValue;

    public override void OnSelected()
    {
        setValue = Random.Range(valueRange.x, valueRange.y);
    }

    public override void OnTick()
    {
        throw new System.NotImplementedException();
    }
}