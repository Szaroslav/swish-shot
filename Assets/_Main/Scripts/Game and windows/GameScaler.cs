using UnityEngine;

[ExecuteAlways]
public class GameScaler : MonoBehaviour
{
    public const float DEFAULT_ORTHO = 3.25f;
    public const float DEFAULT_RATIO = 9f / 16f;

    public RectTransform game;

    private Vector2Int resolution;
    private Rect safeArea;

    public static float AspectRatio()
    {
        return (float)Screen.width / Screen.height;
    }

    public static float ResponsiveOrtho(float ar)
    {
        float car = AspectRatio();
        return DEFAULT_ORTHO * ar / car;
    }

    public static Vector2Int GetResolution()
    {
        return new Vector2Int(Screen.width, Screen.height);
    }

    public static Rect GetSafeArea()
    {
        return Screen.safeArea;
    }

    private void Start()
    {
        resolution = GetResolution();
        ResolutionChanged();
    }

    private void Update()
    {
        if (resolution.x != Screen.width || resolution.y != Screen.height)
            ResolutionChanged();

        resolution = GetResolution();
    }

    private void ResolutionChanged()
    {
        Camera.main.orthographicSize = ResponsiveOrtho(DEFAULT_RATIO);
    }

    private void SafeAreaChanged()
    {
        Vector2 aMin = safeArea.position;
        Vector2 aMax = safeArea.position + safeArea.size;
        aMin.x /= Screen.width;
        aMax.x /= Screen.width;
        aMin.y /= Screen.height;
        aMax.y /= Screen.height;
        game.anchorMin = aMin;
        game.anchorMax = aMax;
    }
}
