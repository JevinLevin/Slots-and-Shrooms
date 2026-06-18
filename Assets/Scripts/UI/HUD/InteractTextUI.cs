using UnityEngine;
using TMPro;
using System;

public class InteractTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textObject;
    [SerializeField] private string textFormat = "Press E to {0}";


    public static Action<string> OnShowInteract;
    public static Action OnHideInteract;

    private void OnEnable()
    {
        OnShowInteract += ShowInteract;
        OnHideInteract += HideInteract;
    }
    private void OnDisable()
    {
        OnShowInteract -= ShowInteract;
        OnHideInteract -= HideInteract;
    }

    private void Awake()
    {
        HideInteract();
    }

    private void HideInteract()
    {
        textObject.text = "";
    }

    private void ShowInteract(string interactName)
    {
        textObject.text = string.Format(textFormat, interactName);
    }
}
