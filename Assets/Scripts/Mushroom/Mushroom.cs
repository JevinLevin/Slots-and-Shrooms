using System.Collections.Generic;
using UnityEngine;

public enum MushroomRarity { poison, common, yummy, mega }

public class Mushroom
{
    MushroomRarity rarity;
    private List<MushroomAttributeSO> attributes;

    public Mushroom(List<MushroomAttributeSO> attributes, MushroomRarity rarity)
    {
        this.attributes = attributes;
        this.rarity = rarity;
    }
}
    