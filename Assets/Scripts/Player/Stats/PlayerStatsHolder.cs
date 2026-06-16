using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatsHolder : MonoBehaviour
{
    #region Singleton
    public static PlayerStatsHolder Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    [SerializeField] private PlayerStat[] playerBaseStats;
    public PlayerStat[] PlayerBaseStats => playerBaseStats; 
    [SerializeField] private List<PlayerStat> playerStats;

    private void Start()
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