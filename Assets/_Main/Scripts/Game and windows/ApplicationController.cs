using UnityEngine;

public class ApplicationController : Singleton<ApplicationController>
{
    public const int FRAME_RATE = 60;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = FRAME_RATE;
        Application.backgroundLoadingPriority = ThreadPriority.Low;
    }
}
