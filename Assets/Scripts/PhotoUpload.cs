using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class PhotoUpload : MonoBehaviour // capture image + db cloth image upload
{
    public WebCam webCam;
    public GameObject uploadCompleteImage;
    public string imgUrl;

    void Start()
    {
        // another GameObject's field access 
        webCam = GameObject.Find("WebCamManager").GetComponent<WebCam>();
    }

    public void PhotoCaptureBtn()
    {
        Debug.Log("PhotoCaptureBtnTest");
        StartCoroutine(PhotoCapture()); //here
    }

    IEnumerator PhotoCapture()
    {
        byte[] imageBytes = webCam.snapRotated.EncodeToJPG();

        //use debug => Android build not using
        //string localPath = Application.persistentDataPath + "/customer_image.jpg";
        //File.WriteAllBytes(localPath, imageBytes);
        
        
        // Create a new form
        WWWForm form = new WWWForm();

        // Add the image as a field to the form
        form.AddBinaryData("customer_image", imageBytes, "customer_image.jpg", "image/jpeg");
        form.AddField("product", imgUrl);

        // Create a UnityWebRequest object to send the form data
        UnityWebRequest uploadRequest = UnityWebRequest.Post("http://220.149.231.136:8032/fit", form);

        // Send the request and wait for a response
        yield return uploadRequest.SendWebRequest();

        if (uploadRequest.result == UnityWebRequest.Result.Success)
        {
            // Request successful
            Debug.Log("Image upload complete!");
            uploadCompleteImage.SetActive(true);
            // Print the response text
            Debug.Log(uploadRequest.downloadHandler.text);
            Invoke("DeleteUploadCompleteImage", 5f); // 5초후 이미지 업로드 완료 이미지 Off
        }
        else
        {
            // Request failed
            Debug.Log("Image upload failed: " + uploadRequest.error);
        }
    }

    void DeleteUploadCompleteImage()
    {
        uploadCompleteImage.SetActive(false);
    }
}

