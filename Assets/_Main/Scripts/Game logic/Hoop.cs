﻿using UnityEngine;
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

    [Header("Objects")]
    public GameObject rim;
    public CircleCollider2D hoopTrigger;
    public EdgeCollider2D topTrigger;
    public EdgeCollider2D bottomTrigger;
    public GameObject bounceTrigger;

    private Rigidbody2D rb;
    private CapsuleCollider2D[] rimColls;
    private Vector3 rimPos;

    public void SetColliders(bool v)
    {
        foreach (var c in rimColls)
            c.enabled = v;

        hoopTrigger.enabled = topTrigger.enabled = bottomTrigger.enabled = v;
        bounceTrigger.SetActive(v);
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

    public void Bounce()
    {
        LeanTween.cancel(rim.gameObject);
        LeanTween.moveY(rim.gameObject, rimPos.y - 0.035f, 0.25f)
            .setEaseInOutCubic()
            .setOnComplete(() => {
                LeanTween.moveY(rim.gameObject, rimPos.y, 0.25f)
                    .setEaseInOutCubic();
            });
    }

    public void IncreaseSpeed(float v)
    {
        rb.velocity += new Vector2(rb.velocity.x >= 0 ? v : -v, 0);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rimColls = rim.GetComponents<CapsuleCollider2D>();
        rimPos = rim.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Hoop border")
        {
            rb.velocity *= -1;
        }
    }
}
