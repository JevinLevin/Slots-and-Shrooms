using System;
using UnityEngine;

public class DamageTintUI : MonoBehaviour
{

    [SerializeField] private CanvasFader canvasFader;
    private void OnEnable()
    {
        PlayerHealth.OnTakeDamage += ShowTint;
    }

    private void OnDisable()
    {
        PlayerHealth.OnTakeDamage -= ShowTint;
    }

    private void ShowTint()
    {
        canvasFader.PlayFull();
    }
}
