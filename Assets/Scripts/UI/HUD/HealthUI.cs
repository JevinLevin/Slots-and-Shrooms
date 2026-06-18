using UnityEngine;
using PrimeTween;
using System;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private SlicedFilledImage fillImage;
    [SerializeField] private float updateBarDuration = 0.2f;
    [SerializeField] private Ease updateBarEase = Ease.OutQuad;

    private Tween updateTween;

    private void OnEnable()
    {
        PlayerHealth.OnHealthUpdated += UpdateHealthbar;
    }
    private void OnDisable()
    {
        PlayerHealth.OnHealthUpdated -= UpdateHealthbar;
    }

    private void UpdateHealthbar(float healthProgress)
    {
        if (updateTween.isAlive)
            updateTween.Stop();

        updateTween = Tween.Custom(fillImage.fillAmount, healthProgress, updateBarDuration, newValue => fillImage.fillAmount = newValue, updateBarEase);
    }
}
