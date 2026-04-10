using UnityEngine;

public class LampItemTest : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO lampItem;
    public void Interact()
    {
        PlayerInventory.GetInstance().AddItem(lampItem);
        Destroy(gameObject);
    }
}
