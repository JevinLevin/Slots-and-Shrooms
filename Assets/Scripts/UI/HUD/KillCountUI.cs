using TMPro;
using UnityEngine;

public class KillCountUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private string textFormat = "REQUIRED KILLS: {0}/{1}";

    private void OnEnable()
    {
        WaveManager.OnKillsUpdated += UpdateText;
    }

    private void OnDisable()
    {
        WaveManager.OnKillsUpdated -= UpdateText;
    }

    private void UpdateText(int count, int target)
    {
        timerText.text = string.Format(textFormat, count, target);

    }
}
