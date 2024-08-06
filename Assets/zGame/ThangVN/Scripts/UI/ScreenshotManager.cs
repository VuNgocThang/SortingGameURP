using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScreenshotManager : MonoBehaviour
{
    public Image targetImage;
    private string screenshotFilePath;

    void Start()
    {
        // Đường dẫn để lưu ảnh chụp màn hình
        screenshotFilePath = Path.Combine("Resources/Image/", "screenshot.png");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CaptureScreenshot();
        }
    }

    // Hàm chụp ảnh màn hình
    public void CaptureScreenshot()
    {
        ScreenCapture.CaptureScreenshot(screenshotFilePath);
        StartCoroutine(LoadScreenshot());
    }

    // Coroutine để load ảnh từ file và gán vào image element
    IEnumerator LoadScreenshot()
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
                targetImage.sprite = sprite;
            }
        }
    }
}
