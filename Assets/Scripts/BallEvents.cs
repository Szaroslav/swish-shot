using UnityEngine;

public class BallEvents : MonoBehaviour
{
    public Ball ball;

    public void EnableHoop()
    {
        Game.Instance.hoop.SetColliders(true);
        Game.Instance.hoop.ExposeHoop(-3);
    }

    public void DisableHoop()
    {
        Game.Instance.hoop.SetColliders(false);
        Game.Instance.hoop.ExposeHoop(0);
    }
}
