using UnityEngine;

[CreateAssetMenu(fileName = "StatAttribute", menuName = "AttributeSOs/StatAttribute")]
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
        description = $"add {Mathf.Round(setValue)}% to {statType}";
    }

    public override void OnTick()
    {
        throw new System.NotImplementedException();
    }
}