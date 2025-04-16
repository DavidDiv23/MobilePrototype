using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeItemUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text amountText;

    public void Setup(Sprite itemIcon, string amount, bool hasEnough)
    {
        icon.sprite = itemIcon;
        amountText.text = amount;
        amountText.color = hasEnough ? Color.green : Color.red;
    }
}
