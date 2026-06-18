using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactDist = 2;
    [SerializeField] private LayerMask layer;

    private IInteractable currentInteractable;

    void Update()
    {
        var interactable = TryGetInteractable();

        if (interactable == null && currentInteractable != null)
        {
            currentInteractable.OnUnhover();
            currentInteractable = null;
        }
        else if (interactable != null && currentInteractable != interactable)
        {
            if(currentInteractable != null)
                currentInteractable.OnUnhover();
            currentInteractable = interactable;
            currentInteractable.OnHover();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnInteract(this);
            }
        }
    }

    private IInteractable TryGetInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDist, layer))
        {
            if (hit.collider == null) return null;
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            if (interactable == null)
                interactable = hit.collider.gameObject.GetComponentInChildren<IInteractable>();
            if (interactable == null)
                interactable = hit.collider.gameObject.GetComponentInParent<IInteractable>();
            if (interactable == null) return null;

            if (!interactable.IsInteractable)
                return null;

            return interactable;
        }
        return null;
    }
}
