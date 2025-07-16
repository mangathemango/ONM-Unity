using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Transform tf;

    private Vector2 direction;
    [SerializeField][Range(1000, 10000)] private float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = new();
        if (Input.GetKey(KeyCode.W)) direction.y += 1;
        if (Input.GetKey(KeyCode.A)) direction.x -= 1;
        if (Input.GetKey(KeyCode.S)) direction.y -= 1;
        if (Input.GetKey(KeyCode.D)) direction.x += 1;
        anim.SetBool("Running", direction.magnitude > 0);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        sr.flipX = mousePosition.x < tf.position.x;
    }

    void FixedUpdate()
    {
        if (direction.magnitude > 0)
        {
            rb.AddForce(speed * Time.deltaTime * direction.normalized);
        }
    }
}
