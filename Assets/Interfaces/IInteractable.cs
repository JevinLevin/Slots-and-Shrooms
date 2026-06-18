using UnityEngine;

public interface IInteractable
{
    public void OnHover();
    public void OnUnhover();
    public void OnInteract(Interactor interactor);
    public bool IsInteractable { get; }
}
