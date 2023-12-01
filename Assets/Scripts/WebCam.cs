using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using com.example;

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
    public GameObject countingImage;

    // 캡처된 이미지 확인용 변수
    public Image captureImage;

    // sprite
    public Sprite cameraIconImage;
    public Sprite retryIconImage;
    public Image captureButtonImage;


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
        

        display.texture = camTexture;
        camTexture.Play();
        display.gameObject.SetActive(true);
    }

    public void WebCamCapture() // using WebCamCaptureButton
    {
        countingImage.SetActive(false);

        snap = new Texture2D(camTexture.width, camTexture.height, TextureFormat.RGBA32, false);
        snap.SetPixels(camTexture.GetPixels());
        snap.Apply();

        captureImage.gameObject.SetActive(true);
        captureImage.sprite = Sprite.Create(snap, new Rect(0, 0, snap.width, snap.height), new Vector2(0.5f, 0.5f));
        display.gameObject.SetActive(false);

        captureButtonImage.sprite = retryIconImage;
        SaveSnapAsJpg();
    }

    public void WebCamCaptureButton()
    {
        display.gameObject.SetActive(true);
        captureImage.gameObject.SetActive(false);
        countingImage.SetActive(true);
        StartCoroutine(Timer());
        Invoke("WebCamCapture", 5f);
    }

    public void WebCamStopButton() // using Btn : CloseBtn in Virtual Fitting Panel
    {
        camTexture.Stop();
        captureImage.gameObject.SetActive(false);
    }

    public void SaveSnapAsJpg()
    {
        // snap이 null이 아닌지 확인
        if (snap == null)
        {
            Debug.LogError("Snap is null");
            return;
        }

        // JPG로 인코딩
        byte[] jpgBytes = snap.EncodeToJPG();

        SupaManager.Instance.UploadImage(jpgBytes);
    }

    IEnumerator Timer()
    {
        int timeLeft = 5; // 타이머 시작 시간 설정
        while (timeLeft >= 0)
        {
            timerText.text = timeLeft.ToString(); // 타이머 텍스트 업데이트
            yield return new WaitForSeconds(1); // 1초 기다림
            timeLeft--; // 시간 감소
        }
    }

}