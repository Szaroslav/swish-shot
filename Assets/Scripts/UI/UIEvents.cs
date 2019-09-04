using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public void UpdateScores()
    {
        Game.Instance.ui.UpdateScores();
    }
}
