using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    private List<Item> itemList;
    public event EventHandler OnItemListChanged;

    public Inventory()
    {
        itemList = new List<Item>();
        Debug.Log("Inventory initialized");
    }

    // Improved test items method with error handling
    public void AddDefaultTestItems(ItemDatabaseSO database)
    {
        if (database == null)
        {
            Debug.LogError("Database reference is null!");
            return;
        }

        // Safe test items list with fallbacks
        var testItems = new List<(string name, string fallbackName, int amount)>()
    {
        ("Crystal", null, 1),
        ("Stick", "Wooden Stick", 3),
        ("Snail Shell", "Shell", 2),
        ("Orange", null, 1),
        ("Rock", "Stone", 5),
        ("Blueprint", "Crafting Blueprint", 1),
        ("Berry", "Red Berry", 10),
        ("Feather", "Bird Feather", 4)
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
        Debug.Log($"Added {item.amount}x {item.itemData.itemName}. Total items: {itemList.Count}");
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


    /*
    private List<Item> itemList;
   public Inventory()
    {
        itemList = new List<Item>();

        AddItem(new Item { itemtype = Item.ItemType.Crystal, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.Stick, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.Snail, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.Orange, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.Rock, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.Blueprint, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.Berry, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.Feather, amount = 1 });
        
        Debug.Log(itemList.Count);
    
        Debug.Log("Iventory");
    }

    public void AddItem(Item item)
    {
        if (item.isStackable())
        {
            // Handle stackable items (existing logic)
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemtype == item.itemtype)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
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
                    itemtype = item.itemtype,
                    amount = 1 // Each gets amount 1
                };
                itemList.Add(newItem);
            }
        }
        Debug.Log($"Added {item.amount}x {item.itemtype}. Total items: {itemList.Count}");
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
    */
}
