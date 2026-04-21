using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void Interact();
}

public class InteractionHandler : MonoBehaviour
{
    [Header("InputHandler")]
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [Header("Source and range")]
    [SerializeField] private Transform interactionSource;
    [SerializeField] private float interactRange;

    void Update()
    {
        if (playerInputHandler.Interaction())
        {
            Ray r = new Ray(interactionSource.position, interactionSource.forward);
            Debug.DrawRay(interactionSource.position, interactionSource.forward * interactRange, Color.green, 2f);
            
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
