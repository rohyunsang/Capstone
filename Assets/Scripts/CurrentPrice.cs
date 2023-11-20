using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPrice : MonoBehaviour
{
    public Text priceTextMain;
    public Text priceTextBasket;
    public int currentPrice = 0;

    
    public void CurrnetPriceUpdate(int price){
        currentPrice += price;
        priceTextMain.text = currentPrice.ToString("#,0");
        priceTextBasket.text = currentPrice.ToString("#,0");
    }
}

// 코루틴으로 최신화 할까 생각중. 
// Object.Price 가져오기. 