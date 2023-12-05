using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PanelManager : MonoBehaviour
{
    public GameObject LoginPanel;
    public GameObject VirtualFittingPanel;
    public GameObject SettingPanel;
    public GameObject InitPanel;
    public GameObject PurchasePanel;
    public GameObject MainPanel;
    public GameObject MeasurePanel;

    public void OffMeasurePanel()
    {
        MeasurePanel.SetActive(false);
    }

    public void OnMeasurePanel()
    {
        MeasurePanel.SetActive(true);
    }

    public void OnMainPanel(){
        MainPanel.SetActive(true);
    }
    public void OffMainPanel(){
        MainPanel.SetActive(false);
    }
    public void InvokeOffPurchasePanel()
    { 
        PurchasePanel.SetActive(false);
        MainPanel.SetActive(true);
        InitPanel.SetActive(true);
    }
    public void OffPurchasePanel()
    {  //waiting 3second
        Invoke("InvokeOffPurchasePanel", 3f);
    }
    public void OnVirtualFittingPanel()
    {
        RectTransform rectTransform = VirtualFittingPanel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector3.zero; // (0, 0, 0)으로 설정 
        //VirtualFittingPanel.SetActive(true);
    }
    public void OffVIrtualFittingPanel()
    {
        RectTransform rectTransform = VirtualFittingPanel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(-700f,0f,0f); // (0, 0, 0)으로 설정
        //VirtualFittingPanel.SetActive(false);
    }
    
    public void OnSettingPanel()
    {
        SettingPanel.SetActive(true);
    }
    public void OnLoginPanel()
    {
        LoginPanel.SetActive(true);
    }
    public void OnInitPanel()
    {
        InitPanel.SetActive(true);
    }
    public void OnPurchasePanel()
    {
        PurchasePanel.SetActive(true);
        OffPurchasePanel(); // 일단 구매기능이 없으니 구매를 건너뜀.
    }
    public void OffInitPanel()
    {
        InitPanel.SetActive(false);
    }
    public void OffLoginPanel()
    {
        LoginPanel.SetActive(false);
    }
    
    public void OffSettingPanel()
    {
        SettingPanel.SetActive(false);
    }

}
