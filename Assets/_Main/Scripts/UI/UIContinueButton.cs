using UnityEngine;
using UnityEngine.UI;

public class UIContinueButton : MonoBehaviour
{
    private float time = 0;

    private RectTransform rt;
    private Button btn;
    private Image img;

    private Color defaultColor;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        btn = GetComponent<Button>();
        img = GetComponent<Image>();

        defaultColor = img.color;
    }

    private void Update()
    {
        time += Time.unscaledDeltaTime;

        if (time >= 1)
        {
            if (Game.Instance.continued || !Game.Instance.monetization.IsContinueAdLoaded())
                TurnButtonOff();
            else if (!LeanTween.isTweening(gameObject) && !Game.Instance.continued && Game.Instance.monetization.IsContinueAdLoaded())
                TurnButtonOn();

            time = 0;
        }
    }

    private void TurnButtonOn()
    {
        btn.interactable = true;

        LeanTween.scale(rt, Vector3.one * 1.05f, 0.75f)
            .setLoopPingPong()
            .setIgnoreTimeScale(true);
        LeanTween.value(gameObject, c => {
            img.color = c;
        }, defaultColor, new Color(1, 0.337f, 0.345f), 0.75f)
            .setLoopPingPong()
            .setIgnoreTimeScale(true);
    }

    private void TurnButtonOff()
    {
        LeanTween.cancelAll(gameObject);

        rt.localScale = Vector3.one;
        btn.interactable = false;
        img.color = new Color(0.1f, 0.1f, 0.1f);
    }
}
