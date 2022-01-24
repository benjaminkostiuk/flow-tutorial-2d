using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private bool isFacingRight = true;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private LayerMask jumpableGround;

    private enum MovementState
    {
        IDLE,
        RUNNING,
        JUMPING,
        FALLING
    }

    // Start is called before the first frame update
    private void Start()
    {
        this.body = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * moveSpeed, body.velocity.y);

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
        UpdateAnimationState(dirX);
    }

    private void UpdateAnimationState(float dirX)
    {
        MovementState state = MovementState.IDLE;

        if(dirX < 0)
        {
            isFacingRight = false;
            state = MovementState.RUNNING;
        } else if(dirX > 0)
        {
            isFacingRight = true;
            state = MovementState.RUNNING;
        }

        if(body.velocity.y > .1f)
        {
            state = MovementState.JUMPING;
        } else if(body.velocity.y < -.1f)
        {
            state = MovementState.FALLING;
        }

        animator.SetInteger("state", (int) state);
        spriteRenderer.flipX = !isFacingRight;
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
