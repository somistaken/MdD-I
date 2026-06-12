using UnityEngine;

public class InteractableRelay : MonoBehaviour, IInteractable
{
    [SerializeField] private ExitDoor parentDoor;

    public void Interact()
    {
        if (parentDoor != null)
        {
            parentDoor.Interact();
        }
    }
}