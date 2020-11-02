using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    [Header("Components")]
    public CanvasGroup canvasGroup;

    [Header("Buttons")]
    public UIContinueButton uicb;
    public RectTransform shareBtn;
    public RectTransform leaderboardBtn;
    public RectTransform rateBtn;
    public RectTransform continueBtn;
    public RectTransform playAgainBtn;

    [Header("Animation curves")]
    public AnimationCurve buttonsEase;

    public void In()
    {
        LeanTween.cancel(continueBtn.gameObject);
        shareBtn.localScale = leaderboardBtn.localScale = rateBtn.localScale = continueBtn.localScale = playAgainBtn.localScale = Vector3.zero;

        UIContinueButton uicb = continueBtn.GetComponent<UIContinueButton>();
        Button btn = uicb.GetComponent<Button>();
        uicb.GetComponent<Image>().color = btn.interactable ? uicb.defaultColor : uicb.disabledColor;

        LeanTween.value(gameObject, v => { canvasGroup.alpha = v; }, 0, 1, 0.33f)
            .setEaseInCubic()
            .setIgnoreTimeScale(true)
            .setOnComplete(() => {
                canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
            });
        LeanTween.scale(continueBtn, Vector3.one, 0.3f)
            .setEase(LeanTweenType.easeInOutCubic)
            .setIgnoreTimeScale(true)
            .setDelay(0.33f)
            .setOnComplete(() => {
                uicb.VerifyState();
            });

        ScaleButton(shareBtn, 0.3f, 0.33f);
        ScaleButton(leaderboardBtn, 0.3f, 0.43f);
        ScaleButton(rateBtn, 0.3f, 0.53f);
        ScaleButton(playAgainBtn, 0.3f, 0.5f, false);
    }

    public void Out()
    {
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
        
        LeanTween.value(gameObject, v => { canvasGroup.alpha = v; }, 1, 0, 0.33f)
            .setEaseInCubic()
            .setIgnoreTimeScale(true);
    }

    void ScaleButton(RectTransform rt, float t, float d, bool useButtonsEase = true)
    {
        LTDescr ltd = LeanTween.scale(rt, Vector3.one, t)
            .setIgnoreTimeScale(true)
            .setDelay(d);

        if (useButtonsEase)
            ltd.setEase(LeanTweenType.easeOutCubic);
        else
            ltd.setEase(LeanTweenType.easeInOutCubic);
    }
}
