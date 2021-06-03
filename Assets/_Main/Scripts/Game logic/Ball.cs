using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    public const float THROW_FORCE      = 500.0f;
    public const float TORQUE           = 15.0f;
    public const float GRAVITY_SCALE    = 1.0f;
    public const float SCALE_DURATION   = 0.66f;
    public const float BALL_RIM_SCALE   = 0.75f;
    public const float BALL_SPAWN_SCALE = 1.9f;
    public const float RANDOM_X         = 1.0f;
    public const float SHADOW_MAGNITUDE = 0.6f;
    public const float FIX_BOUNCE_X     = 0.1f;
    
    public Animator animator;
    public ShadowAnim shadow;

    [NonSerialized]
    public bool touched                 = false;
    [NonSerialized]
    public bool moving                  = false;
    [NonSerialized]
    public bool touchedRim              = false;
    [NonSerialized]
    public bool[] passed                = new bool[2];

    private bool gravity = false;

    private Rigidbody2D rb;
    private Vector3 position;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        position = transform.position;
    }

    protected void FixedUpdate()
    {
        if (gravity)
        {
            rb.AddForce(Vector2.down * 12f);
        }
    }

    private IEnumerator WaitForGravity()
    {
        yield return new WaitForSeconds(0.5f);
        gravity = true;
    }

    public void Throw(Vector2 direction, float f)
    {
        rb.AddForce(direction * THROW_FORCE * f);
        rb.AddTorque(Mathf.Clamp(direction.x * TORQUE, -1, 1), ForceMode2D.Impulse);
        StartCoroutine(WaitForGravity());

        shadow.Set(SCALE_DURATION);
        animator.SetBool("throw", true);

        moving = true;
    }

    public void UpdateBall()
    {
        moving = touchedRim = passed[0] = passed[1] = gravity = false;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = rb.gravityScale = 0;

        animator.SetBool("throw", false);
        animator.SetBool("reset", false);

        Vector3 p = position;
        if (Game.Instance.stage >= 1)
            p = new Vector3(UnityEngine.Random.Range(-RANDOM_X, RANDOM_X), p.y, p.z);

        transform.position = p;
        transform.localScale = Vector3.one * BALL_SPAWN_SCALE;
        transform.eulerAngles = Vector3.zero;
        shadow.enabled = false;
        shadow.transform.position = p + new Vector3(0, -0.5f, 0);
    }

    public void SetSkin(Sprite skin)
    {
        GetComponent<SpriteRenderer>().sprite = skin;
    }

    public bool IsScaling()
    {
        AnimatorStateInfo s = animator.GetCurrentAnimatorStateInfo(0);
        return s.IsName("Throw") || s.IsName("Reset");
    }

    public bool IsReseting()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Reset");
    }

    public IEnumerator IsGameOver()
    {
        yield return new WaitForSeconds(AnimationDurations.RESET_BALL);

        if (!passed[0] || !passed[1])
            Game.Instance.GameOver();
        else
            Game.Instance.UpdateGame();
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "RimTrigger")
        {
            if (collider.name == "Top trigger")
            {
                passed[0] = true;
            }
            else if (collider.name == "Bottom trigger" && passed[0])
            {
                passed[1] = true;
                Game.Instance.AddPoint();
            }
        }
        else if (collider.name == "Bounce trigger")
        {
            //Game.Instance.hoop.Bounce();
        }
    }

    protected void OnTriggerExit2D(Collider2D collider)
    {
        if (!IsReseting() && (collider.name == "Hoop trigger" || collider.tag == "GameBorder"))
        {
            animator.SetBool("reset", true);
            StartCoroutine(IsGameOver());
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Rim")
        {
            rb.velocity = FixVelocity();
            touchedRim = true;
        }
    }

    private Vector2 FixVelocity()
    {
        Vector2 v = rb.velocity;
        float m = touchedRim || Mathf.Abs(v.normalized.x) > 0.3f ? v.magnitude : v.magnitude * 1.33f;
        v.Normalize();

        if (!Game.Instance.hoop.moving && Mathf.Abs(v.x) < FIX_BOUNCE_X)
            return new Vector2(v.x >= 0 ? FIX_BOUNCE_X : -FIX_BOUNCE_X, v.y) * m;

        return v * m;
    }
}
