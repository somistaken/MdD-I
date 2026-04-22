using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemData;

    public void Interact()
    {
        PlayerInventory.GetInstance().AddItem(itemData);

        Destroy(gameObject);
    }


}