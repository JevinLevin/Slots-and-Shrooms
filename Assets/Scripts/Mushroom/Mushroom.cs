using System.Collections.Generic;
using UnityEngine;

public class Mushroom
{
    private List<MushroomAttributeSO> attributes;
    public List<MushroomAttributeSO> Attributes { get { return attributes; } }
    private Texture2D mushroomTexture;
    public Texture2D MushroomTexture => mushroomTexture;

    private List<PlayerStat> statsList = new List<PlayerStat>(); 
    public List<PlayerStat> StatList => statsList;

    public Mushroom(List<MushroomAttributeSO> attributes)
    {
        this.attributes = attributes;

        foreach(MushroomAttributeSO attribute in attributes)
        {
            if(attribute.Type == AttributeType.Stat)
            {
                StatMushroomAttributesSO statAttribute = (StatMushroomAttributesSO)attribute;
                PlayerStat newStat = new PlayerStat(statAttribute.statType, statAttribute.SetValue);
                statsList.Add(newStat);
            }
        }
    }

    public void SetTexture(Texture2D texture)
    {
        mushroomTexture = texture;
    }
}
    