using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Transform spriteTransform;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x > 0)
        {
            spriteTransform.localScale = new Vector3(1, 1, 1);
        }
        else if (movement.x < 0)
        {
            spriteTransform.localScale = new Vector3(-1, 1, 1);
        }

        movement = movement.normalized;

        bool isMoving = movement != Vector2.zero;
        animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public bool IsMoving()
    {
        return movement != Vector2.zero;
    }
}