using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPC_Drop_Manager : MonoBehaviour
{
    public int correctDrops = 0;
    public bool hasCompleted = false;
    public int dragAndDropValue = 0;
    public bool hasCompletedDragAndDrop = false;
    
    public DialogueRunner dialogueRunner;

    public void RegisterCorrectDrop()
    {
        correctDrops++;
        Debug.Log(correctDrops);
        if (correctDrops == 3)
        {
            hasCompleted = true;
            dialogueRunner.StartDialogue("");
        }
    }
    
    public void RegisterDragAndDrop()
    {
        dragAndDropValue++;
        Debug.Log(dragAndDropValue);
        if (dragAndDropValue == 4)
        {
            hasCompletedDragAndDrop = true;
        }
    }
    

    public void GiveBluePrint()
    {
        Debug.Log("Blueprint given!");
    }
    public void ResetDrops()
    {
        correctDrops = 0;
        hasCompleted = false;
    }
}
