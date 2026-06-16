using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactDist = 2;
    [SerializeField] private LayerMask layer; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDist, layer))
            {
                if(hit.collider == null) return;
                IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                if(interactable == null) return;
                interactable.OnInteract(this); 
            }
        }
    }
}
