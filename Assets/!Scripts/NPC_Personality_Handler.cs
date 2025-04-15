using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC_Personality_Handler : MonoBehaviour
{
    public int happiness = 0;
    public bool hasBecomeHappy = false;
    private void Start()
    {
        
    }

    public void IncreaseHappiness(int amount)
    {
        hasBecomeHappy = true;
        happiness += amount;
        Debug.Log("Happiness increased by " + amount + ". Current happiness: " + happiness);
    }
    
    public void DecreaseHappiness(int amount)
    {
        happiness -= amount;
        Debug.Log("Happiness decreased by " + amount + ". Current happiness: " + happiness);
    }
    
    
}
