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
    public string dialogueNodeName; // Assign per character in Inspector

    public GameObject[] unlockableWords;
    public GameObject exclamationMark;
    public Camera mainCamera;
    public CameraZoomScript cameraZoom;

    private UI_Handler uiHandler;
    public bool hasStartedDialogue;

    private void Start()
    {
        uiHandler = FindObjectOfType<UI_Handler>();
        dialogueRunner.onDialogueComplete.AddListener(CheckIfMyDialogueFinished);
        dialogueRunner.onDialogueStart.AddListener(OnDialogueStart);
    }

    private void OnDialogueStart()
    {
        uiHandler.HideCanvas();
    }

    public void StartingDialogue()
    {
        dialogueRunner.StartDialogue(dialogueNodeName);
        hasStartedDialogue = true;
    }

    private void CheckIfMyDialogueFinished()
    {
        // Only respond if this specific character's dialogue just finished
        if (dialogueRunner.CurrentNodeName == dialogueNodeName)
        {
            OnMyDialogueComplete();
        }
    }

    private void OnMyDialogueComplete()
    {
        hasStartedDialogue = false;
        if (exclamationMark != null) exclamationMark.SetActive(true);
        if (cameraZoom != null) cameraZoom.ResetCamera();

        foreach (var word in unlockableWords)
        {
            word.SetActive(true);
        }
    }
}
