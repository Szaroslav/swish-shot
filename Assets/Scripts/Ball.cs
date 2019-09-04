using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    public const float THROW_FORCE      = 25.0f;
    public const float TORQUE           = 20.0f;
    public const float GRAVITY_SCALE    = 6.2f;
    public const float SCALE_DURATION   = 0.66f;
    public const float BALL_SCALE       = 0.5f;
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

    private Rigidbody2D rb;
    private Vector3 position;

    private Vector2 pp, cp;
    private float speed = 15;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        position = transform.position;
    }

    protected void Update()
    {
        if (moving && rb.velocity.y < 0)
            rb.gravityScale = GRAVITY_SCALE * 0.65f;

        //if (moving)
            //Debug.Log(transform.position.y);
    }

    /*protected void FixedUpdate()
    {
        if (moving)
        {
            pp = cp;
            cp = transform.position;
            float t = (pp.y - 1.625f) / (pp.y - cp.y);

            if (moving && pp.y > cp.y && cp.y <= 1.625f)
            {
                Vector2 v = new Vector2(pp.x - (pp.x - cp.x) * t, pp.y - (pp.y - cp.y) * t);
                Debug.Log(v.x + ", " + v.y);
                //Debug.Log("Current position Y: " + cp.y + ", " + (pp.y - (pp.y - cp.y) * t) + " Y X: " + (pp.x - (pp.x - cp.x) * t));
            }
        }
    }*/

    public void Throw(Vector2 direction, float f)
    {
        rb.AddForce(direction * THROW_FORCE * f, ForceMode2D.Impulse);
        rb.AddTorque(Mathf.Clamp(direction.x * TORQUE, -1, 1), ForceMode2D.Impulse);
        rb.gravityScale = GRAVITY_SCALE;
        
        shadow.Set(SCALE_DURATION);
        animator.SetBool("throw", true);

        moving = true;
    }

    public void UpdateBall()
    {
        moving = touchedRim = passed[0] = passed[1] = false;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = rb.gravityScale = 0;

        animator.SetBool("throw", false);
        animator.SetBool("reset", false);

        Vector3 p = position;
        if (Game.Instance.stage >= 1)
            p = new Vector3(UnityEngine.Random.Range(-RANDOM_X, RANDOM_X), p.y, p.z);

        transform.position = p;
        transform.localScale = Vector3.one;
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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "RimTrigger")
        {
            if (collision.name == "Top trigger")
            {
                passed[0] = true;
            }
            else if (collision.name == "Bottom trigger" && passed[0])
            {
                passed[1] = true;
                Game.Instance.AddPoint();
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsReseting() && (collision.name == "Hoop trigger" || collision.tag == "GameBorder"))
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
        if (!touchedRim && Mathf.Abs(v.x) > 1) v.x *= 0.5f;
        float m = v.magnitude;
        v.Normalize();

        if (v.x > -FIX_BOUNCE_X && v.x < FIX_BOUNCE_X)
            return new Vector2(v.x >= 0 ? FIX_BOUNCE_X : -FIX_BOUNCE_X, v.y) * m;

        return v * m;
    }
}
