using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingBasketManager : MonoBehaviour  // use '담기' button
{
    public GameObject product; //UI prefab
    public Transform contentTransform;  // Parent Ojbect current => Content
    public Image clothImage;

    //PriceText Variable
    public GameObject currentPriceManager;
    public GameObject mainPanelObject;

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
        Debug.Log(" cloth name " + clothImage.name);
        string objectName = clothImage.name;
        InstantiateToShoppingBasket(objectName);

        // 상품이 장바구니에 담겼습니다. 텍스트 2초간 출력
        StartCoroutine(ShowMessageCoroutine("상품이 장바구니에 담겼습니다.", 2f, 44));
    }
    void InstantiateToShoppingBasket(string objectName)
    {
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
                break;
            }
        }


        // remove btn
        uiObject.transform.Find("AddCartBtn").gameObject.SetActive(false);
    }


    // 메시지를 일정 시간 동안 출력하는 코루틴 함수
    IEnumerator ShowMessageCoroutine(string message, float duration, int fontSize)
    {
        GameObject messageObject = new GameObject("MessageText");
        messageObject.transform.SetParent(mainPanelObject.transform); // 부모 오브젝트를 패널의 contentTransform으로 설정합니다.
        RectTransform messageRectTransform = messageObject.AddComponent<RectTransform>();
        messageRectTransform.anchorMin = new Vector2(0.5f, 0.5f); // 앵커를 중앙으로 설정합니다.
        messageRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        messageRectTransform.pivot = new Vector2(0.5f, 0.5f);
        messageRectTransform.anchoredPosition = Vector2.zero; // 메시지 오브젝트를 가운데로 정렬합니다.
        messageRectTransform.sizeDelta = new Vector2(300f, 300f);

        Image backgroundImage = messageObject.AddComponent<Image>();
        backgroundImage.color = new Color(0.8f, 0.8f, 0.8f, 0.8f); // 배경 색상과 투명도를 설정합니다.
        backgroundImage.rectTransform.sizeDelta = new Vector2(300f, 300f); // 이미지 크기를 조정합니다.

        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(messageObject.transform); // 부모를 메시지 오브젝트로 설정합니다.
        RectTransform textRectTransform = textObject.AddComponent<RectTransform>();
        textRectTransform.anchorMin = new Vector2(0.5f, 0.5f); // 앵커를 중앙으로 설정합니다.
        textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);
        textRectTransform.anchoredPosition = Vector2.zero; // 텍스트 오브젝트를 가운데로 정렬합니다.
        textRectTransform.sizeDelta = new Vector2(300f,300f);

        Text messageText = textObject.AddComponent<Text>();
        messageText.text = message;
        messageText.font = Font.CreateDynamicFontFromOSFont("Arial", fontSize);
        messageText.alignment = TextAnchor.MiddleCenter;
        messageText.color = Color.black;
        messageText.fontSize = fontSize; // 글꼴 크기를 설정합니다.

        yield return new WaitForSeconds(duration);

        Destroy(messageObject);
    }
}
