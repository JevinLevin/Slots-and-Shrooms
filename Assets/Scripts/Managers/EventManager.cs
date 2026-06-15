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

    public event Action<GameObject> onHit;
    public event Action onTick;

    public void OnHit(GameObject ObjHit)
    {
        if (onHit != null)
        {
            onHit(ObjHit);
        }
    }
    public void OnTick()
    {
        if (onTick != null) onTick();
    }
}
