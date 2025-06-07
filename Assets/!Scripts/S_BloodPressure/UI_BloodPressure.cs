using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BloodPressure : MonoBehaviour
{
    [Header("UI References")]
    public Slider pressureBar;
    public Button controlButton;
    public Image fillImage;
    public TMP_Text scoreText;

    [Header("Zone Settings")]
    [Range(0, 1)] public float redZoneMin = 0f;       // Start of lower red zone
    [Range(0, 1)] public float yellowZoneMin = 0.05f; // Start of lower yellow zone
    [Range(0, 1)] public float greenZoneMin = 0.3f;   // Start of green zone
    [Range(0, 1)] public float greenZoneMax = 0.7f;   // End of green zone
    [Range(0, 1)] public float yellowZoneMax = 0.9f;  // End of upper yellow zone
    [Range(0, 1)] public float redZoneMax = 1f;       // End of upper red zone

    [Header("Game Settings")]
    public float decayRate = 0.1f;
    public float pressGain = 0.15f;

    [Header("Scoring System")]
    public float currentScore = 0f;
    public float maxScore = 100f;
    public float greenScoreRate = 15f;  // Fastest rate in green zone
    public float yellowScoreRate = 5f;  // Slower rate in yellow zone

    [Header("Color Settings")]
    public Color redColor = Color.red;
    public Color yellowColor = Color.yellow;
    public Color greenColor = Color.green;

    void Start()
    {
        controlButton.onClick.AddListener(OnButtonPressed);

        if (fillImage == null && pressureBar.fillRect != null)
        {
            fillImage = pressureBar.fillRect.GetComponent<Image>();
        }

        pressureBar.value = 0f;
        UpdateBarColor();
        UpdateScoreDisplay();
    }

    void Update()
    {
        pressureBar.value -= decayRate * Time.deltaTime;
        pressureBar.value = Mathf.Clamp01(pressureBar.value);

        UpdateBarColor();
        UpdateScore();
    }

    void OnButtonPressed()
    {
        pressureBar.value += pressGain;
    }

    void UpdateBarColor()
    {
        if (fillImage == null) return;

        float currentValue = pressureBar.value;

        // Determine color based on zone boundaries
        if (currentValue < redZoneMin || currentValue > redZoneMax)
        {
            // Critical red zones (extremes)
            fillImage.color = redColor;
        }
        else if (currentValue < yellowZoneMin || currentValue > yellowZoneMax)
        {
            // Red zones (but not extremes)
            fillImage.color = redColor;
        }
        else if (currentValue < greenZoneMin || currentValue > greenZoneMax)
        {
            // Yellow warning zones
            fillImage.color = yellowColor;
        }
        else
        {
            // Green ideal zone
            fillImage.color = greenColor;
        }
    }

    void UpdateScore()
    {
        float scoringRate = 0f;
        float currentValue = pressureBar.value;

        // Determine scoring rate based on current zone
        if (currentValue >= greenZoneMin && currentValue <= greenZoneMax)
        {
            // Green zone - fastest rate
            scoringRate = greenScoreRate;
        }
        else if ((currentValue >= yellowZoneMin && currentValue < greenZoneMin) ||
                 (currentValue > greenZoneMax && currentValue <= yellowZoneMax))
        {
            // Yellow zones - slower rate
            scoringRate = yellowScoreRate;
        }
        // Red zones get 0 scoring rate

        // Apply scoring
        if (scoringRate > 0)
        {
            currentScore += scoringRate * Time.deltaTime;
            currentScore = Mathf.Clamp(currentScore, 0, maxScore);
        }

        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            float percentage = (currentScore / maxScore) * 100f;
            scoreText.text = $"{percentage:F0}%";
        }
    }

    // Visualize zones in the editor for easier configuration
    void OnValidate()
    {
        // Ensure logical zone ordering
        redZoneMin = Mathf.Clamp(redZoneMin, 0, yellowZoneMin);
        yellowZoneMin = Mathf.Clamp(yellowZoneMin, redZoneMin, greenZoneMin);
        greenZoneMin = Mathf.Clamp(greenZoneMin, yellowZoneMin, greenZoneMax);
        greenZoneMax = Mathf.Clamp(greenZoneMax, greenZoneMin, yellowZoneMax);
        yellowZoneMax = Mathf.Clamp(yellowZoneMax, greenZoneMax, redZoneMax);
        redZoneMax = Mathf.Clamp(redZoneMax, yellowZoneMax, 1);
    }
}