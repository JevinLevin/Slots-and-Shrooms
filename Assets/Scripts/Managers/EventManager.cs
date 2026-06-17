using System;
using UnityEditor.Experimental.GraphView;
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
    public event Action onTick;

    public void OnHit(GameObject ObjHit, GameObject attacker)
    {
        if (onHit != null)
        {
            onHit(ObjHit, attacker);
        }
    }
    public void OnTick()
    {
        if (onTick != null) onTick();
    }
}
