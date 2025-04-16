using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueprintSlotUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Button button;
    private Item blueprint;
    private UI_Crafting craftingUI;

    public void Setup(Item blueprintItem, UI_Crafting ui)
    {
        blueprint = blueprintItem;
        craftingUI = ui;
        icon.sprite = blueprint.itemData.icon;
        button.onClick.AddListener(OnClicked);
    }

    void OnClicked()
    {
        craftingUI.ShowBlueprintDetails(blueprint);
    }
}
