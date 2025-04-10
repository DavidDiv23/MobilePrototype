using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;


    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");

        gameObject.SetActive(false);
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();
    }
    public void ToggleInventoryUI()
    {
        bool isActive = !gameObject.activeSelf;
        gameObject.SetActive(isActive);

        if (isActive)
        {
            RefreshInventoryItems();
        }
    }

    public void RefreshInventoryItems()
    {
        // Clear existing items first to prevent duplicates
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float itemSlotCellSizeX = 150f; // Horizontal spacing (keep your original value)
        float itemSlotCellSizeY = 180f; // New vertical spacing (increased from 150f)

        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            // Apply different spacing for X and Y axes
            itemSlotRectTransform.anchoredPosition = new Vector2(
                x * itemSlotCellSizeX,
                -y * itemSlotCellSizeY // Using the larger Y spacing
            );

            Image sprite = itemSlotRectTransform.Find("Sprite").GetComponent<Image>();
            sprite.sprite = item.GetSprite();

            TextMeshProUGUI uiText = itemSlotRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
            uiText.SetText("x" + item.amount.ToString());

            x++;
            if (x > 6) // 7 items per row (0-6)
            {
                x = 0;
                y++;
            }
        }
    }


}
