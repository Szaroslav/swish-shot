using UnityEngine;
using UnityEngine.U2D;

public class Progress : Singleton<Progress>
{
    public const string BEST_SCORE_KEY = "best_score";
    public const string COINS_KEY = "coins";
    public const string SKIN_BALL_KEY = "skin_ball_name";

    [HideInInspector]
    public int score;
    [HideInInspector]
    public int bestScore;
    [HideInInspector]
    public float gameCoins;
    [HideInInspector]
    public float coins;

    [HideInInspector]
    public Sprite currentBallSkin;

    protected override void Awake()
    {
        base.Awake();
        gameCoins = score = 0;
        bestScore = LoadInt(BEST_SCORE_KEY);
        coins = LoadFloat(COINS_KEY);
        coins = 1150;
        string n = LoadString(SKIN_BALL_KEY, "basketball_default");
        currentBallSkin = Resources.Load<Sprite>(UIShop.BALL_L_SKINS_PATH + n);
    }

    public void Set(int score, int points)
    {
        SetBestScore(score);
        SetCoins(points);
    }

    public void SetScore(int points)
    {
        score += points;
        SetBestScore(score);
    }

    public void SetBestScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            SaveInt(BEST_SCORE_KEY, bestScore);
            GPGS.SaveScore(bestScore, GPGSIds.leaderboard_endless_mode);
        }
    }

    public void SetCoins(int coins)
    {
        this.coins += coins;
        gameCoins += coins;
        SaveFloat(COINS_KEY, this.coins);
    }

    public string GetCoinsText()
    {
        return coins <= 999 ? ((int)coins).ToString() : "+999";
    }

    public string GetGameCoinsText()
    {
        return ((int)gameCoins).ToString();
    }

    public void SetBallSkin(Sprite skin)
    {
        currentBallSkin = skin;
        SaveString(SKIN_BALL_KEY, skin.name);
    }

    public static void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static int LoadInt(string key, int value = 0)
    {
        return PlayerPrefs.GetInt(key, value);
    }

    public static float LoadFloat(string key, float value = 0)
    {
        return PlayerPrefs.GetFloat(key, value);
    }

    public static string LoadString(string key, string value = "")
    {
        return PlayerPrefs.GetString(key, value);
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}
