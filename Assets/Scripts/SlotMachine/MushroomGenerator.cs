using System.Collections.Generic;
using UnityEngine;

public class MushroomGenerator : MonoBehaviour 
{
    [SerializeField] private List<MushroomAttributeSO> attributes;
    [SerializeField] private List<MushroomRarityStats> rarityStats;
    private Vector2 rarityRange;
    int maxWeight = 0;

    private void Awake()
    {
        foreach (MushroomRarityStats rarityStat in rarityStats)
        {
            maxWeight += rarityStat.weight;
        }
    }

    public Mushroom GetMushroom()
    {
        Debug.Log("HERE");

        MushroomRarityStats pickedRarity = new MushroomRarityStats();
        int rarityRoll = Random.Range(0, maxWeight);
        int runningTotal = 0;

        foreach (MushroomRarityStats rarityStat in rarityStats)
        {
            runningTotal += rarityStat.weight;
            if (runningTotal >= rarityRoll)
            {
                pickedRarity = rarityStat;
                break;
            }
        }

        List<MushroomAttributeSO> mushroomAttributes = new List<MushroomAttributeSO>();

        int points = pickedRarity.points;
        int maxAttributesWeight = 0;
        foreach (MushroomAttributeSO attribute in attributes)
        {
            maxAttributesWeight += attribute.Weight;
        }

        int dam = 0;
        bool pointsSpent = false;
        while (!pointsSpent)
        {
            int roll = Random.Range(0, maxAttributesWeight);
            int attributeRunningTotal = 0;
            foreach (MushroomAttributeSO attribute in attributes)
            {
                attributeRunningTotal += attribute.Weight;
                if (attributeRunningTotal >= roll)
                {
                    points -= attribute.SelectionCost;
                    if (points <= 0) pointsSpent = true;

                    attribute.OnSelected();
                    mushroomAttributes.Add(attribute);
                    break;
                }
            }
            dam++;
            if (dam > 10)
            {
                Debug.Log("DAMED");
                break;
            }
        }

        Mushroom mushroom = new Mushroom(mushroomAttributes);
        return mushroom;
    }
}
