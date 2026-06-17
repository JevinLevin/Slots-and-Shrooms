using System;
using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private string textFormat = "WAVE END: {0:00}:{1:00}";

    private void OnEnable()
    {
        WaveManager.OnWaveTimeChanged += UpdateText;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveTimeChanged -= UpdateText;
    }

    private void UpdateText(int time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        timerText.text = string.Format(textFormat, minutes, seconds);

    }
}
