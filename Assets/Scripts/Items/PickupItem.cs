using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemData;

    public void Interact()
    {
        if (itemData.isBurnable)
        {
            AudioManager.GetInstance().PlaySound(AudioManager.SoundType.notePickup);
        }
        else
        {
            AudioManager.GetInstance().PlaySound(AudioManager.SoundType.itemPickup);
        }

        if (NoteReaderUI.GetInstance() != null)
        {
            NoteReaderUI.GetInstance().OpenNote(itemData);
        }

        PlayerInventory.GetInstance().AddItem(itemData);

        Destroy(gameObject);
    }
}