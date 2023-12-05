using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyManager : MonoBehaviour
{
    // 옷
    public Transform basketContent;
    public Transform mainContent;

    // 텍스트
    public Text inputHeightText;
    public GameObject inputHeightPlaceHolder;

    public Text widthText;
    public Text s_sleeveText;
    public Text l_sleeveText;
    public Text recommendText;

    public GameObject widthPraceHolder;
    public GameObject sSleevePraceHolder;
    public GameObject lSleevePraceHolder;
    public GameObject recommendPraceHolder;

    public GameObject webCamCaptureImage;

    public void InitAllObjects()
    {
        for (int i = basketContent.childCount - 1; i >= 0; i--)
        {
            GameObject child = basketContent.GetChild(i).gameObject;
            Destroy(child);
        }
        for (int i = mainContent.childCount - 1; i >= 0; i--)
        {
            GameObject child = mainContent.GetChild(i).gameObject;
            Destroy(child);
        }


        inputHeightText.text = "";
        inputHeightPlaceHolder.SetActive(true);

        widthText.text = "";
        s_sleeveText.text = "";
        l_sleeveText.text = "";
        recommendText.text = "";

        widthPraceHolder.SetActive(true);
        sSleevePraceHolder.SetActive(true);
        lSleevePraceHolder.SetActive(true);
        recommendPraceHolder.SetActive(true);

        webCamCaptureImage.SetActive(false);
        //CurrentPrice InitCurrentPrice(); 이것도 삭제해줘야함 


    }
}
