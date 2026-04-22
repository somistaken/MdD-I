using UnityEngine;

public class ChimneyHandler : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        PlayerInventory.GetInstance().BurnNotes();
    }
}
