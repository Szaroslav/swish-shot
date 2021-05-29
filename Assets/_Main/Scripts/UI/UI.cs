using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;
using System.Collections;
using System.IO;

public enum UIScene { Game, GameOver, Settings }

public class UI : MonoBehaviour
{
    public RenderTexture screenshotTexture;

    [Header("Sprite atlases")]
    public SpriteAtlas uiAtlas;

    [Header("Panels")]
    public Animator worldUI;
    public Animator screenUI;
    public Animator settings;
    public UIGameOver gameOver;

    [Header("Texts")]
    public TextMeshProUGUI[] score;
    public TextMeshProUGUI bestScore;
    public TextMeshProUGUI[] coins;

    [Header("Buttons")]
    public Button continueButton;

    private UIScene currentScene = UIScene.Game;

    public void Show()
    {
        UpdateScores();

        CanvasGroup world = worldUI.GetComponent<CanvasGroup>();
        CanvasGroup screen = screenUI.GetComponent<CanvasGroup>();
        world.alpha = screen.alpha = 1;
        world.interactable = world.blocksRaycasts = screen.interactable = screen.blocksRaycasts = true;
        worldUI.GetComponent<GraphicRaycaster>().enabled = screenUI.GetComponent<GraphicRaycaster>().enabled = true;
    }

    public void Hide()
    {
        CanvasGroup world = worldUI.GetComponent<CanvasGroup>();
        CanvasGroup screen = screenUI.GetComponent<CanvasGroup>();
        world.alpha = screen.alpha = 0;
        world.interactable = world.blocksRaycasts = screen.interactable = screen.blocksRaycasts = false;
        worldUI.GetComponent<GraphicRaycaster>().enabled = screenUI.GetComponent<GraphicRaycaster>().enabled = false;
    }

    public void UpdateScores()
    {
        score[0].text = score[1].text = score[2].text = Progress.Instance.score.ToString();
        bestScore.text = Progress.Instance.bestScore.ToString();
        coins[0].text = Progress.Instance.GetCoinsText();
        coins[1].text = Progress.Instance.GetGameCoinsText();
    }

    public void GameOver()
    {
        ChangeCurrentScene(UIScene.GameOver);
        gameOver.In();
    }

    public void Continue()
    {
        ChangeCurrentScene(UIScene.Game);
        Game.Instance.Resume();
        Game.Instance.UpdateGame();
        worldUI.Play("Score in");
        gameOver.Out();
    }

    public void PlayAgain()
    {
        ChangeCurrentScene(UIScene.Game);
        worldUI.Play("Score in");
        gameOver.Out();
        StartCoroutine(Game.Instance.ResetGame());
    }

    public void Settings()
    {
        if (currentScene == UIScene.GameOver)
            PlayAgain();

        Game.Instance.Pause();
        ChangeCurrentScene(UIScene.Settings);
        screenUI.Play("In");
        settings.Play("In");
    }

    public void SettingsBack(bool instant)
    {
        ChangeCurrentScene(UIScene.Game);
        Game.Instance.Resume();
        screenUI.Play(instant ? "Out instant" : "Out");
        settings.Play(instant ? "Out instant" : "Out");
    }

    public void ChangeCurrentScene(UIScene scene)
    {
        //previousScene = currentScene;
        currentScene = scene;
    }

    public void Leaderboard()
    {
        GPGS.ShowLeaderboard(GPGSIds.leaderboard_endless_mode);
    }

    public void Rate()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.szaredko.basketball");
    }
}
