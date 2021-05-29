using UnityEngine;
using System;
using System.Collections;
using System.Security.Cryptography;

public class Game : MonoBehaviour
{
    public static Game Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<Game>();

            return instance;
        }
    }

    private static Game instance;

    public const int DEFAULT_POINTS     = 1;
    public const int SWISH_POINTS       = 2;
    public const int STAGE_1            = 5;
    public const int STAGE_2            = 15;
    public const float RESET_DURATION   = 0.25f;

    [NonSerialized]
    public bool paused = false;
    [NonSerialized]
    public bool continued = false;
    [NonSerialized]
    public int stage = 0;

    public Monetization monetization;
    public UI ui;
    public Ball ball;
    public Hoop hoop;

    protected void Start()
    {
        ball.SetSkin(Progress.Instance.currentBallSkin);
        ui.UpdateScores();
    }

    public void UpdateGame()
    {
        ball.UpdateBall();
        hoop.UpdateHoop();
    }

    public void AddPoint()
    {
        int p = ball.touchedRim ? DEFAULT_POINTS : SWISH_POINTS;
        Progress.Instance.SetScore(p);
        if (stage == 2) Progress.Instance.SetCoins(p);

        ui.worldUI.Play("Score change");
        UpdateStage();
    }

    public void UpdateStage()
    {
        int score = Progress.Instance.score;

        if (stage == 0 && score >= STAGE_1 && score < STAGE_2)
        {
            stage = 1;
            Progress.Instance.SetCoins(1);
        }
        else if (stage == 1 && score >= STAGE_2)
        {
            stage = 2;
            Progress.Instance.SetCoins(3);
        }
        else if (stage == 2)
        {
            hoop.IncreaseSpeed(0.035f);
        }
    }

    public IEnumerator ResetGame()
    {
        continued = false;
        Progress.Instance.gameCoins = Progress.Instance.score = stage = 0;
        UpdateGame();

        yield return new WaitForSecondsRealtime(AnimationDurations.GAME_OVER_OUT);

        ui.UpdateScores();
        ui.gameOver.uicb.VerifyState();
        ball.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        
        yield return new WaitForSecondsRealtime(AnimationDurations.RESET_SCORE);

        Resume();
        ball.animator.updateMode = AnimatorUpdateMode.Normal;
    }

    public void GameOver()
    {
        Pause();
        ui.GameOver();
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
    }

    public void SignInGPGS()
    {
        GPGS.SignIn();
        ui.screenUI.SetTrigger("loading");
    }
}
