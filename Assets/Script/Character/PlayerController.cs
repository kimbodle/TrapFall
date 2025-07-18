using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private bool bIsJump;
    private Rigidbody2D rb;
    private Collider2D collision2D;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 lastMoveDir = Vector2.down;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collision2D = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        move();
        jump();
    }

    private void PlayerInput()
    {
        movement = playerControls.Player.Move.ReadValue<Vector2>();
        bIsJump = playerControls.Player.Jump.ReadValue<bool>();

        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetBool("isMoving", movement.sqrMagnitude > 0.01f);

        // 마지막 이동 방향 저장 (정확히 4방향 입력일 때만)
        if (movement.x != 0 || movement.y != 0)
        {
            lastMoveDir = movement.normalized;

            animator.SetFloat("LastMoveX", lastMoveDir.x);
            animator.SetFloat("LastMoveY", lastMoveDir.y);
        }
    }

    private void move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void jump()
    {
        if (bIsJump)
        {
            //여기서 무적 혹은 콜리전 판정 못하게 바꿀것
        }
    }
}
