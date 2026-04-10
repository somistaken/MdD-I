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

    public Transform interactionSource;
    public float interactRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInputHandler.InteractTriggered)
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
