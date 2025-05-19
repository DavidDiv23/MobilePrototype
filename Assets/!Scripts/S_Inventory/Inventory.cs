using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour  // Must inherit from MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    private List<Item> itemList;
    public event EventHandler OnItemListChanged;

    private bool hasAddedDefaultTestItems = false; // New flag


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            itemList = new List<Item>(); // Starts empty
            hasAddedDefaultTestItems = false; // Reset flag

            // Detach from parent (make it a root GameObject)
            transform.parent = null;

            DontDestroyOnLoad(gameObject); // Now works because it's a root
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Improved test items method with error handling
    public void AddDefaultTestItems(ItemDatabaseSO database)
    {
        if (hasAddedDefaultTestItems) return; // Prevent duplicates

        if (database == null)
        {
            Debug.LogError("Database reference is null!");
            return;
        }

        // Safe test items list with fallbacks
        //THE NAMES HERE NEED TO MATCH THE ITEM NAMES IN THE SCRIPTABLE OBJECT
        var testItems = new List<(string name, string fallbackName, int amount)>()
    {
        ("Crystal", null, 5),
        ("Stick", "Wooden Stick", 3),
        ("Snail Shell", "Shell", 2),
        ("Orange", null, 1),
        ("Rock", "Stone", 20),
        ("Blueprint", "Crafting Blueprint", 1),
        ("Berry", "Red Berry", 10),
       // ("Feather", "Bird Feather", 4),
        //("Pill Dispenser", null, 1),
    };

        int addedCount = 0;

        foreach (var (primaryName, fallbackName, amount) in testItems)
        {
            ItemSO itemData = database.GetItem(primaryName);

            // Try fallback name if primary not found
            if (itemData == null && !string.IsNullOrEmpty(fallbackName))
            {
                itemData = database.GetItem(fallbackName);
            }

            if (itemData != null)
            {
                AddItem(new Item { itemData = itemData, amount = amount });
                addedCount++;
            }
            else
            {
                Debug.LogWarning($"Could not find item '{primaryName}'{(fallbackName != null ? $" or '{fallbackName}'" : "")}");
            }
        }

        hasAddedDefaultTestItems = true;

        Debug.Log($"Successfully added {addedCount} test items");
    }

    public void AddItem(Item item)
    {
        if (item.itemData.isStackable)
        {
            // Handle stackable items
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemData == item.itemData)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                    break; // Exit loop once found
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            // Handle unstackable items - add each as separate entry
            for (int i = 0; i < item.amount; i++)
            {
                Item newItem = new Item
                {
                    itemData = item.itemData,
                    amount = 1 // Each gets amount 1
                };
                itemList.Add(newItem);
            }
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        //Debug.Log($"Added {item.amount}x {item.itemData.itemName}. Total items: {itemList.Count}");
    }

    public void RemoveItem(Item item)
    {
        if (item.itemData.isStackable)
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemData == item.itemData)
                {
                    inventoryItem.amount -= item.amount;
                    if (inventoryItem.amount <= 0)
                    {
                        itemInInventory = inventoryItem;
                    }
                    break;
                }
            }
            if (itemInInventory != null)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            // For non-stackable, remove the first matching item
            itemList.Remove(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

}
