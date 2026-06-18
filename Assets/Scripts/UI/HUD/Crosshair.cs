using System;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private CanvasFader canvasFader;
    [SerializeField] private Image crosshair;
    [SerializeField, Range(0, 1)] private float crosshairOffAlpha = 0.25f;
    [SerializeField] private Image overheatProgress;
    [SerializeField] private Gradient overheatGradient;

    private void OnEnable()
    {
        EventManager.Instance.onHit += HitmarkerPlay;
        Gun.OnGunOverheatUpdate += SetOverheatProgress;
        Gun.OnGunOverheatStart += OverheatStart;
        Gun.OnGunOverheatEnd += OverheatEnd;
        PlayerShooter.OnGunSwapped += GunSwapped;
    }




    private void OnDisable()
    {
        EventManager.Instance.onHit -= HitmarkerPlay;
        Gun.OnGunOverheatUpdate -= SetOverheatProgress;
        Gun.OnGunOverheatStart -= OverheatStart;
        Gun.OnGunOverheatEnd -= OverheatEnd;
        PlayerShooter.OnGunSwapped -= GunSwapped;
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
    
    private void OverheatStart()
    {
        ToggleCrosshair(false);
    }
    private void OverheatEnd()
    {
        ToggleCrosshair(true);

    }

    private void ToggleCrosshair(bool value)
    {
        crosshair.color = value 
            ? Color.white
            : new Color(1, 1, 1, crosshairOffAlpha);

    }
    
    private void GunSwapped(Gun gun, int magAmmo, int totalAmmo)
    {
        if(gun.IsOverheated)
            ToggleCrosshair(false);
        else
            ToggleCrosshair(true);
    }
}
