using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class Monetization : MonoBehaviour
{
    #if UNITY_ANDROID
    private string gameId = "3237396";
    #elif UNITY_IOS
    private string gameId = "3237397";
    #endif

    public bool testMode;
    public string bannerId;
    public string rewardedId;

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {

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
