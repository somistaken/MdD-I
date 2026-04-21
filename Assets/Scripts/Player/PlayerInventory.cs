using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInventory : MonoBehaviour
{
    static PlayerInventory instance;

    [Header("Inventory Items")]
    [SerializeField] SerializedDictionary<string, ItemSO> inventoryDict = new();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one inventory in the scene");
        }
        instance = this;
    }

    public void AddItem(ItemSO item)
    {
        if (inventoryDict.ContainsKey(item.id))
        {
            return;
        }

        inventoryDict.Add(item.id, item);
    }

    public void RemoveItem(string itemID)
    {
        inventoryDict.Remove(itemID);
    }

    public static PlayerInventory GetInstance()
    {
        return instance;
    }

    public bool IsItemInInventory(string inventoryId)
    {
        return inventoryDict.ContainsKey(inventoryId);
    }
}
