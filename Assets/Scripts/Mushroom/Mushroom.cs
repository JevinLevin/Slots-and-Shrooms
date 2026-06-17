using System.Collections.Generic;
using UnityEngine;

public class Mushroom
{
    private List<MushroomAttributeSO> attributes;
    public List<MushroomAttributeSO> Attributes { get { return attributes; } }
    private Texture2D mushroomTexture;
    public Texture2D MushroomTexture => mushroomTexture;

    public Mushroom(List<MushroomAttributeSO> attributes)
    {
        this.attributes = attributes;
    }

    public void SetTexture(Texture2D texture)
    {
        mushroomTexture = texture;
    }
}
    