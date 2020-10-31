using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class Monetization : MonoBehaviour
{
    public bool testMode;

    float time = 0;
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

        continueAd = CreateRewardedAd(continueAdId, "continueAd");
    }

    void Update()
    {
        /*time += Time.unscaledDeltaTime;

        if (time < 5)
            return;
        
        if (!IsContinueAdLoaded())
            continueAd = CreateRewardedAd(continueAdId, "continueAd");

        time = 0;*/
    }

    RewardedAd CreateRewardedAd(string adId, string adName)
    {
        RewardedAd ad = new RewardedAd(adId);
        
        ad.OnUserEarnedReward +=    (sender, args) => HandleAdReward(sender, args, adName);
        ad.OnAdClosed +=            (sender, args) => HandleAdClosed(sender, args, adId, adName);
        ad.OnAdFailedToLoad +=      (sender, args) => HandleAdFailedToLoad(sender, args, adId, adName);
        ad.OnAdFailedToShow +=      (sender, args) => HandleAdFailedToShow(sender, args, adName);

        AdRequest req = new AdRequest.Builder().Build();
        ad.LoadAd(req);

        return ad;
    }

    public void ShowContinueAd()
    {
        if (IsContinueAdLoaded())
        {
            Game.Instance.ui.OnContinue(true);
            continueAd.Show();
        }       
    }

    public bool IsContinueAdLoaded()
    {
        return continueAd.IsLoaded();
    }

    void HandleAdReward(object sender, Reward args, string adName)
    {
        Game.Instance.continued = true;
        //Game.Instance.ui.OnContinue(false);
        Game.Instance.ui.Continue();
    }

    void HandleAdClosed(object sender, EventArgs args, string adId, string adName)
    {
        continueAd = CreateRewardedAd(adId, adName);
        //Game.Instance.ui.Continue();
    }

    void HandleAdFailedToLoad(object sender, AdErrorEventArgs args, string adId, string adName)
    {
        Debug.LogError($"{adName} failed to load with message: {args.Message}");

        continueAd = CreateRewardedAd(adId, adName);
    }

    void HandleAdFailedToShow(object sender, AdErrorEventArgs args, string adName)
    {
        Debug.LogError($"{adName} failed to show with message: {args.Message}");
    }

    /*public void OnUnityAdsDidFinish(string placementId, ShowResult result)
    {
        if (placementId == rewardedId)
        {
            Game.Instance.continued = true;
            Game.Instance.ui.OnContinue(false);

            if (result == ShowResult.Finished)
                Game.Instance.ui.Continue();
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError(message);
    }


    private void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        Advertisement.AddListener(this);
    }

    public IEnumerator ShowBanner()
    {
        while (Advertisement.IsReady(bannerId))
            yield return null;
        
        Advertisement.Banner.Show(bannerId);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(rewardedId);
        Game.Instance.ui.OnContinue(true);
    }*/
}
