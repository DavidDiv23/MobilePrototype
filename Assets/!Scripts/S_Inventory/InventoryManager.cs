using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private UI_Crafting uiCrafting;
    [SerializeField] private ItemDatabaseSO itemDatabase;
    [SerializeField] private Inventory inventory;

    private void Awake()
    {
        // Initialize inventory first
        if (inventory == null)
        {
            inventory = gameObject.AddComponent<Inventory>();
            Debug.Log("Created new Inventory component", this);
        }

        // Initialize UIs after inventory is ready
        InitializeUI();

        // Add test items
        if (itemDatabase != null)
            inventory.AddDefaultTestItems(itemDatabase);
    }

    private void InitializeUI()
    {
        // Set UI_Inventory first
        if (uiInventory != null)
        {
            uiInventory.SetInventory(inventory);
        }
        else
        {
            Debug.LogError("UI_Inventory reference missing!", this);
        }

        // Set UI_Crafting after
        if (uiCrafting != null)
        {
            uiCrafting.SetInventory(inventory);
        }
        else
        {
            Debug.LogWarning("UI_Crafting reference missing - crafting disabled", this);
        }
    }




    public void AddItem(Item item)
    {
        if (item.itemData == null)
        {
            Debug.LogError("Tried to add item with null itemData");
            return;
        }

        inventory.AddItem(item);
        Debug.Log($"Item added: {item.itemData.itemName} (x{item.amount})");
    }

    public void AddItem(ItemSO itemData, int amount = 1)
    {
        if (itemData == null)
        {
            Debug.LogError("Tried to add null itemData");
            return;
        }

        Item item = new Item { itemData = itemData, amount = amount };
        AddItem(item);
    }

    public void AddItem(string itemName, int amount = 1)
    {
        if (itemDatabase == null)
        {
            Debug.LogError("ItemDatabase not assigned in InventoryManager");
            return;
        }

        ItemSO itemData = itemDatabase.GetItem(itemName);
        if (itemData == null)
        {
            Debug.LogError($"Item '{itemName}' not found in database");
            return;
        }

        Item item = new Item { itemData = itemData, amount = amount };
        AddItem(item);
    }

    public bool HasItem(ItemSO itemData, int requiredAmount = 1)
    {
        foreach (Item item in inventory.GetItemList())
        {
            if (item.itemData == itemData && item.amount >= requiredAmount)
            {
                return true;
            }
        }
        return false;
    }

    // Updated RemoveItem method
    public bool RemoveItem(ItemSO itemData, int amountToRemove = 1)
    {
        // First check if we have enough items
        if (!HasItem(itemData, amountToRemove))
        {
            Debug.Log($"Not enough {itemData.itemName} in inventory");
            return false;
        }

        // Create a temporary item for removal
        Item itemToRemove = new Item { itemData = itemData, amount = amountToRemove };

        // Get the actual item from inventory
        Item inventoryItem = inventory.GetItemList().Find(i => i.itemData == itemData);

        if (inventoryItem != null)
        {
            // Remove the item(s)
            if (itemData.isStackable)
            {
                inventoryItem.amount -= amountToRemove;
                if (inventoryItem.amount <= 0)
                {
                    inventory.GetItemList().Remove(inventoryItem);
                }
            }
            else
            {
                // For non-stackable items, we need to remove the exact number
                for (int i = 0; i < amountToRemove; i++)
                {
                    inventory.GetItemList().Remove(inventoryItem);
                }
            }
            return true;
        }

        return false;
    }





    /*
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
    */
}

