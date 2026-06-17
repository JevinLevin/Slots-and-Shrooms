using System;
using TMPro;
using UnityEngine;

public class GunDisplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textObject;
    [SerializeField] private string textFormat = "{0}\n{1}/{2}";

    private string gunName;
    private int maxAmmo;
    private int ammoCount;
    
    private void OnEnable()
    {
        PlayerShooter.OnGunSwapped += UpdateGunText;
        Gun.OnGunAmmoChanged += UpdateAmmoText;
    }
    private void OnDisable()
    {
        PlayerShooter.OnGunSwapped -= UpdateGunText;
        Gun.OnGunAmmoChanged -= UpdateAmmoText;
    }

    private void UpdateText()
    {
        textObject.text = string.Format(textFormat, gunName, ammoCount, maxAmmo);
    }

    private void UpdateGunText(GunSO gunData, int ammo)
    {
        gunName = gunData.gunName;
        maxAmmo = gunData.magSize;
        ammoCount = ammo;
        UpdateText();
    }
    private void UpdateAmmoText(int ammo)
    {
        ammoCount = ammo;
        UpdateText();
    }
}
