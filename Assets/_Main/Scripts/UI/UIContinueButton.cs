using UnityEngine;

public class UIContinueButton : MonoBehaviour
{
    private float time = 0;

    private void Update()
    {
        time += Time.unscaledDeltaTime;

        if (time >= 1)
        {
            AnimatorStateInfo s = Game.Instance.ui.gameOver.animator.GetCurrentAnimatorStateInfo(1);
            if (s.IsName("On") && (Game.Instance.continued || Application.internetReachability == NetworkReachability.NotReachable))
                Game.Instance.ui.gameOver.animator.Play("Off", 1);
            else if (s.IsName("Off") && !Game.Instance.continued && Application.internetReachability != NetworkReachability.NotReachable)
                Game.Instance.ui.gameOver.animator.Play("On", 1);

            time = 0;
        }
    }
}
