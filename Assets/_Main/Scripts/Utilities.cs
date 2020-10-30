using UnityEngine;
using System.IO;

public class Utilities
{
    public static string TakeScreenshot()
    {

        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture(1);
        string path = Path.Combine(Application.temporaryCachePath, "screenshot.png");
        File.WriteAllBytes(path, screenshot.EncodeToPNG());

        Object.Destroy(screenshot);

        return path;
    }
}
