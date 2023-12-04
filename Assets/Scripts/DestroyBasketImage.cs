using com.example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyBasketImage : MonoBehaviour
{
    public GameObject CurrentPriceManager;
    public Text ClothPrice;
    public Sprite greyHeart;


    private void Awake()
    {
        CurrentPriceManager = GameObject.Find("CurrentPriceManager");
    }
    public void DestroyCloth() // using btn;
    {
        int priceTmp = int.Parse(ClothPrice.text.Replace(",", ""));
        CurrentPriceManager.GetComponent<CurrentPrice>().currentPrice -= priceTmp;
        CurrentPriceManager.GetComponent<CurrentPrice>().basketCount--;
        CurrentPriceManager.GetComponent<CurrentPrice>().UpdateCurrentPrice();
        Debug.Log(transform.parent.gameObject.name);

        
        foreach(GameObject gameObject in SupaManager.Instance.clothes)
        {
            
            if (gameObject.name.Equals(transform.parent.gameObject.name[0].ToString())){ //char to
                gameObject.transform.Find("AddCartBtn").GetComponent<Image>().sprite = greyHeart;
            }
        }

        Destroy(transform.parent.gameObject);
    }
}
