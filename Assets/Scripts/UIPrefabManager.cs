using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

// make a prefab and Find fuction btn connect
public class UIPrefabManager : MonoBehaviour // Cloth Product PrefabManager
{
    public GameObject uiPrefab; //UI prefab
    public Transform contentTransform;  // Parent Ojbect current => Content
    Image leftImage;
    Image rightImage;
    //public Cloth cloth;
    public GameObject clothesManagerObject;
    public ClothesManager clothesManager;
    void Start()
    {
        clothesManagerObject = GameObject.Find("ClothesManager");
        clothesManager = clothesManagerObject.GetComponent<ClothesManager>();
    }

    public void PrefabInstantiate()
    {
        for (int i = 0; i < 10; i++)
        {
            APIDownload(i);
        }
    }
    IEnumerator APIDownload(int idx)
    {
        GameObject uiObject = Instantiate(uiPrefab, contentTransform);

        Transform leftImageTransform = uiObject.transform.Find("LeftImage");
        Transform rightImageTransform = uiObject.transform.Find("RightImage");
        
        if (leftImageTransform != null)
        {
            // Accessing the Image component of the LeftImage object
            leftImage = leftImageTransform.GetComponent<Image>();
            rightImage = rightImageTransform.GetComponent<Image>();
            if (leftImage != null)
            {
                for (int i = 0; i < 2; i++)
                {
                     //naming
                    uiObject.name = "ClothesPreSet" + ((idx-1)/2).ToString(); 
                    leftImage.name = "LeftImage" + ((idx-1)/2).ToString();
                    rightImage.name = "RightImage" + ((idx-1)/2).ToString();
                    
                    string clothImageURL = "http://220.149.231.136:8051/" + clothesManager.clothes[idx].imgUrl;
                    

                    Debug.Log("clothes[idx]+" + idx);
                    UnityWebRequest www = UnityWebRequestTexture.GetTexture(clothImageURL);
                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.Success)
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(www);

                        // Create a sprite from the downloaded texture
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                        
                            leftImageTransform.GetComponentInChildren<ImageURL>().imgUrl = "http://220.149.231.136:8051/" + clothesManager.clothes[idx].imgUrl;
                            leftImage.sprite = sprite;

                            Text[] textComponents = leftImageTransform.GetComponentsInChildren<Text>();

                            Text leftNameText = null;
                            Text leftPriceText = null;

                            foreach (Text textComponent in textComponents)
                            {
                                if (textComponent.name == "NameText")
                                {
                                    leftNameText = textComponent;
                                }
                                else if (textComponent.name == "PriceText")
                                {
                                    leftPriceText = textComponent;
                                }
                            }

                            if (leftNameText != null && leftPriceText != null)
                            {
                                // Access and modify the properties of NameText and PriceText
                                leftNameText.text = clothesManager.clothes[idx].name;
                                leftPriceText.text = clothesManager.clothes[idx].price.ToString();
                            }
                        
                        
                    }
                    else
                    {
                        Debug.Log("Image download failed: " + www.error);
                    }
                    idx++;
                }
            }
        }

    }


}
