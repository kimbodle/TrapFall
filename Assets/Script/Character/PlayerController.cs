using System;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Collider2D collision2D;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float originalSpeed = 0;
    private float slowEndTime = 0f;
    private bool isSlowed = false;
    private bool isInverted = false;
    private Coroutine invertCoroutine = null;

    private bool isJumpDisabled = false;
    private Coroutine jumpDisableCoroutine = null;

    private Vector2 lastMoveDir = Vector2.down;
    public bool bIsJump = false;

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
        if (isInverted)
        {
            movement.x *= -1;
            movement.y *= -1;
        }

        if (!isJumpDisabled && playerControls.Player.Jump.triggered && !bIsJump)
        {
            bIsJump = true; 
            SoundManager.Instance.PlaySFX(SFXType.Jump);
            StartCoroutine(TemporaryCollisionIgnore());
        }

        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetBool("isMoving", movement.sqrMagnitude > 0.01f);

        // 마지막 이동 방향 저장 (정확히 4방향 입력일 때만)
        if (movement.x != 0 || movement.y != 0)
        {
            lastMoveDir = movement.normalized;
            //SoundManager.Instance.PlaySFX(SFXType.Walk);

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
        bIsJump = true;
        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(0.5f);
        bIsJump = false;
        spriteRenderer.color = Color.white;
    }
    public void DisableJump(float duration)
    {
        if (jumpDisableCoroutine != null)
            StopCoroutine(jumpDisableCoroutine);

        jumpDisableCoroutine = StartCoroutine(JumpDisableWatcher(duration));
    }
    private IEnumerator JumpDisableWatcher(float duration)
    {
        isJumpDisabled = true;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(duration);

        isJumpDisabled = false;
        spriteRenderer.color = Color.white;
    }
    public void ModifySpeed(float multiplier, float duration)
    {
        float now = Time.time;

        if (!isSlowed)
        {
            isSlowed = true;
            originalSpeed = moveSpeed;
            moveSpeed *= multiplier;
            StartCoroutine(SlowWatcher());
        }

        slowEndTime = now + duration;
    }

    private IEnumerator SlowWatcher()
    {
        while (Time.time < slowEndTime)
            yield return null;

        moveSpeed = originalSpeed;
        isSlowed = false;
    }

    public void InvertInput(float duration = 2f)
    {
        if (invertCoroutine != null)
            StopCoroutine(invertCoroutine);

        isInverted = true;
        invertCoroutine = StartCoroutine(InvertWatcher(duration));
    }

    private IEnumerator InvertWatcher(float duration)
    {
        yield return new WaitForSeconds(duration);
        isInverted = false;
        invertCoroutine = null;
    }
}
