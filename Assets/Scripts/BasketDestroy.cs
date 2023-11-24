using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BasketDestroy : MonoBehaviour
{
    public Transform contentTransform; //use initialize shopping basket 
    public GameObject contentObject;
    public GameObject currentPriceManager;
    public GameObject basketCounterTextObject;
    // Start is called before the first frame update
    void Start()
    {
        contentTransform = contentObject.transform;
        basketCounterTextObject = GameObject.Find("BasketCounterText");
    }

    // contentObject의 모든 자식 오브젝트들을 파괴합니다.
    public void DestroyContentObjects()
    {
        foreach (Transform child in contentTransform)
        {
            if (child.gameObject.name.Equals("LineSettingEmpty"))
                continue;
            //price text
            Text[] childTexts = child.GetComponentsInChildren<Text>(true);
            foreach (Text childText in childTexts)
            {
                if (childText.name == "PriceText")
                {
                    int clothPrice = (-1) * int.Parse(childText.text);
                    break;
                }
            }
            Destroy(child.gameObject);
        }
    }
    
}
