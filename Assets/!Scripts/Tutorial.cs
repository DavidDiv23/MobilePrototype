using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Yarn.Unity;

public class Tutorial : MonoBehaviour
{
    [Header("Core Systems")]
    [SerializeField] private NavMeshAgent playerAgent;
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private FaceAnya faceAnya;

    [Header("UI & Interaction")]
    [SerializeField] private GameObject panelUI;
    [SerializeField] private GameObject hospitalEntrance;
    [SerializeField] private GameObject hospitalButton;
    [SerializeField] private GameObject exclamationMark;
    [SerializeField] private GameObject[] unlockableWords;
    [SerializeField] private GameObject plantPanel;
    [SerializeField] private GameObject UICraftingWindow;
    [SerializeField] private GameObject arrow1;
    [SerializeField] private GameObject arrow2;
    [SerializeField] private GameObject patientLog;
    [SerializeField] private Button outsideExclamationButton;
    [SerializeField] private Button treatButton;
    [SerializeField] private GameObject treatExclamation;
    [SerializeField] private GameObject blueprintSelectionPanel;

    [Header("Inventory & Items")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private ItemSO plantItemSO;
    [SerializeField] private ItemSO pillsBlueprint;
    [SerializeField] private UI_Crafting uiCrafting;

    private InMemoryVariableStorage yarnVariables;
    private DialogueStep currentStep;
    private List<DialogueStep> dialogueSteps;
    
    
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

    private void Awake()
    {
        yarnVariables = dialogueRunner.VariableStorage as InMemoryVariableStorage;
        dialogueSteps = new List<DialogueStep>
        {
            new("ManagerDialogue", "$finishedManagerDialogue", () =>
            {
                outsideExclamationButton.interactable = true;
            }),
            new("ManagerDialogueForPills", "$finishedPillDialogue", () =>
            {
                inventory.AddItem(new Item { itemData = pillsBlueprint, amount = 1 });
                plantPanel.SetActive(true);
                inventoryManager.AddItem(new Item { itemData = plantItemSO, amount = 3 });
            }),
            new("CraftingPills", "$finishedCraftingDialogue", () =>
            {
                UICraftingWindow.SetActive(true);
                treatButton.interactable = true;
                treatExclamation.SetActive(true);
                arrow1.SetActive(true);
                arrow2.SetActive(true);
            }),
            new("AdministratingPills", "$readyToAdministerPills", () =>
            {
                treatExclamation.SetActive(false);
            }),
            new("PatientLog", "$finishedPatientLogDialogue", () =>
            {
                patientLog.SetActive(true);
                foreach (var word in unlockableWords)
                    word.SetActive(true);
            }),
            new ("ChoosingBlueprint", "$readyToChooseBlueprint", () =>
            {
                //open the blueprint selection UI
                blueprintSelectionPanel.SetActive(true);
                //test
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
        outsideExclamationButton.interactable = false;
        treatButton.interactable = false;
        treatExclamation.SetActive(false);
        blueprintSelectionPanel.SetActive(false);
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
        StartDialogueByNodeName("CraftingPills");
    }
    public void StartDialogueAfterBlueprintSelection()
    {
        dialogueRunner.StartDialogue("BlueprintChosen");
    }

    private void OnDialogueComplete()
    {
        if (dialogueRunner.CurrentNodeName == "AnyaIntro" &&
            yarnVariables != null &&
            yarnVariables.TryGetValue("$popUpScreenTut", out bool hasFinished) && hasFinished)
        {
            panelUI.SetActive(true);
            hospitalButton.SetActive(true);
        }

        currentStep?.PostDialogueAction?.Invoke();
        currentStep = null;
    }

    public void HideButtons() => plantPanel.SetActive(false);

    public void GoToEntrance() =>
        playerAgent.SetDestination(hospitalEntrance.transform.position);

    public void DisableButton()
    {
        hospitalButton.SetActive(false);
        panelUI.SetActive(false);
    }
    

    private void OnEnable() =>
        UI_Crafting.OnItemCrafted += OnPlantBlueprintCrafted;

    private void OnDisable() =>
        UI_Crafting.OnItemCrafted -= OnPlantBlueprintCrafted;
    
    
    // This method is called when the player crafts the plant blueprint (change it)
    private void OnPlantBlueprintCrafted(ItemSO obj)
    {
        if (exclamationMark != null)
            exclamationMark.SetActive(true);

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
