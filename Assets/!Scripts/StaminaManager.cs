using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StaminaManager : MonoBehaviour
{
    [Header("Stamina Settings")]
    public int maxStamina = 5;
    public int currentStamina = 5;
    public float rechargeTime = 15f;

    [Header("UI References")]
    public TextMeshProUGUI staminaText;
    public Image emotionIcon;
    public Button useStaminaButton;
    public Image staminaFillBar;

    [Header("Emotion Sprites")]
    public Sprite happySprite;
    public Sprite somewhatHappySprite;
    public Sprite neutralSprite;
    public Sprite overwhelmedSprite;
    public Sprite burntOutSprite;

    private Queue<IEnumerator> rechargeQueue = new Queue<IEnumerator>();
    private bool isRecharging = false;

    void Start()
    {
        UpdateStaminaUI();
        useStaminaButton.onClick.AddListener(UseStamina);
    }

    public void UseStamina()
    {
        if (currentStamina > 0)
        {
            currentStamina--;
            UpdateStaminaUI();

            rechargeQueue.Enqueue(RechargeOnePoint());
            if (!isRecharging)
                StartCoroutine(ProcessRechargeQueue());
        }
    }

    IEnumerator ProcessRechargeQueue()
    {
        isRecharging = true;
        while (rechargeQueue.Count > 0)
        {
            yield return StartCoroutine(rechargeQueue.Dequeue());
        }
        isRecharging = false;
    }

    IEnumerator RechargeOnePoint()
    {
        yield return new WaitForSeconds(rechargeTime);

        currentStamina = Mathf.Min(currentStamina + 1, maxStamina);
        UpdateStaminaUI();
    }

    void UpdateStaminaUI()
    {
        // Update stamina text
        if (staminaText != null)
            staminaText.text = $"{currentStamina}/{maxStamina}";

        // Update emotion icon
        if (emotionIcon != null)
        {
            Sprite newSprite = burntOutSprite;
            switch (currentStamina)
            {
                case 5:
                    newSprite = happySprite;
                    break;
                case 4:
                    newSprite = somewhatHappySprite;
                    break;
                case 3:
                    newSprite = neutralSprite;
                    break;
                case 2:
                    newSprite = overwhelmedSprite;
                    break;
                case 1:
                case 0:
                    newSprite = burntOutSprite;
                    break;
            }
            emotionIcon.sprite = newSprite;
            
            staminaFillBar.fillAmount = (float)currentStamina / (float)maxStamina;
        }
        // Disable the button if stamina is empty
        if (useStaminaButton != null)
            useStaminaButton.interactable = currentStamina > 0;
    }
}
