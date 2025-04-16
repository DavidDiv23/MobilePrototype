using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
   
    public bool isStackable = true;

    [Header("Category")]
    public ItemCategory category;  // This is the category you can assign in inspector

    [Header("Blueprint Specific")]
    [Tooltip("Only for Blueprint category items")]
    public bool isLearnedByDefault = false;
    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();

    [System.Serializable]
    public class CraftingRecipe
    {
        public ItemSO resultItem;
        public int resultAmount = 1;
        public List<ItemRequirement> requirements;
        

        [System.Serializable]
        public class ItemRequirement
        {
            public ItemSO item;
            public int amount;
        }
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        // Hide blueprint-specific fields if not a blueprint
        if (category != ItemCategory.Blueprint)
        {
            isLearnedByDefault = false;
            recipes.Clear();
        }
    }
#endif
    // You can add more properties as needed
    // Example: public ItemCategory category;
    // Example: public GameObject prefab;
}
