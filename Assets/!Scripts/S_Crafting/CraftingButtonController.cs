using UnityEngine;
using UnityEngine.UI;

public class CraftingButtonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button craftingButton;
    [SerializeField] private UI_Crafting craftingUI;

    void Start()
    {
        // Auto-assign references if not set
        if (craftingButton == null)
            craftingButton = GetComponent<Button>();

        if (craftingUI == null)
            craftingUI = FindObjectOfType<UI_Crafting>(true);

        // Add click listener
        if (craftingButton != null && craftingUI != null)
        {
            craftingButton.onClick.AddListener(ToggleCraftingUI);
        }
        else
        {
            Debug.LogError("Missing references in CraftingButtonController", this);
        }

        // Ensure UI starts closed
        if (craftingUI != null)
            craftingUI.gameObject.SetActive(false);
    }

    public void ToggleCraftingUI()
    {
        if (craftingUI != null)
        {
            craftingUI.ToggleCraftingUI();
        }
    }
}