using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PanelManager : MonoBehaviour
{
    public GameObject LoginPanel;
    public GameObject VirtualFittingPanel;
    public GameObject VirtualFittingResultPanel;
    public GameObject SettingPanel;
    public GameObject InitPanel;
    public GameObject PurchasePanel;
    public GameObject ZoomImagePanel;
    public GameObject NaviPanel;
    public GameObject MainPanel;
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
        VirtualFittingPanel.SetActive(true);
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
    public void OnZoomImagePanel()
    {
        ZoomImagePanel.SetActive(true);
    }
    public void OnNaviPanel(){
        NaviPanel.SetActive(true);
    }
    public void OffNaviPanel(){
        NaviPanel.SetActive(false);
    }
    public void OffZoomImagePanel()
    {
        ZoomImagePanel.SetActive(false);
    }
    public void OffInitPanel()
    {
        InitPanel.SetActive(false);
    }
    public void OffLoginPanel()
    {
        LoginPanel.SetActive(false);
    }
    public void OffVIrtualFittingPanel()
    {
        VirtualFittingPanel.SetActive(false);
    }
    public void OffVIrtualFittingResultPanel()
    {
        VirtualFittingResultPanel.SetActive(false);
    }
    public void OffSettingPanel()
    {
        SettingPanel.SetActive(false);
    }

}
