using System;
using UnityEngine;

public class SensitivityOption : MonoBehaviour
{
    public Vector2 range = new Vector2(0.25f, 1.75f);
    public static float SensMultiplier = 0.5f;
    
    public void SetValue(float value)
    {
        SensMultiplier = Mathf.Lerp(range.x, range.y, value);
    }
    
}
