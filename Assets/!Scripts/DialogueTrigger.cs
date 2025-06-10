using System;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    public string dialogueNodeName;


    public GameObject exclamationMark;
    public Camera mainCamera;
    public CameraZoomScript cameraZoom;

    private UI_Handler uiHandler;
    public bool hasStartedDialogue;

    private void Start()
    {
        uiHandler = FindObjectOfType<UI_Handler>();
        dialogueRunner.onDialogueStart.AddListener(OnDialogueStart);
        dialogueRunner.onDialogueComplete.AddListener(CheckIfMyDialogueFinished);
    }

    private void OnDialogueStart()
    {
        uiHandler.HideCanvas();
    }

    public void StartingDialogue()
    {
        var variableStorage = dialogueRunner.VariableStorage;

        if (dialogueNodeName == "AnyaIntro" || dialogueNodeName == "AnyaDialogueManager")
        {
            variableStorage.TryGetValue("$hasGivenBlueprint", out bool hasGivenBlueprint);
            variableStorage.TryGetValue("$completedPillsTask", out bool hasCompletedPills);
            variableStorage.TryGetValue("$hasFinishedIntro", out bool hasFinishedIntro);

            if (!hasFinishedIntro)
            {
                dialogueRunner.StartDialogue("AnyaIntro");
            }
            else if (hasCompletedPills && !hasGivenBlueprint)
            {
                dialogueRunner.StartDialogue("GiftingBlueprint");
            }
            else
            {
                dialogueRunner.StartDialogue("AnyaDialogueManager");
            }
        }
        else
        {
            dialogueRunner.StartDialogue(dialogueNodeName);
        }

        hasStartedDialogue = true;
    }

    private void CheckIfMyDialogueFinished()
    {
        if (dialogueRunner.CurrentNodeName == "AnyaIntro" 
            || dialogueRunner.CurrentNodeName == "AnyaDialogueManager"
            || dialogueRunner.CurrentNodeName == "AnyaDialoguePriorToManager"
            || dialogueRunner.CurrentNodeName == "PlayerReturnsToAnya"
            || dialogueRunner.CurrentNodeName == "GiftingBlueprint"
            || dialogueRunner.CurrentNodeName == "CompletingPillsTask")
        {
            OnMyDialogueComplete();
        }
    }

    private void OnMyDialogueComplete()
    {
        hasStartedDialogue = false;
  
        if (cameraZoom != null) cameraZoom.ResetCamera();
        
    }
}
