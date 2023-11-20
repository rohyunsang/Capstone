using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PhotoDownload : MonoBehaviour //결과 이미지 다운로드
{
    [SerializeField]
    public RawImage resultImage;
    public GameObject resultImagePanel;
    public string imageURL = "http://220.149.231.136:8032/get_result/final_img.jpg";
    public void ResultDownloadBtn()
    {
        resultImagePanel.SetActive(true);
        StartCoroutine("ResultImageDownload");
    }
    IEnumerator ResultImageDownload()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            //texture = RotatedTexture2D(texture);
            // Assign the downloaded texture to the RawImage component
            
            resultImage.texture = texture;
        }
        else
        {
            Debug.Log("Image download failed: " + www.error);
        }
    }

    Texture2D RotatedTexture2D(Texture2D texture)
    {
        Color32[] pixels = texture.GetPixels32();
        Color32[] rotatedPixels = new Color32[texture.height * texture.width];
        int rotatedIndex = 0;
        for (int i = texture.width - 1; i >= 0; i--)
        {
            for (int j = 0; j < texture.height; j++)
            {
                rotatedPixels[rotatedIndex] = pixels[i + j * texture.width];
                rotatedIndex++;
            }
        }
        Texture2D rotatedTexture = new Texture2D(texture.height, texture.width);
        rotatedTexture.SetPixels32(rotatedPixels);
        rotatedTexture.Apply();
        return rotatedTexture;
    }
}
