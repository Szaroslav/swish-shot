using UnityEngine;
using GooglePlayGames;

public class GPGS : MonoBehaviour
{
    protected void Start()
    {
        PlayGamesPlatform.Activate();
    }

    public static bool IsAuthenticated()
    {
        return PlayGamesPlatform.Instance.localUser.authenticated;
    }

    public static void SignIn()
    {
        if (!IsAuthenticated())
        {
            Game.Instance.Pause();
            PlayGamesPlatform.Instance.Authenticate((success) => 
            {
                Game.Instance.Resume();
                Game.Instance.ui.screenUI.SetBool("signedIn", true);
            });
        }
    }

    public static void SignOut()
    {
        if (IsAuthenticated())
        {
            PlayGamesPlatform.Instance.SignOut();
        }
    }

    public static void ShowLeaderboard(string leaderboardId)
    {
        if (!IsAuthenticated())
        {
            PlayGamesPlatform.Instance.Authenticate((success) => 
            {
                if (success)
                {
                    PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
                }
            });
        }
        else
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
        }
    }

    public static void SaveScore(int score, string leaderboardId)
    {
        if (IsAuthenticated())
        {
            Social.ReportScore(score, leaderboardId, (success) => { });
        }
    }
}
