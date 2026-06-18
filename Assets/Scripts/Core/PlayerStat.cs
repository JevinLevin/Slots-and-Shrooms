using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    public StatType type;
    public float value;

    public PlayerStat(StatType type, float value)
    {
        this.type = type;
        this.value = value;
    }
}