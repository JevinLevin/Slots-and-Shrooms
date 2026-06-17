using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerStats")]
public class PlayerStatsHolderSO : ScriptableObject
{

    [SerializeField] private PlayerStat[] playerBaseStats;
    public PlayerStat[] PlayerBaseStats => playerBaseStats; 
    private List<PlayerStat> playerStats;

    public void Initialise()
    {
        playerStats = playerBaseStats.Select
            (x => new PlayerStat
            {
                type = x.type,
                value = x.value
            }
            ).ToList();
    }

    public void AddStat(StatMushroomAttributesSO statAttribute)
    {
        foreach(PlayerStat playerStat  in playerStats)
        {
            if (playerStat.type == statAttribute.statType)
            {
                playerStat.value += statAttribute.SetValue;
                break;
            }
        }
    }

    public PlayerStat ReadStat(StatType statType)
    {
        foreach (PlayerStat playerStat in playerStats)
        {
            if (playerStat.type == statType) return playerStat; 
        }

        Debug.LogError($"StatType: {statType} not found");
        return null;
    }
}