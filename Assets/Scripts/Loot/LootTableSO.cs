using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootTable", menuName = "LootTable")]
public class LootTableSO : ScriptableObject
{
    [SerializeField] private List<LootTableRolls> Pools = new List<LootTableRolls>();
    
    public Dictionary<LootSO, int> GenerateLoot()
    {
        Dictionary<LootSO, int> output = new();
        
        // Loop based on multiplier
        int multiplier = 1;
        
        for(int loop = 0; loop < multiplier; loop++)
        {
            foreach (LootTableRolls roll in Pools)
            {

                int totalWeight = roll.CalculateWeight();


                for (int r = 0; r < roll.rollCount; r++)
                {
                    int currentWeight = 0;
                    int chosenWeight = Random.Range(0,totalWeight);
                    foreach(LootTableEntry entry in roll.rolls)
                    {
                        currentWeight += entry.weight;
                        if (chosenWeight < currentWeight)
                        {
                            if (Random.value < entry.randomChance)
                            {
                                int addCount = Random.Range(entry.count.x, entry.count.y + 1);

                                if (addCount > 0)
                                {
                                    if (output.TryGetValue(entry.loot, out int existing))
                                        output[entry.loot] = existing + addCount;
                                    else
                                        output[entry.loot] = addCount;
                                }
                            }
                            break;
                        }
                    }   
                }
            }   
        }
        return output;
    }
}

[System.Serializable]
public class LootTableRolls
{
    public int rollCount = 1;
    public List<LootTableEntry> rolls = new();
    [HideInInspector] public int totalWeight;

    public int CalculateWeight()
    {
        totalWeight = 0;
        foreach(LootTableEntry entry in rolls)
        {
            totalWeight += entry.weight;
        }
        return totalWeight;
    }
}

[System.Serializable]
public class LootTableEntry
{
    public LootSO loot;
    public int weight = 1;
    public Vector2Int count = Vector2Int.one;
    
    [Range(0f,1f)]
    public float randomChance = 1.0f;
}