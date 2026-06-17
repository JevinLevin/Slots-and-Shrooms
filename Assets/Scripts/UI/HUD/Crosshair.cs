using System;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private CanvasFader canvasFader;
    [SerializeField] private Image overheatProgress;
    [SerializeField] private Gradient overheatGradient;

    private void OnEnable()
    {
        EventManager.Instance.onHit += HitmarkerPlay;
        Gun.OnGunOverheatUpdate += SetOverheatProgress;
    }
    private void OnDisable()
    {
        EventManager.Instance.onHit -= HitmarkerPlay;
        Gun.OnGunOverheatUpdate -= SetOverheatProgress;
    }

    private void Awake()
    {
        SetOverheatProgress(0);
    }

    private void HitmarkerPlay(GameObject obj, GameObject attacker)
    {
        canvasFader.PlayFull();
    }

    private void SetOverheatProgress(float value)
    {
        overheatProgress.fillAmount = value;
        Color color = overheatGradient.Evaluate(value);
        overheatProgress.color = color;
    }
}
