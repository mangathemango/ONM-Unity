using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField][Range(1000, 10000)] private float speed;
    [SerializeField] private Transform hand;
    [SerializeField] private GameObject gunPrefab;

    private Transform gunTransform;
    private GameObject gunObj;
    private Vector2 moveDirection;


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
        moveDirection = new();
        if (Input.GetKey(KeyCode.W)) moveDirection.y += 1;
        if (Input.GetKey(KeyCode.A)) moveDirection.x -= 1;
        if (Input.GetKey(KeyCode.S)) moveDirection.y -= 1;
        if (Input.GetKey(KeyCode.D)) moveDirection.x += 1;
        anim.SetBool("Running", moveDirection.magnitude > 0);

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

        if (Input.GetMouseButton(0)) gunObj.GetComponent<Gun>().Shoot();
    }

    void FixedUpdate()
    {
        if (moveDirection.magnitude > 0)
        {
            rb.AddForce(speed * Time.deltaTime * moveDirection.normalized);
        }
    }
}
