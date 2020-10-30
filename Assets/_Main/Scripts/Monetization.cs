using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
//using GoogleMobileAds.Api;

public class Monetization : MonoBehaviour
{
    public bool testMode;
    public string bannerId;
    public string rewardedId;

    /*RewardedAd continueAd;

    void Start()
    {
        string continueAdId;
#if UNITY_ANDROID
        continueAdId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
        continueAdId = "ca-app-pub-3940256099942544/1712485313";
#else
        continueAdId = "unexpected_platform";
#endif
        MobileAds.Initialize("ca-app-pub-5324115406353383~8923554787");

        continueAd = new RewardedAd(continueAdId);
        AdRequest req = new AdRequest.Builder().Build();
        continueAd.LoadAd(req);
    }

    public void ShowContinueAd()
    {
        if (continueAd.IsLoaded())
            continueAd.Show();
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
