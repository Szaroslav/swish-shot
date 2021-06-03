using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIShop : MonoBehaviour
{
    public const int RANDOM_BALL_PRICE      = 100;
    public const float COINS_DURATION       = 0.15f;
    public const string BALL_L_SKINS_PATH   = "Skins/Balls/512x512/";
    
    public SpriteAtlas ballSkins;

    public UICoinsAnim coins;
    public ScrollRect skinsScrollRect;
    public Image currentBallSkin;
    public TextMeshProUGUI randomPrice;

    [HideInInspector]
    public List<UISkin> availableBallSkins = new List<UISkin>();

    private UIScrollAnim scrollAnim;

    protected void Start()
    {
        coins.tmp.text = Progress.Instance.GetCoinsText();
        currentBallSkin.sprite = Progress.Instance.currentBallSkin;
        scrollAnim = skinsScrollRect.GetComponent<UIScrollAnim>();
        randomPrice.text = RANDOM_BALL_PRICE.ToString();

        foreach (Transform t in skinsScrollRect.content.transform)
            t.GetComponent<UISkin>().OnStart();
    }

    public void Show()
    {
        coins.tmp.text = Progress.Instance.GetCoinsText();

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
        if (Progress.Instance.coins > 999 && value <= 999) coins.Set(0, value);
        else if (Progress.Instance.coins <= 999) coins.Set(COINS_DURATION, value);
        Progress.Instance.SetCoins(value - (int)Progress.Instance.coins);
    }

    public void OnRandom()
    {
        if (Progress.Instance.coins >= RANDOM_BALL_PRICE && availableBallSkins.Count > 0 && !scrollAnim.enabled)
        {
            SetCoins((int)Progress.Instance.coins - RANDOM_BALL_PRICE);

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
