using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun")]
public class GunSO : ScriptableObject
{
    public Sprite gunIcon;
    public string gunName;
    public string gunDesc;

    public float fireRate = 10;
    public float magSize = 16;
    public int bulletsPerShot = 1;
}
