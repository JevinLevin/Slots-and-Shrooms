using UnityEngine;

public class TherapyChair : MonoBehaviour, IInteractable
{
    [SerializeField] private Outlinable outlinable;

    private bool activated = false;

    public bool IsInteractable => activated;

    public void ToggleState(bool value)
    {
        outlinable.SetOutline(value);
        activated = value;
    }

    public void OnInteract(Interactor interactor)
    {
        if (!activated)
            return;

        activated = false;

        GameManager.Instance.StartLoadGame();
    }

    public void OnHover()
    {
        InteractTextUI.OnShowInteract(" to therapise yourself");
    }

    public void OnUnhover()
    {
        InteractTextUI.OnHideInteract();
    }
}
