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

    public bool isNPCClicked;
    public Camera mainCamera;
    public bool hasStartedDialogue;
    private UI_Handler uiHandler;
    
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
        dialogueRunner.StartDialogue("LionDialogue");
        hasStartedDialogue = true;
    }
    public void OnDialogueComplete()
    {
        hasStartedDialogue = true;
        isNPCClicked = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit) && hit.transform == transform) 
            {
                isNPCClicked = true;
            }
        }
    }
}
