using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    static PlayerInventory instance;
    private InventoryUI inventoryUI;
    [SerializeField] GameObject itemPrefab;

    [Header("Inventory Items")]
    [SerializeField] SerializedDictionary<string, ItemSO> inventoryDict = new();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one inventory in the scene");
        }
        instance = this;

        inventoryUI = GetComponent<InventoryUI>();
    }

    public void AddItem(ItemSO item)
    {
        if (inventoryDict.ContainsKey(item.id))
        {
            return;
        }

        inventoryUI.AddUIItem(item);
        inventoryDict.Add(item.id, item);
    }

    public void RemoveItem(string itemID)
    {
        inventoryUI.RemoveUIITem(itemID);
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

    public void BurnNotes()
    {
        foreach (KeyValuePair<string, ItemSO> item  in inventoryDict.ToList())
        {
            if (item.Value.isBurnable)
            {
                inventoryUI.RemoveUIITem(item.Key);
                inventoryDict.Remove(item.Key);
            }
        }
    }
}
