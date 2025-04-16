using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items = new List<ItemSO>();

    public ItemSO GetItem(string itemName)
    {
        // Null check the items list first
        if (items == null)
        {
            Debug.LogError("Items list is null in database!");
            return null;
        }

        // Find the first non-null item with matching name
        return items.Find(item => item != null && item.itemName == itemName);
    }

    // Editor validation
#if UNITY_EDITOR
    void OnValidate()
    {
        if (items == null) return;

        // Remove any null entries
        items.RemoveAll(item => item == null);

        // Check for duplicate names
        var names = new HashSet<string>();
        foreach (var item in items)
        {
            if (item == null) continue;

            if (names.Contains(item.itemName))
            {
                Debug.LogWarning($"Duplicate item name: {item.itemName}");
            }
            names.Add(item.itemName);
        }
    }
#endif
}
