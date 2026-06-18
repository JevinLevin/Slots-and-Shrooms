using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MushroomInventory", menuName = "MushroomInventory")]
public class MushroomInventorySO : ScriptableObject
{
    [SerializeField] private PlayerStatsHolderSO playerStats;

    private List<Mushroom> mushroomList = new List<Mushroom>();
    [SerializeField] private List<OnHitMushroomAttributeSO> onHitAttributes = new List<OnHitMushroomAttributeSO>();
    [SerializeField] private List<PlayerStat> playerStatsList = new List<PlayerStat>();
    [SerializeField] private List<PassiveMushroomAttributeSO> passiveMushroomAttributeSOs = new List<PassiveMushroomAttributeSO>();

    public List<Mushroom> GetMushrooms => mushroomList;
    public Action<Mushroom> OnMushroomAdded;
    public Action<Mushroom> OnMushroomRemoved;

    public void Initialise()
    {
        EventManager.Instance.onHit += OnHit;
        EventManager.Instance.onTick += OnTick;
    }
    public void Deinitialise()
    {
        EventManager.Instance.onHit -= OnHit;
        EventManager.Instance.onTick -= OnTick;
        ClearMushrooms(); 
    }

    private void OnTick()
    {
        foreach(var passiveEffect in passiveMushroomAttributeSOs) passiveEffect.OnTick();
    }

    private void OnHit(GameObject hitObj, GameObject attacker)
    { 
        foreach(var onHitEffect in onHitAttributes) onHitEffect.OnHit(hitObj, attacker);
    }

    public void AddMushroom(Mushroom mushroom)
    {
        mushroomList.Add(mushroom);
        UnpackMushRoom(mushroom);
        OnMushroomAdded?.Invoke(mushroom);
    }

    public void RemoveMushroom(Mushroom mushroom)
    {
        RemoveMushroom(mushroomList.FindIndex(check => check == mushroom));
    }
    public void RemoveMushroom(int index)
    {
        foreach(var attribute in mushroomList[index].Attributes)
        {
            switch (attribute)
            {
                case StatMushroomAttributesSO stat:
                    foreach (PlayerStat playerStat in mushroomList[index].StatList)
                    {
                        PlayerStat reverseStat = new PlayerStat(playerStat.type, playerStat.value); 
                        playerStats.AddStat(reverseStat);
                    }

                    break;
                case OnHitMushroomAttributeSO onHit:
                    onHitAttributes.Remove(onHit);
                    break;
                case PassiveMushroomAttributeSO passive:
                    passiveMushroomAttributeSOs.Remove(passive);
                    break;
                default:
                    break;
            }
        }
        mushroomList.Remove(mushroomList[index]);
        OnMushroomRemoved?.Invoke(mushroomList[index]);
    }

    private void UnpackMushRoom(Mushroom mushroom)
    {
        foreach(var attribute in mushroom.Attributes)
        {
            switch (attribute)
            {
                case StatMushroomAttributesSO stat:
                    PlayerStat playerStat = new PlayerStat(stat.statType, stat.SetValue);
                    playerStatsList.Add(playerStat);
                    AddStatToPlayer(playerStat);
                    break;
                case OnHitMushroomAttributeSO onHit:
                    onHitAttributes.Add(onHit);
                    break;
                case PassiveMushroomAttributeSO passive:
                    passiveMushroomAttributeSOs.Add(passive);
                    break;
                default:
                    break;
            }
        }
    }

    private void AddStatToPlayer(PlayerStat stat)
    {
        playerStats.AddStat(stat); 
    }

    public void ClearMushrooms()
    {
        mushroomList.Clear();
        onHitAttributes.Clear();
        playerStatsList.Clear();
        passiveMushroomAttributeSOs.Clear(); 
    }
}