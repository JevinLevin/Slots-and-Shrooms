using UnityEngine;

public abstract class MushroomAttributeSO : ScriptableObject
{
    [Header("Base Settings")]
    [SerializeField] private string mushroomName;

    [SerializeField, TextArea] private string description;
    [SerializeField] private int SelectionCost; 

    public abstract void OnSelected(); 
    public abstract void OnTick();
}
