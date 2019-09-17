using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;
using System.Collections;

public enum UIScene { Game, GameOver, Settings }

public class UI : MonoBehaviour
{
    [Header("Sprite atlases")]
    public SpriteAtlas uiAtlas;

    [Header("Animators")]
    public Animator worldUI;
    public Animator screenUI;
    public Animator gameOver;
    public Animator settings;

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
        score[0].text = score[1].text = Progress.Instance.score.ToString();
        bestScore.text = Progress.Instance.bestScore.ToString();
        coins[0].text = Progress.Instance.GetCoinsText();
        coins[1].text = Progress.Instance.GetGameCoinsText();
    }

    public void GameOver()
    {
        ChangeCurrentScene(UIScene.GameOver);
        gameOver.SetBool("gameOver", true);
    }

    public void OnContinue(bool i)
    {
        gameOver.SetBool("continue", i);
    }

    public void Continue()
    {
        ChangeCurrentScene(UIScene.Game);
        Game.Instance.Resume();
        Game.Instance.UpdateGame();
        worldUI.Play("Score in");
        gameOver.SetBool("gameOver", false);
    }

    public void PlayAgain()
    {
        ChangeCurrentScene(UIScene.Game);
        worldUI.Play("Score in");
        gameOver.SetBool("gameOver", false);
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

    public IEnumerator WaitForSceneAnimation(float duration, int sceneIndex)
    {
        yield return new WaitForSecondsRealtime(duration);
        //SceneManager.LoadScene(sceneIndex);
    }

    public void Share()
    {
        string title = Localization.GetText("SHARE_WINDOW_TITLE_TEXT");
        string text = Localization.GetText("SHARE_MESSAGE_TEXT");
        text = text.Replace("@score", Progress.Instance.score.ToString());
        //string screenshotPath = Utilities.TakeScreenshot();
        new NativeShare().SetTitle(title).SetText(text).Share();
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
