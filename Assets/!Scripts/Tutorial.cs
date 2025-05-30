using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public Inventory inventory;
    public InventoryManager inventoryManager;
    public ItemSO plantItemSO;
    public ItemSO pillsBlueprint;
    public UI_Crafting uiCrafting;

    private InMemoryVariableStorage yarnVariables;

    private class DialogueStep
    {
        public string NodeName;
        public string CompletionVariable;
        public Action PostDialogueAction;

        public DialogueStep(string nodeName, string completionVar, Action postAction = null)
        {
            NodeName = nodeName;
            CompletionVariable = completionVar;
            PostDialogueAction = postAction;
        }
    }

    private List<DialogueStep> dialogueSteps;
    private DialogueStep currentStep;

    private void Awake()
    {
        yarnVariables = dialogueRunner.VariableStorage as InMemoryVariableStorage;

        dialogueSteps = new List<DialogueStep>
        {
            new DialogueStep("ManagerDialogue", "$finishedManagerDialogue"),
            new DialogueStep("ManagerDialogueAfterAnya", "$finishedAfterAnya", () =>
            {

                inventory.AddItem(new Item { itemData = pillsBlueprint, amount = 1 });
            }),
            new DialogueStep("ManagerDialogueForPills", "$finishedPillDialogue", () =>
            {
                plantPanel.SetActive(true);
                inventoryManager.AddItem(new Item { itemData = plantItemSO, amount = 3 });
                UICraftingWindow.SetActive(true);
                arrow1.SetActive(true);
                arrow2.SetActive(true);
                uiCrafting.gameObject.SetActive(true);
            }),
        };
    }

    private void Start()
    {
        panelUI.SetActive(false);
        hospitalButton.SetActive(false);
        arrow1.SetActive(false);
        arrow2.SetActive(false);

        dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
    }

    public void StartingDialogue()
    {
        if (yarnVariables == null)
            yarnVariables = dialogueRunner.VariableStorage as InMemoryVariableStorage;

        foreach (var step in dialogueSteps)
        {
            if (!yarnVariables.TryGetValue(step.CompletionVariable, out bool isDone) || !isDone)
            {
                currentStep = step;
                dialogueRunner.StartDialogue(step.NodeName);
                return;
            }
        }
        currentStep = null;
        dialogueRunner.StartDialogue("RepeatOrFallbackDialogue");
    }

    private void OnDialogueComplete()
    {
        if (dialogueRunner.CurrentNodeName == "AnyaIntro" &&
            yarnVariables != null &&
            yarnVariables.TryGetValue("$popUpScreenTut", out bool hasFinished) &&
            hasFinished)
        {
            panelUI.SetActive(true);
            hospitalButton.SetActive(true);
        }
        
        currentStep?.PostDialogueAction?.Invoke();
        currentStep = null;
    }

    public void HideButtons()
    {
        plantPanel.SetActive(false);
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

    private void OnDisable()
    {
        UI_Crafting.OnItemCrafted -= OnPlantBlueprintCrafted;
    }

    private void OnPlantBlueprintCrafted(ItemSO obj)
    {
        if (exclamationMark != null) exclamationMark.SetActive(true);
        foreach (var word in unlockableWords)
        {
            word.SetActive(true);
        }
    }
}
