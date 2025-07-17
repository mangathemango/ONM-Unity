using System;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField][Range(1000, 10000)] private float speed;
    [SerializeField][Range(5000, 50000)] private float dashSpeed;
    [SerializeField][Range(0.0f, 5.0f)] private float dashCooldown;
    [SerializeField][Range(0.0f, 1.0f)] private float dashDuration;
    [SerializeField] private Transform hand;
    [SerializeField] private GameObject gunPrefab;

    // States
    private bool dashReady = true;
    private bool dashing = false;
    private Vector2 moveDirection;
    private bool Running {
        get
        {
            return moveDirection.magnitude > 0;
        }
    }

    // Components
    private GameObject gunObj;
    private Transform gunTransform;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform tf;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tf = GetComponent<Transform>();
        gunObj = Instantiate(gunPrefab, hand);
        gunTransform = gunObj.transform;
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
        if (Input.GetMouseButton(0)) gunObj.GetComponent<Gun>().Shoot();
        if (Input.GetKeyDown(KeyCode.LeftShift) && Running) Dash();


        anim.SetBool("Running", Running);

        // Flips the player if mouse is on the left side of the player
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new(mousePosition.x, mousePosition.y, 0);
        if (tf.position.x < mousePosition.x) tf.localScale = new(1, 1, 1);
        else tf.localScale = new(-1, 1, 1);

        // Make gun look at cursor
        Vector3 gunDirection = (mousePosition - gunTransform.position).normalized;
        gunTransform.rotation = Quaternion.LookRotation(Vector3.forward, gunDirection);
        gunTransform.Rotate(Vector3.forward, 90.0f);
        if (tf.position.x > mousePosition.x) gunTransform.Rotate(Vector3.forward, 180.0f);


    }

    void FixedUpdate()
    {
        if (Running)
        {
            float currentSpeed;
            if (dashing) currentSpeed = dashSpeed;
            else currentSpeed = speed;
            rb.AddForce(currentSpeed * Time.deltaTime * moveDirection.normalized);
        }
    }

    private void Dash()
    {
        if (!dashReady) return;
        dashing = true;
        dashReady = false;
        Invoke(nameof(ResetDashing), dashDuration);
        Invoke(nameof(ResetDashReady), dashCooldown);
    }

    private void ResetDashing()
    {
        dashing = false;
    }
    private void ResetDashReady()
    {
        dashReady = true;
    }
}
