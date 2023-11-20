using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryOnButton : MonoBehaviour  // Webcam TryOn
{
    public WebCam webCam;
    public PanelManager panelManager;
    public GameObject virtualPanel;
    public GameObject webCamObject;

    // Start is called before the first frame update
    void Start() // connect Inspector Component
    {
        webCam = GameObject.Find("WebCamManager").GetComponent<WebCam>();
        virtualPanel = GameObject.Find("PanelManager").GetComponent<PanelManager>().VirtualFittingPanel;
    }
        

    public void OnPrefabButton(){  // using Prefab : ProductSample
        virtualPanel.SetActive(true);
        webCam.WebCamPlayButton();
        //GameObject.Find("ImageUploadBtn").GetComponent<PhotoUpload>().imgUrl = transform.GetComponent<ImageURL>().imgUrl;
    } 
}
