using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;

    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;

    [SerializeField] private float overlapSize = 2f;
    private bool canDash = true;
    private bool grounded = true;
    private bool isDashing;

    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    // 🎨 SPRITES
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    private Sprite currentSprite;

    private void Update()
    {
        if (isDashing)
            return;

        horizontal = Input.GetAxisRaw("Horizontal");

        Sprite newSprite;

        if (horizontal < 0)
            newSprite = leftSprite;
        else if (horizontal > 0)
            newSprite = rightSprite;
        else
            newSprite = idleSprite;

        if (newSprite != currentSprite)
        {
            spriteRenderer.sprite = newSprite;
            currentSprite = newSprite;
        }

        // 🦘 JUMP
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // ⚡ DASH
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, overlapSize, groundLayer);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        grounded = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(
            transform.localScale.x * dashingPower * Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical") * transform.localScale.y * dashingPower * 0.5f
        );

        yield return new WaitForSeconds(dashingTime + 0.05f);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        yield return new WaitUntil(() => IsGrounded());

        canDash = true;
    }

    public void DashReset()
    {
        canDash = true;
    }
}