using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour, IPointerClickHandler
{
    public DialogueRunner dialogueRunner;
    public void OnPointerClick(PointerEventData eventData)
    {
        dialogueRunner.StartDialogue("LionDialogue");
    }
}
