using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            dialogueRunner.StartDialogue("LionDialogue");
        }
    }
}
