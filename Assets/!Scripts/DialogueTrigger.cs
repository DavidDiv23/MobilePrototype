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
            variableStorage.TryGetValue("$hasFinishedIntro", out bool hasFinishedIntro);
            
            if (hasFinishedIntro)
            {
                dialogueRunner.StartDialogue("AnyaDialogueManager");
            }
            else
            {
                dialogueRunner.StartDialogue("AnyaIntro");
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
        if (dialogueRunner.CurrentNodeName == "AnyaIntro")
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
