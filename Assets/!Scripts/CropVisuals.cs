using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropVisuals : MonoBehaviour
{
    public GameObject[] growthStages;

    public void ShowStages(int index)
    {
        for (int i = 0; i < growthStages.Length; i++)
        {
            growthStages[i].SetActive(i == index);
        }
    }
}
