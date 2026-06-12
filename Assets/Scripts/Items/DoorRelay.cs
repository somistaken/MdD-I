using UnityEngine;

public class DoorRelay : MonoBehaviour, IInteractable
{
    [Tooltip("Arrastra aquí el objeto Pivote que tiene el DoorController")]
    [SerializeField] private DoorController parentDoor;

    public void Interact()
    {
        if (parentDoor != null)
        {
            parentDoor.Interact();
        }
    }
}