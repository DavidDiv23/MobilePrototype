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


    public Camera mainCamera;
    public bool hasStartedDialogue;
    private UI_Handler uiHandler;
    public bool isInteracting;
    
    private void Start()
    {
        dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
        dialogueRunner.onDialogueStart.AddListener(OnDialogueStart);
        uiHandler = FindObjectOfType<UI_Handler>();
    }

    private void OnDialogueStart()
    {
        uiHandler.HideCanvas();
    }


    public void StartingDialogue()
    {
        dialogueRunner.StartDialogue("AnyaIntro");
        hasStartedDialogue = true;
    }
    public void OnDialogueComplete()
    {
        hasStartedDialogue = false;
    }

    
}
