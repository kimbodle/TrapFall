﻿using System;
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
    SoundManager soundManager;

    private float originalSpeed = 0;
    private float slowEndTime = 0f;
    private bool isSlowed = false;
    private bool isInverted = false;
    private Coroutine invertCoroutine = null;

    private bool isJumpDisabled = false;
    private Coroutine jumpDisableCoroutine = null;

    private Vector2 lastMoveDir = Vector2.down;
    public bool bIsJump = false;
    private bool isInputBlocked = false;

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
        soundManager = SoundManager.Instance;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        if (isInputBlocked)
        {
            movement = Vector2.zero;
            return;
        }
        PlayerInput();
    }

    public void BlockInput(bool block)
    {
        isInputBlocked = block;
    }
    private void FixedUpdate()
    {
        move();
        bool isMoving = movement.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            if (!soundManager.IsWalkingSoundPlaying())
            {
                soundManager.PlayWalkingLoop();
            }
        }
        else
        {
            soundManager.StopWalkingLoop();
        }
    }
    public void KnockbackTo(Vector3 targetPos)
    {
        Debug.Log("플레이어 넉백");
        StartCoroutine(KnockbackRoutine(targetPos));
    }

    private IEnumerator KnockbackRoutine(Vector3 targetPos)
    {
        float duration = 0.3f;
        Vector3 start = transform.position;
        float time = 0f;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(start, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
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
            soundManager.PlaySFX(SFXType.Jump);
            StartCoroutine(TemporaryCollisionIgnore());
            animator.SetTrigger("Jump");
        }

        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetBool("isMoving", movement.sqrMagnitude > 0.01f);

        // 마지막 이동 방향 저장 (정확히 4방향 입력일 때만)
        if (movement.x != 0 || movement.y != 0)
        {
            lastMoveDir = movement.normalized;
            //soundManager.PlaySFX(SFXType.Walk);

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
        yield return new WaitForSeconds(0.5f);
        bIsJump = false;
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

        yield return new WaitForSeconds(duration);

        isJumpDisabled = false;
    }
    public void ModifySpeed(float multiplier, float duration)
    {
        float now = Time.time;

        slowEndTime = Mathf.Max(slowEndTime, now + duration);
        if (!isSlowed)
        {
            isSlowed = true;
            originalSpeed = moveSpeed;
            moveSpeed *= multiplier;

            StartCoroutine(SlowWatcher());
        }
    }

    private IEnumerator SlowWatcher()
    {
        while (Time.time < slowEndTime)
        {
            yield return null;
        }

        // 느려진 시간이 끝나면 복원
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
