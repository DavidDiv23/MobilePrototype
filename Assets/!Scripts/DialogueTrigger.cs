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
    public GameObject[] unlockableWords;

    public Camera mainCamera;
    public bool hasStartedDialogue;
    private UI_Handler uiHandler;
    public bool isInteracting;
    public GameObject exclamationMark;
    
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
        exclamationMark.SetActive(true);

        foreach (var word in unlockableWords)
        {
            word.SetActive(true);
        }
    }
}
