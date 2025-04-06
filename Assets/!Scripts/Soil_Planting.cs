using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soil_Planting : MonoBehaviour
{
    public Transform plantingPoint;
    private GameObject plantedCrop;
    public Button plantButton;
    
    public void PlantCrop(GameObject cropPrefab)
    {
        if (plantedCrop == null)
        {
            plantedCrop = Instantiate(cropPrefab, plantingPoint.position, Quaternion.identity);
            plantButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("A crop is already planted here.");
        }
    }
}
