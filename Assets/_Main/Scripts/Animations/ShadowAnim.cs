using UnityEngine;

public class ShadowAnim : Anim
{
    public Transform ball;

    private float sourceY;
    private float targetY           = -1.75f;

    public void Set(float duration)
    {
        time = 0;
        sourceY = transform.position.y;
        this.duration = duration;

        enabled = true;
    }

    private void Update()
    {
        time += Time.deltaTime / duration;

        float y = Mathf.Lerp(sourceY, targetY, time);
        transform.position = new Vector3(ball.position.x, y, transform.position.z);

        if (time >= 1)
            enabled = false;
    }
}
