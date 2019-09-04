using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UIScrollAnim : Anim
{
    private float sourceY;
    private float targetY;
    private ScrollRect scroll;
    private UISkin skin;

    private void Update()
    {
        time += Time.unscaledDeltaTime / duration;

        scroll.verticalNormalizedPosition = Mathf.Lerp(sourceY, targetY, time);

        if (time >= 1)
        {
            enabled = false;
            skin.OnBuy(true);
        }
    }

    public void Set(float duration, float targetY, UISkin skin)
    {
        if (!scroll)
            scroll = GetComponent<ScrollRect>();

        time = scroll.verticalNormalizedPosition != targetY ? 0 : 1;
        sourceY = scroll.verticalNormalizedPosition;
        this.duration = duration;
        this.targetY = targetY;
        this.skin = skin;

        enabled = true;
    }
}
