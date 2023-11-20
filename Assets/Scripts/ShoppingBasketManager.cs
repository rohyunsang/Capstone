using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingBasketManager : MonoBehaviour  // use '담기' button
{
    public GameObject clothCopy; //UI prefab
    public Transform contentTransform;  // Parent Ojbect current => Content
    public Image clothImage;

    //PriceText Variable
    public GameObject currentPriceManager;
    GameObject shoppingBasketPanel;
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
        string objectName = clothImage.name;
        InstantiateToShoppingBasket(objectName);

        // 상품이 장바구니에 담겼습니다. 텍스트 2초간 출력
        StartCoroutine(ShowMessageCoroutine("상품이 장바구니에 담겼습니다.", 2f, 44));
    }
    void InstantiateToShoppingBasket(string objectName)
    {
        clothCopy = GameObject.Find(objectName); // image 이름으로 gameObject 찾아서 복사.
        GameObject uiObject = Instantiate(clothCopy, contentTransform);

        //price text
        Text[] childTexts = clothImage.GetComponentsInChildren<Text>(true);
        foreach (Text childText in childTexts)
        {
            if (childText.name == "PriceText")
            {
                //int clothCopyPrice = int.Parse(childText.text);
                //currentPriceManager.GetComponent<CurrentPrice>().CurrnetPriceUpdate(clothCopyPrice);
                break;
            }
        }


        // remove btn
        uiObject.transform.Find("TryOnBtn").gameObject.SetActive(false);  // Try On btn, 담기 btn 비활성화
        uiObject.transform.Find("AddCartBtn").gameObject.SetActive(false);

        // 새로운 닫기 버튼 생성

        GameObject closeButton = new GameObject("CloseButton");
        closeButton.transform.SetParent(uiObject.transform); // 새 버튼의 부모를 contentTransform으로 설정
        RectTransform closeButtonRectTransform = closeButton.AddComponent<RectTransform>();
        closeButtonRectTransform.anchorMin = new Vector2(1f, 0f); // 우측 상단에 위치
        closeButtonRectTransform.anchorMax = new Vector2(1f, 0f);
        closeButtonRectTransform.pivot = new Vector2(1f, 0f);
        closeButtonRectTransform.anchoredPosition = Vector2.zero; // 이미지 우측 상단에 위치하도록 설정
        closeButtonRectTransform.sizeDelta = new Vector2(70f, 70f); // 적절한 크기로 설정

        // 새로운 닫기 버튼에 이미지 컴포넌트 추가
        Image closeButtonImage = closeButton.AddComponent<Image>();
        closeButtonImage.color = new Color(0.498f, 0.545f, 1f);

        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(closeButton.transform); // 텍스트의 부모를 closeButton으로 설정
        RectTransform textRectTransform = textObject.AddComponent<RectTransform>();
        textRectTransform.anchorMin = new Vector2(0.5f, 0.5f); // 중앙에 위치
        textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);
        textRectTransform.anchoredPosition = Vector2.zero; // 중앙에 위치하도록 설정

        // 텍스트 컴포넌트 추가 및 설정
        Text textComponent = textObject.AddComponent<Text>();
        textComponent.text = "X"; // 텍스트 내용 설정
        textComponent.font = Font.CreateDynamicFontFromOSFont("Arial", 14); // 원하는 폰트 및 크기 설정
        textComponent.alignment = TextAnchor.MiddleCenter; // 중앙 정렬
        textComponent.color = Color.black; // 텍스트 컬러 설정
        textComponent.fontSize = 30; 
        // 닫기 버튼을 클릭했을 때 부모 오브젝트를 삭제하는 기능 추가
        Button closeButtonButton = closeButton.AddComponent<Button>();
        closeButtonButton.onClick.AddListener(() =>
        {
            //price text
            Text[] childTexts = clothImage.GetComponentsInChildren<Text>(true);
            foreach (Text childText in childTexts)
            {
                if (childText.name == "PriceText")
                {
                    int clothCopyPrice = (-1) * int.Parse(childText.text);
                    currentPriceManager.GetComponent<CurrentPrice>().CurrnetPriceUpdate(clothCopyPrice);
                    break;
                }
            }

            Destroy(closeButton.transform.parent.gameObject);
            
        });

        uiObject.transform.SetAsFirstSibling(); // 이미지 위로 닫기 버튼이 표시되도록 설정
        uiObject.transform.localPosition = Vector3.zero;
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
