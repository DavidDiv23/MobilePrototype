using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public ItemSO itemData;
    public int amount;

    // Properties
    public Sprite GetSprite() => itemData?.icon;
    public bool IsStackable() => itemData?.isStackable ?? false;
    public string GetItemName() => itemData?.itemName ?? "Invalid Item";
    public string GetDescription() => itemData?.description ?? "No description available";
    public ItemCategory GetCategory() => itemData?.category ?? ItemCategory.Ingredient;
    public bool IsBlueprint() => GetCategory() == ItemCategory.Blueprint;
    public bool IsIngredient() => GetCategory() == ItemCategory.Ingredient;

    // Recipe methods
    public bool HasRecipes() => itemData?.recipes?.Count > 0;
    public List<ItemSO.CraftingRecipe> GetRecipes() => itemData?.recipes ?? new List<ItemSO.CraftingRecipe>();


    //Method 1
    /* public enum ItemType
     {
         Berry,
         Crystal,
         Feather,
         Leaf,
         Orange,
         Rock,
         Snail,
         Stick,
         Blueprint,
     }

     public ItemType itemtype;
     public int amount;

     public Sprite GetSprite()
     {
         switch(itemtype)
         {
             default:
             case ItemType.Berry:         return ItemAssets.Instance.berrySprite;
             case ItemType.Feather:       return ItemAssets.Instance.featherSprite;
             case ItemType.Crystal:       return ItemAssets.Instance.crystalSprite;
             case ItemType.Leaf:          return ItemAssets.Instance.leafSprite;
             case ItemType.Orange:        return ItemAssets.Instance.orangeSprite;
             case ItemType.Rock:          return ItemAssets.Instance.rockSprite;
             case ItemType.Snail:         return ItemAssets.Instance.snailSprite;
             case ItemType.Stick:         return ItemAssets.Instance.stickSprite;
             case ItemType.Blueprint:     return ItemAssets.Instance.blueprintSprite;

         }
     }

     public bool isStackable()
     {
         switch (itemtype)
         {
             default:
             case ItemType.Berry:
             case ItemType.Feather:
             case ItemType.Crystal:
             case ItemType.Leaf:
             case ItemType.Orange:
             case ItemType.Rock:
             case ItemType.Snail:
             case ItemType.Stick:
                 return true;
             case ItemType.Blueprint:
                 return false;
         }
     }

     public string GetItemName()
     {
         switch (itemtype)
         {
             case ItemType.Berry: return "Berry";
             case ItemType.Crystal: return "Crystal";
             case ItemType.Feather: return "Feather";
             case ItemType.Leaf: return "Medicinal Leaf";
             case ItemType.Orange: return "Orange";
             case ItemType.Rock: return "Rock";
             case ItemType.Snail: return "Snail Shell";
             case ItemType.Stick: return "Stick";
             case ItemType.Blueprint: return "Blueprint";
             default: return "Unknown Item";
         }
     }

     public string GetDescription()
     {
         switch (itemtype)
         {
             case ItemType.Berry:
                 return "Juicy red berry. Has health benefits.";
             case ItemType.Crystal:
                 return "Shiny river crystal. Used for crafting.";
             case ItemType.Feather:
                 return "Bawk Bawk gone pew pew sad.";
             case ItemType.Leaf:
                 return "Dried medicinal leaf. Has healing properties.";
             case ItemType.Orange:
                 return "Sweet citrus fruit. Can be used for flavoring.";
             case ItemType.Rock:
                 return "It's not just a boulder.";
             case ItemType.Snail:
                 return "The shell of a snail. Reminds you of a cinnamon roll.";
             case ItemType.Stick:
                 return "STICK STICK STICK STICK STICK STICK *bark bark*.";
             case ItemType.Blueprint:
                 return "Crafting recipe. Unlocks new technologies at the crafting bench.";
             default:
                 return "Idk man dont ask me";
         }
     }
    */
}
