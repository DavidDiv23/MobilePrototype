using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BloodPressure : MonoBehaviour
{
    [Header("UI References")]
    public Slider pressureBar;
    public Button controlButton;
    public Image fillImage; // Reference to the slider's fill image

    [Header("Game Settings")]
    public float decayRate = 0.1f;
    public float pressGain = 0.15f;
    public float minTarget = 0.3f; // Start of ideal zone
    public float maxTarget = 0.7f; // End of ideal zone
    public float activationThreshold = 0.05f; // Value where color system activates

    [Header("Color Settings")]
    public Color initialColor = Color.yellow;  // Starting color (below activation)
    public Color warningColor = Color.yellow;  // Below ideal zone
    public Color normalColor = Color.green;    // Ideal range
    public Color criticalColor = Color.red;    // Too high or too low

    private bool isSystemActive = false;

    void Start()
    {
        controlButton.onClick.AddListener(OnButtonPressed);

        // Get the fill image if not assigned
        if (fillImage == null && pressureBar.fillRect != null)
        {
            fillImage = pressureBar.fillRect.GetComponent<Image>();
        }

        // Start completely empty
        pressureBar.value = 0f;
        fillImage.color = initialColor;
    }

    void Update()
    {
        // Decrease bar value over time
        pressureBar.value -= decayRate * Time.deltaTime;
        pressureBar.value = Mathf.Clamp01(pressureBar.value);

        // Check if we've passed the activation threshold
        if (!isSystemActive && pressureBar.value > activationThreshold)
        {
            isSystemActive = true;
        }

        UpdateBarColor();
    }

    void OnButtonPressed()
    {
        pressureBar.value += pressGain;

        // Activate system if we pass threshold with button press
        if (!isSystemActive && pressureBar.value > activationThreshold)
        {
            isSystemActive = true;
        }
    }

    void UpdateBarColor()
    {
        if (fillImage == null) return;

        float currentValue = pressureBar.value;

        if (!isSystemActive)
        {
            // Initial state - below activation threshold
            fillImage.color = initialColor;
        }
        else
        {
            // Active state - use 3-color system
            if (currentValue < minTarget)
            {
                // Below ideal zone
                fillImage.color = warningColor;
            }
            else if (currentValue > maxTarget)
            {
                // Above ideal zone
                // Considered critical if very high
                fillImage.color = (currentValue > 0.9f) ? criticalColor : warningColor;
            }
            else
            {
                // Within ideal range
                fillImage.color = normalColor;
            }

            // Critical state - completely empty (after activation)
            if (currentValue <= 0.01f)
            {
                fillImage.color = criticalColor;
            }
        }
    }
}
