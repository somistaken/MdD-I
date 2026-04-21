using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    public ItemSO itemData;

    public void Interact()
    {
        PlayerInventory.GetInstance().AddItem(itemData);
        Destroy(gameObject);
    }
}