using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using com.example;
using System.Runtime.InteropServices;

public class WebCam : MonoBehaviour
{
    // 웹캠 변수
    public RawImage display;

    [SerializeField]
    public WebCamTexture camTexture;
    private int currentIndex = 0;
    public Texture2D snap;
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
    public GameObject silhouetteRawImage;

    public bool isRunningCam = false;


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
        if (isRunningCam)
        {

        }
        else
        {
            display.gameObject.SetActive(true);
            silhouetteRawImage.SetActive(true);
            Debug.Log("WebCamPlayButtonClicked");

            WebCamDevice device = WebCamTexture.devices[currentIndex];
            camTexture = new WebCamTexture(device.name, Screen.width, Screen.height);
            camTexture.requestedFPS = 30;

            display.texture = camTexture;
            camTexture.Play();
            display.gameObject.SetActive(true);
            isRunningCam = true;
        }
        
    }

    public void WebCamCapture() // using WebCamCaptureButton
    {
        silhouetteRawImage.SetActive(false);
        countingImage.SetActive(false);

        snap = new Texture2D(camTexture.width, camTexture.height, TextureFormat.RGBA32, false);
        snap.SetPixels(camTexture.GetPixels());
        snap.Apply();

        captureImage.gameObject.SetActive(true);
        captureImage.sprite = Sprite.Create(snap, new Rect(0, 0, snap.width, snap.height), new Vector2(0.5f, 0.5f));

        captureButtonImage.sprite = retryIconImage;
        
        SaveSnapAsJpg();
    }
    public void OriginIcon()
    {
        captureButtonImage.sprite = cameraIconImage;
    }

    public void WebCamCaptureButton()
    {
        silhouetteRawImage.SetActive(true);
        display.gameObject.SetActive(true);
        captureImage.gameObject.SetActive(false);
        countingImage.SetActive(true);
        StartCoroutine(Timer());
        Invoke("WebCamCapture", 5f);
    }

    public static Texture2D RotateImage(Texture2D tex, float angleDegrees)
    {
        int originalWidth = tex.width;
        int originalHeight = tex.height;

        // 90도 또는 270도 회전 시, 너비와 높이가 바뀜
        int width = angleDegrees == 90 || angleDegrees == 270 ? originalHeight : originalWidth;
        int height = angleDegrees == 90 || angleDegrees == 270 ? originalWidth : originalHeight;

        Texture2D rotatedTex = new Texture2D(width, height, tex.format, false);

        float halfOriginalWidth = originalWidth * 0.5f;
        float halfOriginalHeight = originalHeight * 0.5f;
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;

        float phi = Mathf.Deg2Rad * angleDegrees;
        float cosPhi = Mathf.Cos(phi);
        float sinPhi = Mathf.Sin(phi);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float cX = x - halfWidth;
                float cY = y - halfHeight;
                int originalX = Mathf.RoundToInt(cosPhi * cX - sinPhi * cY + halfOriginalWidth);
                int originalY = Mathf.RoundToInt(sinPhi * cX + cosPhi * cY + halfOriginalHeight);

                bool insideOriginalBounds = (originalX >= 0 && originalX < originalWidth) &&
                                            (originalY >= 0 && originalY < originalHeight);

                rotatedTex.SetPixel(x, y, insideOriginalBounds ? tex.GetPixel(originalX, originalY) : new Color(0, 0, 0, 0));
            }
        }

        rotatedTex.Apply();
        return rotatedTex;
    }

    public void SaveSnapAsJpg()
    {
        snapRotated = RotateImage(snap, 270);
        // snapRotated이 null이 아닌지 확인
        if (snapRotated == null)
        {
            Debug.LogError("snapRotated is null");
            return;
        }

        // JPG로 인코딩
        byte[] jpgBytes = snapRotated.EncodeToJPG();

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