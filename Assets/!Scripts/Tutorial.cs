using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public NavMeshAgent playerAgent;
    public GameObject panelUI;
    public GameObject hospitalEntrance;
    public GameObject hospitalButton;
    public GameObject exclamationMark;
    public GameObject[] unlockableWords;
    public GameObject plantPanel;
    public GameObject UICraftingWindow;
    public GameObject arrow1;
    public GameObject arrow2;
    public DialogueRunner dialogueRunner;
    public string dialogueNodeName;
    
    
    public Inventory inventory;
    public InventoryManager inventoryManager;
    public ItemSO plantItemSO;
    public ItemSO pillsBlueprint;
    
    private InMemoryVariableStorage yarnVariables;
    public UI_Crafting uiCrafting;

    private void Start()
    {
        panelUI.SetActive(false);
        hospitalButton.SetActive(false);
        arrow1.SetActive(false);
        arrow2.SetActive(false);
        yarnVariables = dialogueRunner.VariableStorage as InMemoryVariableStorage;
        
        dialogueRunner.onDialogueComplete.AddListener(CheckIntroComplete);
        dialogueRunner.onDialogueComplete.AddListener(PlantCanvaShowUp);
        dialogueRunner.onDialogueComplete.AddListener(OpenCraftingWindow);
        
    }

    private void PlantCanvaShowUp()
    {
        if (dialogueRunner.CurrentNodeName == "ManagerDialogueAfterAnya")
        {
            plantPanel.SetActive(true);
            Item blueprintItem = new Item
            {
                itemData = pillsBlueprint,
                amount = 1
            };
            inventory.AddItem(blueprintItem);
            
            StartCoroutine(PlayNextDialogue("ManagerDialogueForPills"));
        }
    }

    private IEnumerator PlayNextDialogue(string nextNode)
    {
        yield return null;
        dialogueRunner.StartDialogue(nextNode);

        Item harvestedItem = new Item
        {
            itemData = plantItemSO,
            amount = 3
        };
        inventoryManager.AddItem(harvestedItem);
        UICraftingWindow.SetActive(true);
        arrow1.SetActive(true);
        arrow2.SetActive(true);
    }

    public void HideButtons()
    {
        plantPanel.SetActive(false);
    }

    private void OpenCraftingWindow()
    {
        if (dialogueNodeName == "ManagerDialogueForPills")
        {
            uiCrafting.gameObject.SetActive(true);
        }
    }

    public void StartingDialogue()
    {
        var variableStorage = dialogueRunner.VariableStorage;

        if (dialogueNodeName == "ManagerDialogue" || dialogueNodeName == "ManagerDialogueAfterAnya")
        {
            variableStorage.TryGetValue("$popUpScreenTut", out bool popUpScreenTut);
            
            if (popUpScreenTut)
            {
                dialogueRunner.StartDialogue("ManagerDialogueAfterAnya");
            }
            else
            {
                dialogueRunner.StartDialogue("ManagerDialogue");
            }
        }
        else
        {
            dialogueRunner.StartDialogue(dialogueNodeName);
        }
    }

    public void CheckIntroComplete()
    {
        if (dialogueRunner.CurrentNodeName == "AnyaIntro" &&
            yarnVariables != null &&
            yarnVariables.TryGetValue("$popUpScreenTut", out bool hasFinished) &&
            hasFinished)
        {
            panelUI.SetActive(true);
            hospitalButton.SetActive(true);
            dialogueNodeName = "ManagerDialogueAfterAnya";
        }
    }

    
    public void GoToEntrance()
    {
        playerAgent.SetDestination(hospitalEntrance.transform.position);
    }

    public void DisableButton()
    {
        hospitalButton.SetActive(false);
        panelUI.SetActive(false);
    }
    
    private void OnEnable()
    {
        UI_Crafting.OnItemCrafted += OnPlantBlueprintCrafted;
    }

    private void OnPlantBlueprintCrafted(ItemSO obj)
    {
        if (exclamationMark != null) exclamationMark.SetActive(true);
        foreach (var word in unlockableWords)
        {
            word.SetActive(true);
        }
    }

    private void OnDisable()
    {
        UI_Crafting.OnItemCrafted -= OnPlantBlueprintCrafted;
    }
}
