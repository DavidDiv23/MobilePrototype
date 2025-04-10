using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private UI_Inventory uiInventory;
    private Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory();  // Initialize the inventory
        uiInventory.SetInventory(inventory);  // Set the inventory in UI
    }

    // New method to add item to inventory
    public void AddItem(Item item)
    {
        inventory.AddItem(item);  // Add item to the internal inventory
        Debug.Log("Item added: " + item.itemtype);

        // Optionally update the UI
        uiInventory.RefreshInventoryItems();
    }
}

