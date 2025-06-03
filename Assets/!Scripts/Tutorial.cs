using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
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
    public GameObject patientLog;
    
    public Inventory inventory;
    public InventoryManager inventoryManager;
    public ItemSO plantItemSO;
    public ItemSO pillsBlueprint;
    public UI_Crafting uiCrafting;

    public DialogueRunner dialogueRunner;
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
            }),
            new DialogueStep("CraftingPills", "$finishedCraftingDialogue", () =>
            {
                UICraftingWindow.SetActive(true);
                arrow1.SetActive(true);
                arrow2.SetActive(true);
            }),
            new DialogueStep("AdministratingPills", "$readyToAdministerPills"),
            new DialogueStep("PatientLog", "$finishedPatientLogDialogue", () =>
            {
                patientLog.SetActive(true);
                foreach (var word in unlockableWords)
                {
                    word.SetActive(true);
                }
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

    public void StartCraftingDialogue()
    {
        currentStep = dialogueSteps.Find(step => step.NodeName == "CraftingPills");
        if (currentStep != null)
        {
            dialogueRunner.StartDialogue(currentStep.NodeName);
        }
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
        arrow1.SetActive(false);
        arrow2.SetActive(false);
        
        StartDialogueByNodeName("AdministratingPills");
    }
    private void StartDialogueByNodeName(string nodeName)
    {
        currentStep = dialogueSteps.Find(step => step.NodeName == nodeName);
        if (currentStep != null)
        {
            dialogueRunner.StartDialogue(currentStep.NodeName);
        }
    }
}
