using System;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField][Range(1000, 10000)] private float speed;
    [SerializeField][Range(10, 100)] private float dashSpeed;
    [SerializeField][Range(0.0f, 5.0f)] private float dashCooldown;
    [SerializeField][Range(0.0f, 1.0f)] private float dashDuration;

    // States
    private bool dashReady = true;
    private bool dashing = false;
    private Vector2 moveDirection;
    public bool Running {
        get
        {
            return moveDirection.magnitude > 0;
        }
    }

    // Components
    private Rigidbody2D rb;
    private Transform tf;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dashing)
        {
            moveDirection = new();
            if (Input.GetKey(KeyCode.W)) moveDirection.y += 1;
            if (Input.GetKey(KeyCode.A)) moveDirection.x -= 1;
            if (Input.GetKey(KeyCode.S)) moveDirection.y -= 1;
            if (Input.GetKey(KeyCode.D)) moveDirection.x += 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && Running) Dash();

        // Flips the player if mouse is on the left side of the player
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new(mousePosition.x, mousePosition.y, 0);
        if (tf.position.x < mousePosition.x) tf.localScale = new(1, 1, 1);
        else tf.localScale = new(-1, 1, 1);
    }

    void FixedUpdate()
    {
        if (Running)
        {
            float currentSpeed = speed;
            rb.AddForce(currentSpeed * Time.deltaTime * moveDirection.normalized);
        }
    }

    private void Dash()
    {
        if (!dashReady) return;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dashSpeed * moveDirection.normalized, ForceMode2D.Impulse);
        dashing = true;
        dashReady = false;
        Invoke(nameof(ResetDashing), dashDuration);
        Invoke(nameof(ResetDashReady), dashCooldown);
    }

    private void ResetDashing() => dashing = false;
    private void ResetDashReady() => dashReady = true;
    
}
