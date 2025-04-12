using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Drop_Manager : MonoBehaviour
{
    public int correctDrops = 0;
    public bool hasGivenBlueprint = false;

    public void RegisterCorrectDrop()
    {
        correctDrops++;
        if (correctDrops >= 3 && !hasGivenBlueprint)
        {
            hasGivenBlueprint = true;
            GiveBluePrint();
        }
    }

    public void GiveBluePrint()
    {
        Debug.Log("Blueprint given!");
    }
    public void ResetDrops()
    {
        correctDrops = 0;
        hasGivenBlueprint = false;
    }
}
