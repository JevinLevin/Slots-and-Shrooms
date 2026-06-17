using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunDisplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textObject;
    [SerializeField] private string textFormat = "{0}\n{1}/{2}";
    [SerializeField] private string textFormatInfinite = "{0}\n";
    [SerializeField] private Image infiniteSymbol;
    [SerializeField] private Color infiniteOverheatColor;

    private string gunName;
    private int maxAmmo;
    private int ammoCount;
    
    private void OnEnable()
    {
        PlayerShooter.OnGunSwapped += UpdateGunText;
        Gun.OnGunAmmoChanged += UpdateAmmoText;
        Gun.OnGunOverheatStart += OverheatStart;
        Gun.OnGunOverheatEnd += OverheatEnd;
    }

    private void OnDisable()
    {
        PlayerShooter.OnGunSwapped -= UpdateGunText;
        Gun.OnGunAmmoChanged -= UpdateAmmoText;
        Gun.OnGunOverheatStart -= OverheatStart;
        Gun.OnGunOverheatEnd -= OverheatEnd;
    }
    private void OverheatStart()
    {
        infiniteSymbol.color = infiniteOverheatColor;
    }
    private void OverheatEnd()
    {
        infiniteSymbol.color = Color.white;
    }


    private void UpdateText()
    {
        if(maxAmmo > 0)
        {
            textObject.text = string.Format(textFormat, gunName, ammoCount, maxAmmo);
            infiniteSymbol.enabled = false;
        }
        else
        {
            textObject.text = string.Format(textFormatInfinite, gunName);
            infiniteSymbol.enabled = true;
        }
    }

    private void UpdateGunText(Gun gun, int ammo)
    {
        gunName = gun.GunData.gunName;
        maxAmmo = gun.GunData.magSize;
        ammoCount = ammo;
        UpdateText();
    }
    private void UpdateAmmoText(int ammo)
    {
        ammoCount = ammo;
        UpdateText();
    }
}
