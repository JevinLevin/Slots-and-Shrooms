using System;
using UnityEngine;

public class PickupPrefab : MonoBehaviour
{
    [SerializeField] private Transform modelRoot;

    private LootSO loot;
    private int count;
    public int GetCount => count;

    public void Initialise(LootSO loot, int count)
    {
        this.loot = loot;
        this.count = count;
        
        Instantiate(loot.GetLootModel, modelRoot);
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponentInChildren<PlayerShooter>();
        if (target)
        {
            target.PickupLoot(this);
            Destroy(gameObject);
        }
    }
}
