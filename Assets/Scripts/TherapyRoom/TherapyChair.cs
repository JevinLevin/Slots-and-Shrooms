using UnityEngine;

public class TherapyChair : MonoBehaviour, IInteractable
{
    [SerializeField] private Outlinable outlinable;

    private bool activated = false;

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
}
