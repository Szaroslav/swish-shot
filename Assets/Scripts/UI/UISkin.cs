using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UISkin : MonoBehaviour
{
    public const string BALL_KEY = "skin_ball_name";
    public const string SAVE_KEY = "ui_skin_ball_";
    
    public bool primary;
    public string skinName;
    public int price;
    public UIShop ui;
    public Image skinImage;
    public Button buyButton;

    private Sprite skin;

    public void OnStart()
    {
        skin = ui.ballSkins.GetSprite(skinName);
        skin.name = skinName;
        skinImage.sprite = skin;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = price.ToString();
        buyButton.onClick.AddListener(() => OnBuy(false));

        if (primary || Progress.LoadInt(SAVE_KEY + skinName) == 1)
            Own();
        else
            ui.availableBallSkins.Add(this);
    }

    public void OnBuy(bool randomUnlock)
    {
        if (randomUnlock || Progress.Instance.coins >= price)
        {
            Own();
            ChangeSkin();
            Progress.SaveInt(SAVE_KEY + skinName, 1);
            
            if (!randomUnlock)
                ui.SetCoins((int)Progress.Instance.coins - price);
        }
    }

    public void Own()
    {
        ui.availableBallSkins.Remove(this);
        buyButton.enabled = false;
        buyButton.image.color = new Color(0.5f, 0.5f, 0.5f);
        buyButton.transform.GetChild(0).gameObject.SetActive(false);
        buyButton.transform.GetChild(1).gameObject.SetActive(true);
        GetComponent<Button>().interactable = true;
    }

    public void ChangeSkin()
    {
        if (skin != ui.currentBallSkin.sprite)
        {
            ui.currentBallSkin.sprite = skin;
            StartCoroutine(LoadSkinAsync(UIShop.BALL_L_SKINS_PATH + skinName));
            GetComponent<Animator>().SetTrigger("changeSkin");
        }
    }

    private IEnumerator LoadSkinAsync(string path)
    {
        ResourceRequest req = Resources.LoadAsync<Sprite>(path);

        while (!req.isDone)
            yield return null;

        Sprite s = req.asset as Sprite;
        ui.currentBallSkin.sprite = s;
        Progress.Instance.SetBallSkin(s);
        Game.Instance.ball.SetSkin(s);
    }
}
