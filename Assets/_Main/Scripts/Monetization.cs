using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;

public class Monetization : MonoBehaviour
{
    public bool testMode;

    bool earnReward = false;
    string continueAdId;

    RewardedAd continueAd;

    void Start()
    {
    #if UNITY_ANDROID
        continueAdId = !testMode ? "ca-app-pub-5324115406353383/8512491246" : "ca-app-pub-3940256099942544/5224354917";
    #elif UNITY_IOS
        continueAdId = "ca-app-pub-3940256099942544/1712485313";
    #else
        continueAdId = "unexpected_platform";
    #endif

        MobileAds.Initialize(initStatus => { });

        continueAd = CreateRewardedAd(continueAdId);
    }

    void Update()
    {
        if (earnReward)
        {
            Game.Instance.continued = true;
            Game.Instance.ui.Continue();

            earnReward = false;
        }
    }

    RewardedAd CreateRewardedAd(string adId)
    {
        string adName = "";
        if (adId == continueAdId)
            adName = "Continue AD";

        RewardedAd ad = new RewardedAd(adId);

        ad.OnAdOpening +=           (sender, args) => HandleAdOpening(sender, args);
        ad.OnUserEarnedReward +=    (sender, args) => HandleAdReward(sender, args);
        ad.OnAdClosed +=            (sender, args) => HandleAdClosed(sender, args, adId);
        ad.OnAdFailedToLoad +=      (sender, args) => HandleAdFailedToLoad(sender, args, adId, adName);
        ad.OnAdFailedToShow +=      (sender, args) => HandleAdFailedToShow(sender, args, adName);

        AdRequest req = new AdRequest.Builder().Build();
        ad.LoadAd(req);

        return ad;
    }

    public void ShowContinueAd()
    {
        if (IsContinueAdLoaded())
            continueAd.Show();
    }

    public bool IsContinueAdLoaded()
    {
        return continueAd.IsLoaded();
    }

    void HandleAdOpening(object sender, EventArgs args)
    {
        Game.Instance.ui.gameOver.uicb.VerifyState();
    }

    void HandleAdReward(object sender, Reward args)
    {
        earnReward = true;
    }

    void HandleAdClosed(object sender, EventArgs args, string adId)
    {
        continueAd = CreateRewardedAd(adId);
    }

    void HandleAdFailedToLoad(object sender, AdErrorEventArgs args, string adId, string adName)
    {
        Debug.LogError($"{adName} failed to load with message: {args.Message}");

        continueAd = CreateRewardedAd(adId);
    }

    void HandleAdFailedToShow(object sender, AdErrorEventArgs args, string adName)
    {
        Debug.LogError($"{adName} failed to show with message: {args.Message}");
    }
}
