using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soil_Planting : MonoBehaviour
{
    public Transform plantingPoint;
    private GameObject plantedCrop;
    public Button plantButton;
    public Button harvestButton;
    public HarvestUIHandler harvestUIHandler;
    public GameObject cropPrefab;
    
    public void PlantCrop()
    {
        if (plantedCrop == null)
        {
            plantedCrop = Instantiate(cropPrefab, plantingPoint.position, Quaternion.identity);
            plantButton.gameObject.SetActive(false);
            CropVisuals crop = plantedCrop.GetComponent<CropVisuals>();
            harvestUIHandler.SetCurrentCrop(crop);
            crop.assignedSoil = this;
        }
        else
        {
            Debug.Log("A crop is already planted here.");
        }
    }
    public void ResetSoil()
    {
        plantedCrop = null;
        plantButton.gameObject.SetActive(true);
        harvestButton.gameObject.SetActive(false);
    }
}
