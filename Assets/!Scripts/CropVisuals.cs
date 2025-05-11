using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropVisuals : MonoBehaviour
{
    public GameObject[] growthStages;
    private bool isReadyToHarvest = false;
    
    public InventoryManager inventoryManager;
    public ItemSO plantItemSO;
    
    public Soil_Planting assignedSoil;

    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void ShowStages(int index)
    {
        for (int i = 0; i < growthStages.Length; i++)
        {
            growthStages[i].SetActive(i == index);
            if (i == index)
            {
                if (index == growthStages.Length - 1)
                {
                    isReadyToHarvest = true;
                    Debug.Log("Crop is ready to harvest!");
                }
                else
                {
                    isReadyToHarvest = false;
                }
            }
        }
    }

    public void HarvestCrop()
    {
        if (!isReadyToHarvest)
        {
            return;
        }

        Item harvestedItem = new Item
        {
            itemData = plantItemSO,
            amount = 3
        };
        inventoryManager.AddItem(harvestedItem);
        isReadyToHarvest = false;
        
        Destroy(gameObject);
        Debug.Log("Crop harvested!");

        assignedSoil.ResetSoil();
    }
}
