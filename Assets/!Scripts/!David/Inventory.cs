using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
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
        if(item.isStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach(Item inventoryItem in itemList)
            {
                if(inventoryItem.itemtype == item.itemtype)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if(!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }

    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
