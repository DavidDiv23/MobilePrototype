using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarvestUIHandler : MonoBehaviour
{
    public Button harvestButton;
    private CropVisuals currentCrop;

    void Start()
    {
        harvestButton.onClick.AddListener(() =>
        {
            if (currentCrop != null)
                currentCrop.HarvestCrop();
        });

        harvestButton.gameObject.SetActive(false);
    }

    public void SetCurrentCrop(CropVisuals crop)
    {
        currentCrop = crop;
        harvestButton.gameObject.SetActive(true);
    }
}
