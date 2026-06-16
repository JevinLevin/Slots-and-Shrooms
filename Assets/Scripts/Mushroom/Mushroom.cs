using System.Collections.Generic;
using UnityEngine;

public class Mushroom
{
    private List<MushroomAttributeSO> attributes;
    public List<MushroomAttributeSO> Attributes { get { return attributes; } }

    public Mushroom(List<MushroomAttributeSO> attributes)
    {
        this.attributes = attributes;
    }
}
    