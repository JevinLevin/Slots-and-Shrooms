using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region Singleton
    public static EventManager Instance { get; private set; }
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

    public event Action<GameObject, GameObject> onHit;
    public event Action<GameObject> onShoot; 
    public event Action onTick;
    public event Action onEnemyDies;
    public event Action<PlayerStat> statsUpdated;

    public void OnHit(GameObject ObjHit, GameObject attacker)
    {
        if (onHit != null)
        {
            onHit(ObjHit, attacker);
        }
    }
    public void OnShoot(GameObject shooter)
    {
        if (onShoot != null)
        {
            onShoot(shooter);   
        }
    }
    public void OnTick()
    {
        if (onTick != null) onTick();
    }

    public void OnEnemyDies()
    {
        if (onEnemyDies != null) onEnemyDies(); 
    }

    public void StatsUpdated(PlayerStat stat)
    {
        if(statsUpdated != null) statsUpdated(stat);
    }
}
