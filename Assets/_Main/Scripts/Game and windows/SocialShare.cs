using UnityEngine;
using System.Collections;
using System.IO;

public class SocialShare : MonoBehaviour
{
    public Camera screenshotCamera;
    public SpriteRenderer ball;

    protected void Start()
    {
        //screenshotCamera.targetTexture.width = Screen.width;
        //screenshotCamera.targetTexture.height = Screen.height;
    }

    public void Share()
    {
        StartCoroutine(WaitForScreenshot());
    }

    private IEnumerator WaitForScreenshot()
    {
        screenshotCamera.gameObject.SetActive(true);
        ball.sprite = Progress.Instance.currentBallSkin;

        yield return new WaitForEndOfFrame();

        RenderTexture rt = screenshotCamera.targetTexture;
        RenderTexture.active = rt;
        Vector2Int size = new Vector2Int(rt.width, rt.height);

        Texture2D screenshot = new Texture2D(size.x, size.y);
        screenshot.ReadPixels(new Rect(0, 0, size.x, size.y), 0, 0);
        screenshot.Apply();

        string screenshotPath = Application.persistentDataPath + "/share_screenshot.png";
        File.WriteAllBytes(screenshotPath, screenshot.EncodeToPNG());
        Destroy(screenshot);

        RenderTexture.active = null;
        screenshotCamera.gameObject.SetActive(false);

        string title = Localization.GetText("SHARE_WINDOW_TITLE_TEXT");
        new NativeShare().SetTitle(title).AddFile(screenshotPath).Share();
    }
}
