using UnityEngine;
using UnityEngine.UI;
using System;

public class UIContinueButton : MonoBehaviour
{
    [NonSerialized]
    public Color defaultColor;
    [NonSerialized]
    public Color disabledColor = new Color(0.6f, 0.6f, 0.6f);

    private float time = 0;

    private RectTransform rt;
    private Button btn;
    private Image img;

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
            //VerifyState();
            time = 0;
        }
    }

    public void VerifyState()
    {
        if ((Game.Instance.continued || !Game.Instance.monetization.IsContinueAdLoaded()) && btn.interactable)
            TurnButtonOff();
        else if (!LeanTween.isTweening(gameObject) && !Game.Instance.continued && Game.Instance.monetization.IsContinueAdLoaded())
            TurnButtonOn();
    }

    public void TurnButtonOn()
    {
        btn.interactable = true;

        Color red = new Color(0.949f, 0.098f, 0.223f);
        Color purple = new Color(0.704f, 0.204f, 0.886f);

        LeanTween.scale(rt, Vector3.one * 1.05f, 0.75f)
            .setEaseInOutCubic()
            .setLoopPingPong()
            .setIgnoreTimeScale(true);
        LeanTween.value(gameObject, c => {
            img.color = c;
        }, defaultColor, red, 0.75f)
            .setLoopPingPong()
            .setIgnoreTimeScale(true);
    }

    public void TurnButtonOff()
    {
        LeanTween.cancel(gameObject);

        rt.localScale = Vector3.one;
        btn.interactable = false;
        img.color = disabledColor;
    }
}
