using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class InventoryUI : MonoBehaviour
{
    static InventoryUI instance;

    [Header("References")]
    [SerializeField] GameObject uiItemPrefab;
    [SerializeField] RectTransform inventoryPanel;
    [SerializeField] TextMeshProUGUI notesText;
    [Header("Inventory UI elements")]
    SerializedDictionary<string, GameObject> inventoryUI = new();
    private int noteAmount = 0;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Inventory UI in the scene");
        }
        instance = this;
    }

    public void AddUIItem(ItemSO item)
    {
        if (item.isBurnable)
        {
            noteAmount++;
            notesText.text = "Notas: \n" + noteAmount + "/10";
            return;
        }
        var itemUI = Instantiate(uiItemPrefab).GetComponent<ItemUI>();
        itemUI.transform.SetParent(inventoryPanel);
        inventoryUI.Add(item.id, itemUI.gameObject);
        itemUI.Initialize(item);
    }

    public void RemoveUIITem(string itemID)
    {
        var itemUI = inventoryUI.GetValueOrDefault(itemID);
        inventoryUI.Remove(itemID);
        Destroy(itemUI);
    }
}
