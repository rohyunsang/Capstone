using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInstance : MonoBehaviour
{
    public Transform scrollViewMain;
    public GameObject clothProduct;
    public Texture2D[] cloths;

    public void InstantiateCloth() //using btn StartBtn in InitPanel
    {
        for (int i = 0; i < 4; i++)
        {
            //size setting is GridLay Out Group
            GameObject instance = Instantiate(clothProduct, scrollViewMain);

            // Find the 'ClothImage' child object of the instance and insert Texture2D cloths
            Transform clothImageTransform = instance.transform.Find("ClothImage");
            if (clothImageTransform != null)
            {
                Image clothImage = clothImageTransform.GetComponent<Image>();
                if (clothImage != null && i < cloths.Length)
                {
                    clothImage.sprite = Sprite.Create(cloths[i], new Rect(0, 0, cloths[i].width, cloths[i].height), new Vector2(0.5f, 0.5f));
                }
            }
        }
    }
}



