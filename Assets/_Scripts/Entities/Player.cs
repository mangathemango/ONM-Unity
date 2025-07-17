using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField][Range(1000, 10000)] private float speed;
    [SerializeField] private Transform hand;
    [SerializeField] private GameObject gunPrefab;

    private Transform gunTf;
    private Vector2 moveDirection;


    private Rigidbody2D rb;
    private Animator anim;
    private Transform tf;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tf = GetComponent<Transform>();
        gunTf = Instantiate(gunPrefab, hand).GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new();
        if (Input.GetKey(KeyCode.W)) moveDirection.y += 1;
        if (Input.GetKey(KeyCode.A)) moveDirection.x -= 1;
        if (Input.GetKey(KeyCode.S)) moveDirection.y -= 1;
        if (Input.GetKey(KeyCode.D)) moveDirection.x += 1;
        anim.SetBool("Running", moveDirection.magnitude > 0);

        // Flips the player if mouse is on the left side of the player
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        float targetScale;
        if (tf.position.x < mousePosition.x) targetScale = 1;
        else targetScale = -1;
        tf.localScale = Vector3.Lerp(tf.localScale, new Vector3(targetScale, 1, 1), 0.1f);

        // Make gun look at cursor
        gunTf.rotation = Quaternion.LookRotation(Vector3.forward, (mousePosition - gunTf.position).normalized);
        gunTf.Rotate(Vector3.forward, 90.0f);
        if (tf.position.x > mousePosition.x) gunTf.Rotate(Vector3.forward, 180.0f);
    }

    void FixedUpdate()
    {
        if (moveDirection.magnitude > 0)
        {
            rb.AddForce(speed * Time.deltaTime * moveDirection.normalized);
        }
    }
}
