using UnityEngine;
using System.Collections;

public enum InputPhase { Nothing, Began, Moved, Ended }

public class TouchController : MonoBehaviour
{
    public const float MIN_SWIPE_MAGNITUDE  = 0.5f;
    public const float MAX_SWIPE_MAGNITUDE  = 1.25f;
    public const float MAX_BALL_Y           = 0.15f;
    public float BALL_GRAVITY               = 5.4f;

    public Vector2 minSwipe { get { return new Vector2(-0.55f, 0.85f); } }
    public Vector2 maxSwipe { get { return new Vector2(0.5f, 1.0f); } }

    public Ball ball;
    public Hoop hoop;

    private bool touch = false;
    private Vector2 startPosition;
    private Vector3 ballPosition, shadowPosition, currentPosition, lastPosition;
    private Vector3 gizmoStart, gizmoEnd;

    protected void Update()
    {
        if (!ball.moving && !Game.Instance.paused)
        {
            touch = Input.touchCount > 0 ? true : false;
            lastPosition = currentPosition;
            currentPosition = Camera.main.ScreenToWorldPoint(GetPosition());
            InputPhase phase = GetPhase(currentPosition, lastPosition);

            if (phase != InputPhase.Nothing)
            {
                if (phase == InputPhase.Began)
                {
                    ball.touched = DetectBall(currentPosition);
                    startPosition = currentPosition;
                    ballPosition = ball.transform.position;
                    shadowPosition = ball.shadow.transform.position;
                }
                else if (phase == InputPhase.Moved && ball.touched)
                {
                    Vector2 swipeDelta = (Vector2)currentPosition - startPosition;
                    float f = 1;

                    if (swipeDelta.y > 0)
                    {
                        float offset = Mathf.Clamp(swipeDelta.y / MAX_SWIPE_MAGNITUDE * MAX_BALL_Y, 0, MAX_BALL_Y);
                        f = 1 - 0.0175f * (offset / MAX_BALL_Y);

                        Vector3 v = ballPosition;
                        v.y = ballPosition.y + offset;
                        ball.transform.position = v;
                    }

                    if (swipeDelta.magnitude >= MAX_SWIPE_MAGNITUDE && swipeDelta.y >= 0)
                        ThrowBall(swipeDelta, f);
                }
                else if (phase == InputPhase.Ended && !startPosition.Equals(currentPosition) 
                         && ball.touched && !ball.IsScaling())
                {
                    Vector2 swipeDelta = (Vector2)currentPosition - startPosition;

                    if (swipeDelta.magnitude >= MIN_SWIPE_MAGNITUDE && swipeDelta.magnitude <= MAX_SWIPE_MAGNITUDE 
                        && swipeDelta.y >= 0)
                        ThrowBall(swipeDelta, 1);
                    else
                        ResetInput();
                }
            }
        }
    }

    /*protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(gizmoStart, gizmoEnd);
    }*/

    protected void OnApplicationPause(bool pause)
    {
        if (pause && ball.touched)
            ResetInput();
    }

    public Vector2 GetPosition()
    {
        return touch ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;
    }

    public InputPhase GetPhase(Vector3 cp, Vector3 lp)
    {
        #if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
                return InputPhase.Began;
            else if (Input.GetMouseButton(0) && cp != lp)
                return InputPhase.Moved;
            else if (Input.GetMouseButtonUp(0))
                return InputPhase.Ended;
        #elif UNITY_ANDROID || UNITY_IOS
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
                return InputPhase.Began;
            else if (t.phase == TouchPhase.Moved)
                return InputPhase.Moved;
            else if (t.phase == TouchPhase.Ended)
                return InputPhase.Ended;
        #endif
            return InputPhase.Nothing;
    }

    public void ResetInput()
    {
        ball.touched = false;
        ball.transform.position = ballPosition;
    }

    private bool DetectBall(Vector2 p)
    {
        RaycastHit2D hit = Physics2D.Raycast(p, Vector2.up, 0.001f);

        if (hit && hit.collider.tag == "Ball")
            return true;

        return false;
    }

    private void ThrowBall(Vector2 sd, float f)
    {
        ball.touched = false;
        
        sd.Normalize();
        sd = ClampedVector2(sd, minSwipe, maxSwipe);
        gizmoStart = new Vector3(startPosition.x, startPosition.y, -5);
        gizmoEnd = new Vector3(ballPosition.x + sd.x * 10, ballPosition.y + sd.y * 10, -5);
        Vector2 d = AimAssist(ballPosition, sd);
        d = FixThrow(d);
        ball.Throw(d.normalized, f);
    }

    private Vector2 FixThrow(Vector2 dir)
    {
        Vector2 v = dir * (hoop.rim.transform.position.y / dir.y);
        return new Vector2(v.x / BALL_GRAVITY, v.y);
    }

    private Vector2 AimAssist(Vector2 p, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(p, dir, 25, 1 << 8);
        if (hit && hit.collider.name == "Aim assistance")
        {
            float hitX = hit.collider.transform.position.x;
            float x = hitX - (hitX - hit.point.x) / 1.5f;
            return new Vector2(x, hit.point.y) - p;
        }

        return dir;
    }

    private Vector2 ClampedVector2(Vector2 p, Vector2 min, Vector2 max)
    {
        return new Vector2(Mathf.Clamp(p.x, min.x, max.x), Mathf.Clamp(p.y, min.y, max.y));
    }
}
