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
    public GameObject panelUI;
    public NavMeshAgent playerAgent;
    public GameObject hospitalEntrance;
    public DialogueRunner dialogueRunner;
    public string dialogueNodeName;
    public GameObject hospitalButton;
    
    public GameObject exclamationMark;
    public GameObject[] unlockableWords;
    
    public Inventory inventory;
    public ItemSO pillsBlueprint;

    public GameObject treatingCanva;
    private InMemoryVariableStorage yarnVariables;

    private void Start()
    {
        panelUI.SetActive(false);
        hospitalButton.SetActive(false);
        yarnVariables = dialogueRunner.VariableStorage as InMemoryVariableStorage;
        
        dialogueRunner.onDialogueComplete.AddListener(CheckIntroComplete);
        dialogueRunner.onDialogueComplete.AddListener(CraftingBlueprint);
    }
    

    private void CraftingBlueprint()
    {
        if (dialogueRunner.CurrentNodeName == "ManagerDialogueAfterAnya")
        {
            Item blueprintItem = new Item
            {
                itemData = pillsBlueprint,
                amount = 1
            };
            inventory.AddItem(blueprintItem);
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
