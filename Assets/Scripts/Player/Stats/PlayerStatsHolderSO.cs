using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerStats")]
public class PlayerStatsHolderSO : ScriptableObject
{

    [SerializeField] private PlayerStat[] playerBaseStats;
    public PlayerStat[] PlayerBaseStats => playerBaseStats; 
    public List<PlayerStat> playerStats;

    public void Initialise()
    {
        playerStats = playerBaseStats.Select
            (x => new PlayerStat
            (x.type, x.value)
            ).ToList();
        
    }

    public void AddStat(PlayerStat statAttribute)
    {
        foreach(PlayerStat playerStat  in playerStats)
        {
            if (playerStat.type == statAttribute.type)
            {
                playerStat.value += statAttribute.value;
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

    public float GetStatAsMultiplier(StatType statType)
    {
        foreach (PlayerStat playerStat in playerStats)
        {
            float multiplier = 1 + (playerStat.value / 100);
            // Dont let multiplier go below 0.1 or else stuff breaks
            multiplier = Mathf.Max(0.1f, multiplier);
            if (playerStat.type == statType) return multiplier; 
        }
        
        Debug.LogError($"StatType: {statType} not found");
        return 1;
    }
    public float GetStatAsMultiplierInverse(StatType statType)
    {
        foreach (PlayerStat playerStat in playerStats)
        {
            float multiplier = 1 - (playerStat.value / 100);
            // Dont let multiplier go below 0.1 or else stuff breaks
            multiplier = Mathf.Max(0.1f, multiplier);
            if (playerStat.type == statType) return multiplier; 
        }
        
        Debug.LogError($"StatType: {statType} not found");
        return 1;
    }
}