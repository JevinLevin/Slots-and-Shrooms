using UnityEngine;

public enum AttributeType { Stat, Onhit, Passive }

public abstract class MushroomAttributeSO : ScriptableObject
{
    [Header("Base Settings")]
    [SerializeField] private string attributeName;
    public string AttributeName => attributeName;

    [SerializeField] private AttributeType type;
    public AttributeType Type => type;

    [SerializeField, TextArea] private string description;
    public string GetDescription => description;
    [SerializeField] private int selectionCost; 
    public int SelectionCost => selectionCost;
    [SerializeField] private int weight;
    public int Weight => weight;
    [SerializeField] private bool isBuff = true;
    public bool IsBuff => isBuff;

    public abstract void OnSelected(); 
    public abstract void OnTick();
}
