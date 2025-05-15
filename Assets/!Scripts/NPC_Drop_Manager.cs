using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Drop_Manager : MonoBehaviour
{
    public int correctDrops = 0;
    public bool hasCompleted = false;

    public void RegisterCorrectDrop()
    {
        correctDrops++;
        Debug.Log(correctDrops);
        if (correctDrops == 3)
        {
            hasCompleted = true;
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
