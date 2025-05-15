using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crafting : MonoBehaviour
{
    
    public static event Action<ItemSO> OnItemCrafted;
    [Header("References")]
    [SerializeField] private Inventory inventory; 
    [SerializeField] private Button craftButton;

    [Header("Panel 1: Blueprint Inventory")]
    public Transform InventoryWindow;
    public GameObject blueprintSlotPrefab;

    [Header("Panel 2: Description")]
    public GameObject DescriptionWindow;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;

    [Header("Panel 3: Recipe Display")]
    public GameObject CraftableWindow; 
    public Image ResultItemSprite;
    public Transform recipeWindow;
    public GameObject recipeItemTemplate;

    [Header("Grid Settings - Blueprint Inventory")]
    public float leftPadding = 20f;
    public float topPadding = 20f;
    public int blueprintItemsPerRow = 4;
    public float blueprintSlotSizeX = 100f;
    public float blueprintSlotSizeY = 120f;
    public float blueprintHorizontalSpacing = 15f;
    public float blueprintVerticalSpacing = 20f;
    // Remove this line ↓
    // public Vector2 blueprintStartOffset = new Vector2(20, -20);
    
    [Header("Grid Settings - Recipe Window")]
    public float recipeLeftPadding = 15f;
    public float recipeTopPadding = 15f;
    public int recipeItemsPerRow = 2;
    public float recipeSlotSizeX = 80f;
    public float recipeSlotSizeY = 100f;
    public float recipeHorizontalSpacing = 10f;
    public float recipeVerticalSpacing = 15f;
    // Remove this line ↓
    // public Vector2 recipeStartOffset = new Vector2(10, -10);

    private ItemSO selectedRecipeResult;
    private ItemSO.CraftingRecipe currentRecipe;
    private bool isInitialized = false;

    private void Start()
    {
        // Initial refresh
        RefreshBlueprintInventory(this, EventArgs.Empty);
    }

    private void UpdateCraftingState()
    {
        // Update both windows
        RefreshBlueprintInventory(this, EventArgs.Empty);
        if (currentRecipe != null)
        {
            ShowBlueprintDetails(new Item { itemData = selectedRecipeResult });
        }
    }

    // New method to set inventory from InventoryManager
    public void SetInventory(Inventory inventory)
    {
        // Unregister from previous inventory if exists
        if (this.inventory != null)
            this.inventory.OnItemListChanged -= RefreshBlueprintInventory;

        this.inventory = inventory;

        // Register with new inventory
        if (this.inventory != null)
        {
            this.inventory.OnItemListChanged += RefreshBlueprintInventory;
            if (gameObject.activeSelf)
                RefreshBlueprintInventory(this, EventArgs.Empty);
        }
    }

    private void Initialize()
    {
        if (isInitialized) return;

        // Find InventoryWindow even if inactive
        if (InventoryWindow == null)
        {
            InventoryWindow = transform.Find("InventoryWindow");
            if (InventoryWindow == null)
                Debug.LogError("InventoryWindow not found in children!", this);
        }

        // Find template using GetComponentsInChildren (include inactive)
        if (blueprintSlotPrefab == null && InventoryWindow != null)
        {
            Transform template = InventoryWindow
                .GetComponentsInChildren<Transform>(true)
                .FirstOrDefault(t => t.name == "itemSlotTemplate");

            if (template != null)
            {
                blueprintSlotPrefab = template.gameObject;
                blueprintSlotPrefab.SetActive(false); // Keep template hidden
                Debug.Log($"Found template: {blueprintSlotPrefab.name}");
            }
            else
            {
                Debug.LogError("itemSlotTemplate not found!", this);
            }
        }

        isInitialized = true;
    }

    private void Awake()
    {
        Initialize();
        gameObject.SetActive(false);

        // Safely disable windows
        DescriptionWindow?.SetActive(false);
        CraftableWindow?.SetActive(false);
    }

    private void OnEnable()
    {
        if (!isInitialized) Initialize();

        if (inventory == null)
        {
            Debug.LogError("Inventory not set! Crafting UI disabled.", this);
            gameObject.SetActive(false);
            return;
        }

        // Set up listeners
        inventory.OnItemListChanged += RefreshBlueprintInventory;
        craftButton?.onClick.AddListener(AttemptCraft);

        // Refresh if inventory is valid
        if (inventory != null)
        {
            RefreshBlueprintInventory(this, EventArgs.Empty);
        }
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnItemListChanged -= RefreshBlueprintInventory;

        craftButton?.onClick.RemoveListener(AttemptCraft);
    }

    public void ToggleCraftingUI()
    {
        bool newState = !gameObject.activeSelf;
        gameObject.SetActive(newState);

        if (newState)
        {
            // Force refresh inventory check
            StartCoroutine(DelayedRefresh());
        }
        UpdateCraftingState();
    }
    private IEnumerator DelayedRefresh()
    {
        yield return null; // Wait one frame for UI to initialize
        RefreshBlueprintInventory(this, EventArgs.Empty);
    }

    void RefreshBlueprintInventory(object sender, EventArgs e)
    {
        Debug.Log("=== RefreshBlueprintInventory Start ===");

        // Check 1: Critical references
        if (inventory == null) Debug.LogError("Inventory is null!");
        if (InventoryWindow == null) Debug.LogError("InventoryWindow is null!");
        if (blueprintSlotPrefab == null) Debug.LogError("blueprintSlotPrefab is null!");

        try
        {
            // Check 2: Clear existing slots
            Debug.Log($"Clearing children of {InventoryWindow.name}");
            foreach (Transform child in InventoryWindow)
            {
                if (child != null && child.gameObject != blueprintSlotPrefab)
                {
                    Debug.Log($"Destroying {child.name}");
                    Destroy(child.gameObject);
                }
            }

            // Check 3: Get inventory items
            var items = inventory?.GetItemList();
            //Debug.Log($"Total items in inventory: {items?.Count ?? 0}");

            // Filter blueprints
            var blueprints = items
                .Where(item => item != null && item.IsBlueprint())
                .ToList();

            Debug.Log($"Found {blueprints.Count} blueprints in inventory");

            // Create blueprint slots
            for (int i = 0; i < blueprints.Count; i++)
            {
                Item currentBlueprint = blueprints[i];

                GameObject slotObj = Instantiate(blueprintSlotPrefab, InventoryWindow);
                slotObj.SetActive(true);

                // === NEW POSITIONING CODE START ===
                // Get container dimensions
                RectTransform containerRect = InventoryWindow.GetComponent<RectTransform>();
                float containerWidth = containerRect.rect.width;
                float containerHeight = containerRect.rect.height;

                // Calculate starting position (top-left origin accounting for center pivot)
                float startX = -containerWidth / 2 + leftPadding + blueprintSlotSizeX / 2;
                float startY = containerHeight / 2 - topPadding - blueprintSlotSizeY / 2;

                // Grid calculations
                int row = i / blueprintItemsPerRow;
                int col = i % blueprintItemsPerRow;

                // Calculate position with spacing
                float posX = startX + col * (blueprintSlotSizeX + blueprintHorizontalSpacing);
                float posY = startY - row * (blueprintSlotSizeY + blueprintVerticalSpacing);

                // Set position
                RectTransform rect = slotObj.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(posX, posY);
                // === NEW POSITIONING CODE END ===

                // Rest of your existing slot setup code remains the same...
                var slot = slotObj.GetComponent<BlueprintSlotUI>();
                if (slot == null) continue;
                slot.Setup(currentBlueprint, this);
            }


            // Update UI visibility
            bool hasBlueprints = blueprints.Count > 0;
            DescriptionWindow?.SetActive(hasBlueprints);
            CraftableWindow?.SetActive(hasBlueprints);
        }
        catch (Exception ex)
        {
            Debug.LogError($"CRASH POINT: {ex.Message}");
        }
    }

    public void ShowBlueprintDetails(Item blueprint)
    {
        // Early exit for invalid blueprints
        if (blueprint == null || !blueprint.HasRecipes())
        {
            Debug.LogError("Invalid blueprint or missing ingrei!", this);
            return;
        }

        // Get recipes safely
        var recipes = blueprint.GetRecipes();
        if (recipes == null || recipes.Count == 0)
        {
            Debug.LogError("No recipes found in blueprint!", this);
            return;
        }

        // Set current recipe
        currentRecipe = recipes[0];
        selectedRecipeResult = currentRecipe?.resultItem;

        // Validate recipe result
        if (selectedRecipeResult == null)
        {
            Debug.LogError("Recipe result item is null!", this);
            return;
        }

        // Update description panel
        DescriptionWindow.SetActive(true);
        ItemName.text = selectedRecipeResult.itemName;
        ItemDescription.text = selectedRecipeResult.description;

        // Update recipe display panel
        CraftableWindow.SetActive(true);
        ResultItemSprite.sprite = selectedRecipeResult.icon;
        ResultItemSprite.preserveAspect = true;

        // Clear previous recipe items
        foreach (Transform child in recipeWindow)
        {
            if (child.gameObject != recipeItemTemplate)
                Destroy(child.gameObject);
        }

        // Create new recipe requirements
        foreach (var requirement in currentRecipe.requirements)
        {
            if (recipeItemTemplate == null)
            {
                Debug.LogError("RecipeItemTemplate is null!", this);
                continue;
            }

            var requirementObj = Instantiate(recipeItemTemplate, recipeWindow);
            requirementObj.SetActive(true);

            var requirementUI = requirementObj.GetComponent<RecipeItemUI>();
            if (requirementUI == null)
            {
                Debug.LogError("Missing RecipeItemUI component", requirementObj);
                continue;
            }

            // Get requirement data
            var requirementSprite = requirement.item?.icon != null
                ? requirement.item.icon
                : Resources.Load<Sprite>("DefaultIcon");

            var playerAmount = inventory.GetItemList()
                .Where(i => i.itemData == requirement.item)
                .Sum(i => i.amount);

            // Setup UI element
            requirementUI.Setup(
                requirementSprite,
                $"{playerAmount}/{requirement.amount}",
                playerAmount >= requirement.amount
            );

            // === CORRECTED POSITIONING CODE ===
            // Get container rect
            RectTransform recipeContainerRect = recipeWindow.GetComponent<RectTransform>();

            // Calculate starting position based on top-left pivot
            float startX = recipeLeftPadding + (recipeSlotSizeX / 2);
            float startY = recipeTopPadding + (recipeSlotSizeY / 2);

            // Grid calculations
            int index = currentRecipe.requirements.IndexOf(requirement);
            int row = index / recipeItemsPerRow;
            int col = index % recipeItemsPerRow;

            // Calculate position with spacing (positive Y moves downward)
            float posX = startX + col * (recipeSlotSizeX + recipeHorizontalSpacing);
            float posY = startY + row * (recipeSlotSizeY + recipeVerticalSpacing);

            // Set position
            RectTransform rect = requirementObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(posX, posY);
            // === END CORRECTED CODE ===
        }

        // Update craft button state after creating all requirements
        craftButton.interactable = CanCraft();
    }

    // Moved outside ShowBlueprintDetails as class-level methods
    private bool CanCraft()
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

    private void AttemptCraft()
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
            OnItemCrafted?.Invoke(currentRecipe.resultItem);
        }

        // Add crafted item
        inventory.AddItem(new Item
        {
            itemData = currentRecipe.resultItem,
            amount = currentRecipe.resultAmount
        });

        // Refresh UI with new blueprint state
        ShowBlueprintDetails(new Item
        {
            itemData = selectedRecipeResult,
            amount = 1
        });

        UpdateCraftingState();
    }
}