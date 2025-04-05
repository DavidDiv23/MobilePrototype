using System;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] private DialogueRunner dialogueRunner;
    public bool hasFinishedDialogue;
    
    private void Start()
    {
        dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        dialogueRunner.StartDialogue("LionDialogue");
        
    }
    private void OnDialogueComplete()
    {
        hasFinishedDialogue = true;
    }

    private void Update()
    {
        if (hasFinishedDialogue)
        {
            Debug.Log("Dialogue finished");
        }
    }
}
