using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScreenshotManager : MonoBehaviour
{
    public static ScreenshotManager Instance;
    public Image targetImage;
    private string screenshotFilePath;
    public Camera screenshotCamera;
    int screenshotWidth;
    int screenshotHeight;
    private void Awake()
    {
        Instance = this;
    }


    // Hàm chụp ảnh màn hình
    public void CaptureScreenshot()
    {
        screenshotFilePath = Path.Combine(Application.dataPath, $"Resources/Image/room{SaveGame.CurrentRoom}.png");
        screenshotWidth = Screen.width;
        screenshotHeight = Screen.height;

        ScreenCapture.CaptureScreenshot(screenshotFilePath);
    }

    public void LoadScreenshot(Image img, int idRoom)
    {
        screenshotFilePath = Path.Combine(Application.dataPath, $"Resources/Image/room{idRoom}.png");
        StartCoroutine(LoadScreenshot(img));

    }

    // Coroutine để load ảnh từ file và gán vào image element
    IEnumerator LoadScreenshot(Image img)
    {
        // Đợi cho đến khi ảnh được lưu xong
        while (!File.Exists(screenshotFilePath))
        {
            yield return null;
        }

        // Load ảnh từ file
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + screenshotFilePath))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                // Tạo texture từ ảnh vừa load
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                // Tạo sprite từ texture và gán vào image element
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                img.sprite = sprite;
            }
        }
    }

    public void CaptureScreenshotWithoutUI()
    {
        screenshotFilePath = Path.Combine(Application.dataPath, $"Resources/Image/room{SaveGame.CurrentRoom}.png");
        screenshotWidth = Screen.width;
        screenshotHeight = Screen.height;
        Debug.Log(screenshotWidth + " __ " + screenshotHeight);

        StartCoroutine(CaptureScreenshotCoroutine());
    }

    private IEnumerator CaptureScreenshotCoroutine()
    {
        // Tạo một RenderTexture mới
        RenderTexture rt = new RenderTexture(screenshotWidth, screenshotHeight, 24);
        screenshotCamera.targetTexture = rt;

        // Render camera vào RenderTexture
        screenshotCamera.Render();

        // Tạo một Texture2D để lưu dữ liệu từ RenderTexture
        RenderTexture.active = rt;
        Texture2D screenshot = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        screenshot.Apply();

        // Khôi phục trạng thái ban đầu
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // Lưu Texture2D thành file PNG
        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(screenshotFilePath, bytes);

        // Debug log để xác nhận ảnh đã được lưu
        Debug.Log($"Screenshot saved to: {screenshotFilePath}");

        yield return null;
    }
}
