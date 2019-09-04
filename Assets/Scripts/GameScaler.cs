using UnityEngine;

[ExecuteAlways]
public class GameScaler : MonoBehaviour
{
    public const float DEFAULT_ORTHO = 3.25f;
    public const float DEFAULT_RATIO = 9f / 16f;

    private Vector2Int resolution;

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

    private void Start()
    {
        resolution = GetResolution();
        Camera.main.orthographicSize = ResponsiveOrtho(DEFAULT_RATIO);
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
}
