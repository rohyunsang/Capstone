using com.example;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShoppingBasketManager : MonoBehaviour  // use '담기' button
{
    public GameObject product; //UI prefab
    public Transform contentTransform;  
    public Image clothImage;

    //PriceText Variable
    public GameObject currentPriceManager;
    public GameObject mainPanelObject;

    public Sprite redHeart;
    public Sprite greyHeart;
    public GameObject basketInfo;
    public GameObject closeButton;


    // Start is called before the first frame update
    void Start()
    {
        mainPanelObject = GameObject.Find("MainPanel"); 
        // Hierachy에서 Content 오브젝트를 찾고 해당 Transform을 contentTransform에 할당
        Transform content = GameObject.Find("BasketContent").GetComponent<Transform>();
        if (content != null)
        {
            contentTransform = content.transform;
        }
        //PriceText
        currentPriceManager = GameObject.Find("CurrentPriceManager");

    }
    public void AddToShoppingBasketButton()  //using product sample prefab button
    {
        if(transform.GetComponent<Image>().sprite == redHeart)
        {
            for (int i = 0; i < contentTransform.childCount; i++)
            {
                GameObject child = contentTransform.GetChild(i).gameObject;

                if (child.name.Contains(transform.parent.gameObject.name))
                {
                    child.transform.Find("CloseImage").GetComponent<DestroyBasketImage>().DestroyCloth();
                    Destroy(child);
                }
            }
            transform.GetComponent<Image>().sprite = greyHeart;
        }
        else
        {
            transform.GetComponent<Image>().sprite = redHeart;
            InstantiateToShoppingBasket(clothImage.name);
        }
        
    }
    void InstantiateToShoppingBasket(string objectName)
    {
        closeButton.SetActive(true);
        product = GameObject.Find(objectName); // image 이름으로 gameObject 찾아서 복사.
        GameObject uiObject = Instantiate(product, contentTransform);

        //price text
        Text[] childTexts = clothImage.GetComponentsInChildren<Text>(true);
        foreach (Text childText in childTexts)
        {
            if (childText.name == "ClothPrice")
            {
                string priceText = childText.text.Replace(",", "");
                int clothCopyPrice = int.Parse(priceText);
                currentPriceManager.GetComponent<CurrentPrice>().currentPrice += clothCopyPrice;
                currentPriceManager.GetComponent<CurrentPrice>().basketCount++;
                currentPriceManager.GetComponent<CurrentPrice>().UpdateCurrentPrice();
                break;
            }
        }
        #region Info
        // Instantiate basketInfo
        GameObject basketInfoInstance = Instantiate(basketInfo, mainPanelObject.transform);

        // Start Coroutine to automatically destroy basketInfo after 2 seconds
        StartCoroutine(DestroyAfterDelay(basketInfoInstance, 2f));

        // Add click event listener
        EventTrigger trigger = basketInfoInstance.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { Destroy(basketInfoInstance); });
        trigger.triggers.Add(entry);
        #endregion
        // remove btn
        uiObject.transform.Find("AddCartBtn").gameObject.SetActive(false);

        

        closeButton.SetActive(false); // -> origin
    }
    IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            Destroy(obj);
        }
    }
}
