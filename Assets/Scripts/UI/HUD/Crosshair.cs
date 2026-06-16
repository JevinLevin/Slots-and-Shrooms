using System;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private CanvasFader canvasFader;

    private void OnEnable()
    {
        EventManager.Instance.onHit += HitmarkerPlay;
    }
    private void OnDisable()
    {
        EventManager.Instance.onHit -= HitmarkerPlay;
    }

    private void HitmarkerPlay(GameObject obj)
    {
        canvasFader.PlayFull();
    }
}
