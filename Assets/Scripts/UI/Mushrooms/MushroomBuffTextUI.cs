using UnityEngine;
using TMPro;
public class MushroomBuffTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textObj;
    [SerializeField] private string prefix = "+";
    [SerializeField] private Color buffColor;

    public void SetBuffText(string desc)
    {
        textObj.text = prefix + " " + desc;
        textObj.color = buffColor;
    }
}
