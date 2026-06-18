using UnityEngine;

public class PickupDropper : MonoBehaviour
{
    [SerializeField] private LootTableSO lootTable;
    [SerializeField] private PickupPrefab pickupPrefab;

    public void Trigger()
    {
        var lootPool = lootTable.GenerateLoot();
        foreach (var loot in lootPool)
        {
            var newPickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
            newPickup.Initialise(loot.Key, loot.Value);
        }
    }
}
