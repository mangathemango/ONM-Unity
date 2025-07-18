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
    private Vector2 moveDirection;
    public bool Running
    {
        get => moveDirection.magnitude > 0;
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
        // Flips the player if mouse is on the left side of the player
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new(mousePosition.x, mousePosition.y, 0);
        if (tf.position.x < mousePosition.x) tf.localScale = new(1, 1, 1);
        else tf.localScale = new(-1, 1, 1);
    }

    public void Move(Vector2 direction)
    {
        rb.AddForce(speed * Time.deltaTime * direction.normalized);
    }

    public void Dash(Vector2 direction)
    {   
        if (!dashReady || direction.magnitude == 0) return;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dashSpeed * direction.normalized, ForceMode2D.Impulse);
        dashReady = false;
        Invoke(nameof(ResetDashReady), dashCooldown);
    }

    private void ResetDashReady() => dashReady = true;

}
