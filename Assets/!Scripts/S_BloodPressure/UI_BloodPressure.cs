using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UI_BloodPressure : MonoBehaviour
{
    [Header("UI References")]
    public Slider pressureBar;
    public Button controlButton;
    public Image fillImage;          // Image that will display sprites
    public TMP_Text scoreText;
    public GameObject minigamePanel;
    public GameObject winPanel;

    [Header("Zone Settings")]
    [Range(0, 1)] public float redZoneMin = 0f;
    [Range(0, 1)] public float yellowZoneMin = 0.05f;
    [Range(0, 1)] public float greenZoneMin = 0.3f;
    [Range(0, 1)] public float greenZoneMax = 0.7f;
    [Range(0, 1)] public float yellowZoneMax = 0.9f;
    [Range(0, 1)] public float redZoneMax = 1f;

    [Header("Sprite Settings")] // Replaces Color Settings
    public Sprite redSprite;
    public Sprite yellowSprite;
    public Sprite greenSprite;

    [Header("Game Settings")]
    public float decayRate = 0.1f;
    public float pressGain = 0.15f;
    public float winDelay = 2f;

    [Header("Scoring System")]
    public float currentScore = 0f;
    public float maxScore = 100f;
    public float greenScoreRate = 15f;
    public float yellowScoreRate = 5f;

    private bool gameActive = true;

    void Start()
    {
        controlButton.onClick.AddListener(OnButtonPressed);
        if (fillImage == null && pressureBar.fillRect != null)
        {
            fillImage = pressureBar.fillRect.GetComponent<Image>();
        }
        pressureBar.value = 0f;
        UpdateBarSprite(); // Initialize sprite
        UpdateScoreDisplay();
        if (winPanel != null) winPanel.SetActive(false);
    }

    void Update()
    {
        if (!gameActive) return;

        pressureBar.value -= decayRate * Time.deltaTime;
        pressureBar.value = Mathf.Clamp01(pressureBar.value);

        UpdateBarSprite(); // Changed from UpdateBarColor
        UpdateScore();
    }

    void OnButtonPressed()
    {
        if (!gameActive) return;
        pressureBar.value += pressGain;
    }

    void UpdateBarSprite() // Replaces UpdateBarColor
    {
        if (fillImage == null) return;

        float currentValue = pressureBar.value;

        if (currentValue < redZoneMin || currentValue > redZoneMax)
        {
            fillImage.sprite = redSprite;
        }
        else if (currentValue < yellowZoneMin || currentValue > yellowZoneMax)
        {
            fillImage.sprite = redSprite;
        }
        else if (currentValue < greenZoneMin || currentValue > greenZoneMax)
        {
            fillImage.sprite = yellowSprite;
        }
        else
        {
            fillImage.sprite = greenSprite;
        }
    }

    void UpdateScore()
    {
        if (!gameActive) return;

        float scoringRate = 0f;
        float currentValue = pressureBar.value;

        // Determine scoring rate based on current zone
        if (currentValue >= greenZoneMin && currentValue <= greenZoneMax)
        {
            scoringRate = greenScoreRate;
        }
        else if ((currentValue >= yellowZoneMin && currentValue < greenZoneMin) ||
                 (currentValue > greenZoneMax && currentValue <= yellowZoneMax))
        {
            scoringRate = yellowScoreRate;
        }

        // Apply scoring
        if (scoringRate > 0)
        {
            currentScore += scoringRate * Time.deltaTime;
            currentScore = Mathf.Clamp(currentScore, 0, maxScore);

            // Check for win condition
            if (currentScore >= maxScore)
            {
                StartCoroutine(WinGame());
            }
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

    IEnumerator WinGame()
    {
        gameActive = false;

        // Show win panel
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        // Wait for the specified delay
        yield return new WaitForSeconds(winDelay);

        // Reset the game
        ResetGame();

        // Close panels
        if (winPanel != null) winPanel.SetActive(false);
        if (minigamePanel != null) minigamePanel.SetActive(false);
    }

    void ResetGame()
    {
        // Reset game state
        gameActive = true;
        currentScore = 0f;
        pressureBar.value = 0f;
        UpdateScoreDisplay();
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