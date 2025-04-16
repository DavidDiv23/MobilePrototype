using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crafting : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] public Inventory inventory;
    public Button craftButton;

    [Header("Panel 1: Blueprint Inventory")]
    public Transform InventoryWindow;
    public GameObject blueprintSlotPrefab;

    [Header("Panel 2: Description")]
    public GameObject DescriptionWindow;
    public TextMeshProUGUI ItemName; // Changed from TMP_Text to TextMeshProUGUI
    public TextMeshProUGUI ItemDescription;

    [Header("Panel 3: Recipe Display")]
    public GameObject CraftableWindow;
    public Image ResultImage;
    public Image ItemSprite;
    public Transform recipeWindow;
    public GameObject recipeItemTemplate;

    private ItemSO selectedRecipeResult;
    private ItemSO.CraftingRecipe currentRecipe;
    private bool isInitialized = false;

    private void Initialize()
    {
        if (isInitialized) return;

        // Safely find references
        if (InventoryWindow == null)
        {
            InventoryWindow = transform.Find("InventoryWindow");
            if (InventoryWindow == null)
            {
                Debug.LogError("InventoryWindow not found!");
                return;
            }
        }

        if (blueprintSlotPrefab == null && InventoryWindow != null)
        {
            Transform template = InventoryWindow.Find("itemSlotTemplate") ??
                               InventoryWindow.Find("BlueprintSlotTemplate");
            if (template != null)
            {
                blueprintSlotPrefab = template.gameObject;
            }
            else
            {
                Debug.LogError("Slot template not found in InventoryWindow!");
            }
        }

        isInitialized = true;
    }

    private void Awake()
    {
        Initialize();
        gameObject.SetActive(false);

        // Safely disable windows
        if (DescriptionWindow != null)
            DescriptionWindow.SetActive(false);
        else
            Debug.LogWarning("DescriptionWindow reference not set!");

        if (CraftableWindow != null)
            CraftableWindow.SetActive(false);
        else
            Debug.LogWarning("CraftableWindow reference not set!");
    }

    private void OnEnable()
    {
        // Keep your existing OnEnable code
        if (inventory != null)
            inventory.OnItemListChanged += RefreshBlueprintInventory;

        if (craftButton != null)
            craftButton.onClick.AddListener(AttemptCraft);

        RefreshBlueprintInventory(this, null);
    }
    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnItemListChanged -= RefreshBlueprintInventory;

        if (craftButton != null)
            craftButton.onClick.RemoveListener(AttemptCraft);
    }

    public void ToggleCraftingUI()
    {
        if (!isInitialized) Initialize();

        bool newState = !gameObject.activeSelf;
        gameObject.SetActive(newState);

        if (newState)
        {
            // Only refresh if we have valid references
            if (InventoryWindow != null && blueprintSlotPrefab != null)
            {
                RefreshBlueprintInventory(this, null);
            }
            else
            {
                Debug.LogError("Cannot refresh - missing required references!");
                gameObject.SetActive(false);
            }
        }
    }

    void RefreshBlueprintInventory(object sender, System.EventArgs e)
    {
        // Add null checks at start
        if (InventoryWindow == null || blueprintSlotPrefab == null)
        {
            Debug.LogError("Missing required references for RefreshBlueprintInventory!");
            return;
        }

        // Clear existing blueprints safely
        foreach (Transform child in InventoryWindow)
        {
            if (child != null && child.gameObject != blueprintSlotPrefab)
            {
                Destroy(child.gameObject);
            }
        }

        // Add null check for inventory
        if (inventory == null)
        {
            Debug.LogError("Inventory reference not set!");
            return;
        }

        // Rest of your RefreshBlueprintInventory code...
        try
        {
            var blueprints = inventory.GetItemList().Where(item => item != null && item.IsBlueprint()).ToList();

            for (int i = 0; i < blueprints.Count; i++)
            {
                if (blueprintSlotPrefab != null && InventoryWindow != null)
                {
                    var slot = Instantiate(blueprintSlotPrefab, InventoryWindow).GetComponent<BlueprintSlotUI>();
                    if (slot != null)
                    {
                        slot.Setup(blueprints[i], this);
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error refreshing blueprint inventory: {ex.Message}");
        }
    }

    public void ShowBlueprintDetails(Item blueprint)
    {
        if (!blueprint.HasRecipes()) return;

        currentRecipe = blueprint.GetRecipes()[0];
        selectedRecipeResult = currentRecipe.resultItem;

        // Panel 2: Update Description
        DescriptionWindow.SetActive(true);
        ItemName.text = selectedRecipeResult.itemName;
        ItemDescription.text = selectedRecipeResult.description;

        // Panel 3: Update Recipe Display
        CraftableWindow.SetActive(true);

        // Update Result Item
        ItemSprite.sprite = selectedRecipeResult.icon;
        ItemSprite.preserveAspect = true;

        // Clear old recipe items
        foreach (Transform child in recipeWindow)
        {
            if (child.gameObject != recipeItemTemplate)
                Destroy(child.gameObject);
        }

        // Add new recipe requirements
        foreach (var requirement in currentRecipe.requirements)
        {
            var requirementUI = Instantiate(recipeItemTemplate, recipeWindow).GetComponent<RecipeItemUI>();
            requirementUI.gameObject.SetActive(true);

            int playerAmount = inventory.GetItemList()
                .Where(i => i.itemData == requirement.item)
                .Sum(i => i.amount);

            requirementUI.Setup(requirement.item.icon,
                              $"{playerAmount}/{requirement.amount}",
                              playerAmount >= requirement.amount);
        }

        // Update craft button
        craftButton.interactable = CanCraft();
    }

    bool CanCraft()
    {
        if (currentRecipe == null) return false;

        foreach (var req in currentRecipe.requirements)
        {
            int total = inventory.GetItemList()
                .Where(i => i.itemData == req.item)
                .Sum(i => i.amount);

            if (total < req.amount) return false;
        }
        return true;
    }

    void AttemptCraft()
    {
        if (!CanCraft() || currentRecipe == null) return;

        // Remove required items
        foreach (var req in currentRecipe.requirements)
        {
            inventory.RemoveItem(new Item
            {
                itemData = req.item,
                amount = req.amount
            });
        }

        // Add crafted item
        inventory.AddItem(new Item
        {
            itemData = currentRecipe.resultItem,
            amount = currentRecipe.resultAmount
        });

        // Refresh UI
        ShowBlueprintDetails(new Item
        {
            itemData = selectedRecipeResult,
            amount = 1
        });
    }
}