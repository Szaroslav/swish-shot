using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using GoogleMobileAds.Api;
using System;

public class Monetization : MonoBehaviour
{
    public bool testMode;

    RewardedAd continueAd;

    void Start()
    {
        string continueAdId;
    #if UNITY_ANDROID
        continueAdId = !testMode ? "ca-app-pub-5324115406353383/8512491246" : "ca-app-pub-3940256099942544/5224354917";
    #elif UNITY_IOS
        continueAdId = "ca-app-pub-3940256099942544/1712485313";
    #else
        continueAdId = "unexpected_platform";
    #endif

        MobileAds.Initialize(initStatus => { });

        continueAd = CreateRewardedAd(continueAdId, "continueAd");

        /*continueAd.OnAdOpening += (sender, args) => HandleAdOpening(sender, args);
        continueAd.OnUserEarnedReward += HandleAdReward;
        continueAd.OnAdFailedToShow += HandleAdFailedToShow;

        AdRequest req = new AdRequest.Builder().Build();
        continueAd.LoadAd(req);*/
    }

    RewardedAd CreateRewardedAd(string adId, string adName)
    {
        RewardedAd ad = new RewardedAd(adId);
        
        ad.OnUserEarnedReward +=    (sender, args) => HandleAdReward(sender, args, adName);
        ad.OnAdClosed +=            (sender, args) => HandleAdClosed(sender, args, adId, adName);
        ad.OnAdFailedToShow +=      (sender, args) => HandleAdFailedToShow(sender, args);

        AdRequest req = new AdRequest.Builder().Build();
        ad.LoadAd(req);

        return ad;
    }

    public void ShowContinueAd()
    {
        if (continueAd.IsLoaded())
        {
            Game.Instance.ui.OnContinue(true);
            continueAd.Show();
        }       
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

    void HandleAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.LogError("Continue AD failed to show with message: " + args.Message);
    }

    /*public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult result)
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
