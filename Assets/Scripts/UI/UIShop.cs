using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using System.Collections;
using System.Collections.Generic;

public class UIShop : MonoBehaviour
{
    public const float COINS_DURATION = 0.15f;
    public const string BALL_L_SKINS_PATH = "Skins/Balls/512x512/";


    public SpriteAtlas ballSkins;

    public UICoinsAnim coins;
    public ScrollRect skinsScrollRect;
    public Image currentBallSkin;

    [HideInInspector]
    public List<UISkin> availableBallSkins = new List<UISkin>();

    private UIScrollAnim scrollAnim;

    protected void Start()
    {
        coins.tmp.text = Progress.Instance.GetCoinsText();
        currentBallSkin.sprite = Progress.Instance.currentBallSkin;
        scrollAnim = skinsScrollRect.GetComponent<UIScrollAnim>();

        foreach (Transform t in skinsScrollRect.content.transform)
            t.GetComponent<UISkin>().OnStart();
    }

    public void Show()
    {
        CanvasGroup c = GetComponent<CanvasGroup>();
        c.alpha = 1;
        c.interactable = c.blocksRaycasts = GetComponent<GraphicRaycaster>().enabled = true;
        Game.Instance.ui.Hide();
    }

    public void Hide()
    {
        Game.Instance.ui.Show();
        Game.Instance.ui.SettingsBack(true);
        StartCoroutine(WaitForHide());
    }

    public IEnumerator WaitForHide()
    {
        yield return null;

        CanvasGroup c = GetComponent<CanvasGroup>();
        c.alpha = 0;
        c.interactable = c.blocksRaycasts = GetComponent<GraphicRaycaster>().enabled = false;
    }

    public void SetCoins(int value)
    {
        coins.Set(COINS_DURATION, value);
        Progress.Instance.coins -= 1;
    }

    public void OnRandom()
    {
        if (Progress.Instance.coins >= 1 && availableBallSkins.Count > 0 && !scrollAnim.enabled)
        {
            SetCoins((int)Progress.Instance.coins - 1);

            float scHeight = skinsScrollRect.content.rect.height;
            float svHeight = skinsScrollRect.viewport.rect.height;
            int maxY = (int)(scHeight - svHeight);
            int i = Random.Range(0, availableBallSkins.Count - 1);
            RectTransform t = availableBallSkins[i].GetComponent<RectTransform>();
            float skinY = Mathf.Clamp(Mathf.Abs(t.anchoredPosition.y) - t.rect.height / 2, 0, maxY);
            scrollAnim.Set(0.2f, 1 - skinY / maxY, t.GetComponent<UISkin>());
        }
    }
}
