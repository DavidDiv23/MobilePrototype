using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;

    public void Setup(Sprite iconSprite, string text, bool isValid)
    {
        if (icon != null) icon.sprite = iconSprite;
        if (amountText != null)
        {
            amountText.text = text;
            amountText.color = isValid ? validColor : invalidColor;
        }
    }
}
