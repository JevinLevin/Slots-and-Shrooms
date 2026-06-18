using UnityEngine;

[CreateAssetMenu(fileName = "Loot", menuName = "Loot")]
public class LootSO : ScriptableObject
{
    [SerializeField] private string lootName;
    [SerializeField] private GameObject lootModel;
    public GameObject GetLootModel => lootModel;
}
