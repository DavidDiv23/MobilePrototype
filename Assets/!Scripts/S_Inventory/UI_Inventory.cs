using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;

    [Header("Inventory Grid Settings")]
    public int ItemsPerRow = 8;
    public float itemSlotCellSizeX = 150f;
    public float itemSlotCellSizeY = 180f;

    [Header("Grid Positioning")]
    [Tooltip("Left padding from the container edge")]
    public float leftPadding = 20f;
    [Tooltip("Top padding from the container edge")]
    public float topPadding = 20f;
    [Tooltip("Horizontal spacing between items")]
    public float horizontalSpacing = 10f;
    [Tooltip("Vertical spacing between items")]
    public float verticalSpacing = 10f;

    [Header("Item Slot References")]
    public Transform itemSlotContainer;
    public GameObject itemSlotPrefab;

    [Header("Description Window")]
    public GameObject descriptionWindow;
    public Image descriptionImage;
    public TextMeshProUGUI descriptionName;
    public TextMeshProUGUI descriptionText;
    private Item selectedItem;

    private void Awake()
    {
        // Initialize references if not set in inspector
        if (itemSlotContainer == null)
            itemSlotContainer = transform.Find("itemSlotContainer");

        if (itemSlotPrefab == null && itemSlotContainer != null)
            itemSlotPrefab = itemSlotContainer.Find("itemSlotTemplate").gameObject;

        gameObject.SetActive(false);
        if (descriptionWindow != null)
            descriptionWindow.SetActive(false);
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
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
        else
        {
            HideDescription();
        }
    }

    public void RefreshInventoryItems()
    {
        // Clear existing items first to prevent duplicates
        foreach (Transform child in itemSlotContainer)
        {
            if (child.gameObject == itemSlotPrefab) continue;
            Destroy(child.gameObject);
        }

        // Get the container's dimensions
        RectTransform containerRect = itemSlotContainer.GetComponent<RectTransform>();
        float containerWidth = containerRect.rect.width;
        float containerHeight = containerRect.rect.height;

        // Calculate starting position (top-left corner with padding)
        float startX = -containerWidth / 2 + leftPadding + itemSlotCellSizeX / 2;
        float startY = containerHeight / 2 - topPadding - itemSlotCellSizeY / 2;

        int index = 0;
        foreach (Item item in inventory.GetItemList())
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, itemSlotContainer);
            itemSlot.SetActive(true);

            // Calculate position with spacing
            int row = index / ItemsPerRow;
            int col = index % ItemsPerRow;

            float posX = startX + col * (itemSlotCellSizeX + horizontalSpacing);
            float posY = startY - row * (itemSlotCellSizeY + verticalSpacing);

            // Position the item
            RectTransform rectTransform = itemSlot.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(posX, posY);
            rectTransform.sizeDelta = new Vector2(itemSlotCellSizeX, itemSlotCellSizeY);

            // Get references
            Image slotImage = itemSlot.transform.Find("Image")?.GetComponent<Image>();
            TextMeshProUGUI amountText = itemSlot.transform.Find("amount")?.GetComponent<TextMeshProUGUI>();
            Button slotButton = itemSlot.GetComponent<Button>();

            // Set visuals
            if (slotImage != null)
            {
                slotImage.sprite = item.itemData.icon;
                slotImage.preserveAspect = true;
                slotImage.enabled = item.itemData.icon != null;
            }

            if (amountText != null)
            {
                amountText.text = "x" + item.amount.ToString(); // Display amount for all items
                amountText.gameObject.SetActive(true); // Ensure the amount text is always visible
            }

            // Button setup
            if (slotButton != null)
            {
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() => ShowItemDescription(item));
            }

            index++;
        }
    }

    private void ShowItemDescription(Item item)
    {
        selectedItem = item;

        if (descriptionWindow != null)
        {
            descriptionWindow.SetActive(true);

            if (descriptionImage != null)
            {
                descriptionImage.sprite = item.itemData.icon;
                descriptionImage.preserveAspect = true;
            }

            if (descriptionName != null)
                descriptionName.text = item.itemData.itemName;

            if (descriptionText != null)
                descriptionText.text = item.itemData.description;
        }
    }

    public void HideDescription()
    {
        if (descriptionWindow != null)
        {
            descriptionWindow.SetActive(false);
        }
        selectedItem = null;
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnItemListChanged -= Inventory_OnItemListChanged;
        }
    }



    /*private Inventory inventory;

    [Header("Inventory Grid Settings")]
    public int ItemsPerRow = 8;
    public float itemSlotCellSizeX = 150f;
    public float itemSlotCellSizeY = 180f;

    [Header("Grid Positioning")]
    [Tooltip("Left padding from the container edge")]
    public float leftPadding = 20f;
    [Tooltip("Top padding from the container edge")]
    public float topPadding = 20f;
    [Tooltip("Horizontal spacing between items")]
    public float horizontalSpacing = 10f;
    [Tooltip("Vertical spacing between items")]
    public float verticalSpacing = 10f;

    [Header("Item Slot References")]
    public Transform itemSlotContainer;
    public GameObject itemSlotPrefab; // Drag your item slot template here

    [Header("Item Slot Components")]
    public Image itemSlotImage;
    public TextMeshProUGUI itemSlotAmountText;
    public Button itemSlotButton; // Reference to the button component

    [Header("Description Window")]
    public GameObject descriptionWindow;
    public Image descriptionImage;
    public TextMeshProUGUI descriptionName;
    public TextMeshProUGUI descriptionText;
    private Item selectedItem;

    private void Awake()
    {
        // Initialize references if not set in inspector
        if (itemSlotContainer == null)
            itemSlotContainer = transform.Find("itemSlotContainer");

        if (itemSlotPrefab == null && itemSlotContainer != null)
            itemSlotPrefab = itemSlotContainer.Find("itemSlotTemplate").gameObject;

        gameObject.SetActive(false);
        if (descriptionWindow != null)
            descriptionWindow.SetActive(false);
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
        else
        {
            HideDescription();
        }
    }

    public void RefreshInventoryItems()
    {
        // Clear existing items first to prevent duplicates
        foreach (Transform child in itemSlotContainer)
        {
            if (child.gameObject == itemSlotPrefab) continue;
            Destroy(child.gameObject);
        }

        // Get the container's dimensions
        RectTransform containerRect = itemSlotContainer.GetComponent<RectTransform>();
        float containerWidth = containerRect.rect.width;
        float containerHeight = containerRect.rect.height;

        // Calculate starting position (top-left corner with padding)
        float startX = -containerWidth / 2 + leftPadding + itemSlotCellSizeX / 2;
        float startY = containerHeight / 2 - topPadding - itemSlotCellSizeY / 2;

        int index = 0;
        foreach (Item item in inventory.GetItemList())
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, itemSlotContainer);
            itemSlot.SetActive(true);

            // Calculate position with spacing
            int row = index / ItemsPerRow;
            int col = index % ItemsPerRow;

            float posX = startX + col * (itemSlotCellSizeX + horizontalSpacing);
            float posY = startY - row * (itemSlotCellSizeY + verticalSpacing);

            // Position the item
            RectTransform rectTransform = itemSlot.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(posX, posY);
            rectTransform.sizeDelta = new Vector2(itemSlotCellSizeX, itemSlotCellSizeY);

            // Get references - IMPORTANT: Don't use fallback references here
            Transform spriteTransform = itemSlot.transform.Find("Image");
            Image slotImage = spriteTransform != null ? spriteTransform.GetComponent<Image>() : null;

            Transform amountTransform = itemSlot.transform.Find("amount");
            TextMeshProUGUI amountText = amountTransform != null ? amountTransform.GetComponent<TextMeshProUGUI>() : null;

            Button slotButton = itemSlot.GetComponent<Button>();

            // Debug output to verify sprite assignment
            Debug.Log($"Assigning sprite to item {index}: {item.GetSprite()} (Item: {item.GetItemName()})");

            // Set visuals
            if (slotImage != null)
            {
                slotImage.sprite = item.GetSprite();
                slotImage.preserveAspect = true;
                slotImage.enabled = item.GetSprite() != null; // Disable if no sprite
            }
            else
            {
                Debug.LogError("Could not find Image component in item slot prefab!");
            }

            if (amountText != null)
            {
                // Always show amount for all items
                amountText.text = "x" + item.amount.ToString();
                amountText.gameObject.SetActive(true);
            }

            // Button setup
            if (slotButton != null)
            {
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() => ShowItemDescription(item));
            }

            index++;
        }
    }

    private void ShowItemDescription(Item item)
    {
        selectedItem = item;

        if (descriptionWindow != null)
        {
            descriptionWindow.SetActive(true);

            if (descriptionImage != null)
            {
                descriptionImage.sprite = item.GetSprite();
                descriptionImage.preserveAspect = true;
            }

            if (descriptionName != null)
                descriptionName.text = item.GetItemName();

            if (descriptionText != null)
                descriptionText.text = item.GetDescription();
        }
    }

    public void HideDescription()
    {
        if (descriptionWindow != null)
        {
            descriptionWindow.SetActive(false);
        }
        selectedItem = null;
    }
    */
}