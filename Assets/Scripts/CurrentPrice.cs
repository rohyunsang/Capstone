using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPrice : MonoBehaviour  // + Basket Count
{
    public Text currentPriceText;
    public int currentPrice = 0;
    public int basketCount = 0;
    public TextMeshProUGUI basketCountText;

    public void UpdateCurrentPrice()
    {
        currentPriceText.text = currentPrice.ToString("N0");
        basketCountText.text = basketCount.ToString();
    }
    
    public void InitCurrentPrice()
    {
        basketCount = 0;
        currentPrice = 0;
        UpdateCurrentPrice();
    }
}
