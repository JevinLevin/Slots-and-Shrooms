using UnityEngine;

public abstract class MushroomAttributeSO : ScriptableObject
{
    [Header("Base Settings")]
    [SerializeField] private string mushroomName;

    [SerializeField, TextArea] private string description;
    [SerializeField] private int selectionCost; 
    public int SelectionCost => selectionCost;
    [SerializeField] private int weight;
    public int Weight => weight;

    public abstract void OnSelected(); 
    public abstract void OnTick();
}
