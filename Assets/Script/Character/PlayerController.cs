using System;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private bool bIsJump = false;
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
    }

    private void PlayerInput()
    {
        movement = playerControls.Player.Move.ReadValue<Vector2>();
        if (playerControls.Player.Jump.triggered && !bIsJump)
        {
            bIsJump = true;
            StartCoroutine(TemporaryCollisionIgnore());
        }

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

    IEnumerator TemporaryCollisionIgnore()
    {
        // 모든 타일과 충돌 끄기
        foreach (var tile in GameObject.FindGameObjectsWithTag("Tile"))
        {
            Collider2D tileCol = tile.GetComponent<Collider2D>();
            if (tileCol != null)
            {
                Debug.Log("true");
                Physics2D.IgnoreCollision(collision2D, tileCol, true);
            }
        }

        spriteRenderer.color = Color.black;

        yield return new WaitForSeconds(0.5f); // 무적 시간

        // 다시 충돌 켜기
        foreach (var tile in GameObject.FindGameObjectsWithTag("Tile"))
        {
            Collider2D tileCol = tile.GetComponent<Collider2D>();
            if (tileCol != null)
            {
                Debug.Log("false");
                Physics2D.IgnoreCollision(collision2D, tileCol, false);
            }
        }
        spriteRenderer.color = Color.white;
        bIsJump = false;
    }
}
