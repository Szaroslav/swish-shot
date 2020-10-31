using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    [Header("Components")]
    public CanvasGroup canvasGroup;
    public Animator animator;

    [Header("Buttons")]
    public Button shareBtn;
    public Button leaderboardBtn;
    public Button rateBtn;
    public Button continueBtn;
    public Button playAgainBtn;
}
