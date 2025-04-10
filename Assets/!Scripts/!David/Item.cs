using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public enum ItemType
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
}
