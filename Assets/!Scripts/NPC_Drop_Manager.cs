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
    public GameObject dragAndDropObject;
    
    public DialogueRunner dialogueRunner;

    public void RegisterWords()
    {
        correctDrops++;
        Debug.Log(correctDrops);
        if (correctDrops == 3)
        {
            hasCompleted = true;
            dialogueRunner.StartDialogue("FinishingPatientLog");
        }
    }
    
    public void RegisterPills()
    {
        dragAndDropValue++;
        Debug.Log(dragAndDropValue);
        if (dragAndDropValue == 4)
        {
            GiveBluePrint();
            dialogueRunner.StartDialogue("CompletingPillsTask");
            hasCompletedDragAndDrop = true;
            dragAndDropObject.SetActive(false);
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
