using UnityEngine;
using System;

public enum HoopMovement { Horizontal, Vertical }

[RequireComponent(typeof(Rigidbody2D))]
public class Hoop : MonoBehaviour
{
    public const float MOVING_SPEED         = 0.33f;
    public const float RESET_MOVE_DURATION  = 0.15f;

    public Vector3 defaultPosition;

    [NonSerialized]
    public bool moving = false;

    public GameObject rim;
    public CircleCollider2D hoopTrigger;
    public EdgeCollider2D topTrigger;
    public EdgeCollider2D bottomTrigger;

    private Rigidbody2D rb;

    public void SetColliders(bool v)
    {
        BoxCollider2D[] colliders = rim.GetComponents<BoxCollider2D>();

        foreach (BoxCollider2D c in colliders)
            c.enabled = v;

        hoopTrigger.enabled = topTrigger.enabled = bottomTrigger.enabled = v;
    }

    public void ExposeHoop(float z)
    {
        Vector3 p = rim.transform.position; p.z = z;
        rim.transform.position = p;
    }

    public void UpdateHoop()
    {
        if (moving && Game.Instance.stage < 2)
        {
            moving = false;
            transform.position = defaultPosition;
            rb.velocity = Vector2.zero;
        }
        else if (!moving && Game.Instance.stage >= 2)
        {
            Move();
        }
    }

    public void Move()
    {
        moving = true;
        rb.velocity = (UnityEngine.Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right) * MOVING_SPEED;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Hoop border")
        {
            rb.velocity = (rb.velocity.x > 0 ? Vector2.left : Vector2.right) * MOVING_SPEED;
        }
    }
}
