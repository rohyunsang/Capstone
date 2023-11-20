using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class WebCam : MonoBehaviour
{
    // 웹캠 변수
    public RawImage display;

    [SerializeField]
    public WebCamTexture camTexture;
    private int currentIndex = 0;
    public Texture2D snap;
    public Texture2D snapSlicedAndRotated;
    public Texture2D snapRotated;

    // 타이머 변수
    public Text timerText;
    int threeSecond = 3;
    public GameObject countingImage;

    // 캡처된 이미지 확인용 변수
    public Image captureImage;

    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            Permission.RequestUserPermission(Permission.Camera);

        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing) // 전면 카메라를 먼저 찾습니다.
            {
                currentIndex = i;
                break;
            }
        }

        // 전면 카메라를 찾지 못한 경우, 후면 카메라를 사용하도록 설정합니다.
        if (currentIndex == -1 && devices.Length > 0)
        {
            currentIndex = 0;
        }

        captureImage.gameObject.SetActive(false);
    }

    public void WebCamPlayButton()
    {
        Debug.Log("WebCamPlayButtonClicked");
        WebCamDevice device = WebCamTexture.devices[currentIndex];
        camTexture = new WebCamTexture(device.name, Screen.width, Screen.height);
        camTexture.requestedFPS = 30;
        camTexture.requestedWidth = Screen.width;

        // 카메라의 기본 회전 각도를 확인하고, RawImage의 회전을 조정합니다.

        display.texture = camTexture;
        camTexture.Play();
        display.gameObject.SetActive(true);
    }

    public void WebCamCapture()
    {
        snap = new Texture2D(camTexture.width, camTexture.height, TextureFormat.RGBA32, false);
        snap.SetPixels(camTexture.GetPixels());
        snap.Apply();

        //snapSlicedAndRotated = CropTexture2D(snap, 576, 0, 768, 1024);
        snapRotated = RotatedTexture2D(snap);


        captureImage.gameObject.SetActive(true);
        captureImage.sprite = Sprite.Create(snap, new Rect(0, 0, snap.width, snap.height), new Vector2(0.5f, 0.5f));
        display.gameObject.SetActive(false);
    }

    Texture2D RotatedTexture2D(Texture2D snap)
    {
        Color32[] pixels = snap.GetPixels32();
        Color32[] rotatedPixels = new Color32[snap.height * snap.width];
        int rotatedIndex = 0;
        for (int i = 0; i < snap.width; i++)
        {
            for (int j = snap.height - 1; j >= 0; j--)
            {
                rotatedPixels[rotatedIndex] = pixels[i + j * snap.width];
                rotatedIndex++;
            }
        }

        Texture2D rotatedTexture = new Texture2D(snap.height, snap.width);
        rotatedTexture.SetPixels32(rotatedPixels);
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    public void WebCamCaptureButton()
    {
        display.gameObject.SetActive(true);
        captureImage.gameObject.SetActive(false);
        threeSecond = 3;
        countingImage.SetActive(true);
        StartCoroutine(CountingThreeSecond());
        Invoke("WebCamCapture", 3f);
    }

    IEnumerator CountingThreeSecond()
    {
        for (int i = 0; i < 3; i++)
        {
            timerText.text = threeSecond.ToString();
            yield return new WaitForSeconds(1f);
            threeSecond--;
        }
        timerText.fontSize = 20;
        timerText.text = "캡처!";
    }

    public void WebCamStopButton() // using Btn : CloseBtn in Virtual Fitting Panel
    {
        countingImage.SetActive(false);
        camTexture.Stop();
        threeSecond = 3;
        captureImage.gameObject.SetActive(false);
    }

}