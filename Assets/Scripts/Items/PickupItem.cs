using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemData;

    public void Interact()
    {
        if (NoteReaderUI.GetInstance() != null)
        {
            NoteReaderUI.GetInstance().OpenNote(itemData);
        }

        PlayerInventory.GetInstance().AddItem(itemData);

        Destroy(gameObject);
    }
}