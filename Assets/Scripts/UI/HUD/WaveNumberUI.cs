using UnityEngine;
using TMPro;
using System;

public class WaveNumberUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textObject;
    [SerializeField] private string textFormat = "WAVE: {0}";


    private void OnEnable()
    {
        WaveManager.OnNewWave += UpdateText;
    }
    private void OnDisable()
    {
        WaveManager.OnNewWave -= UpdateText;
    }

    private void UpdateText(int waveNumber)
    {
        textObject.text = string.Format(textFormat, waveNumber);
    }
}
