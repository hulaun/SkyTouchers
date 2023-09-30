using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMovement : PlayerControler
{
    private short extraJumps;
    public float playerSpeed = 6f;
    public float jumpPower = 20f;
    public short extraJumpValue = 1;
    public float dashingPower = 16f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    public float wallSlidingSpeed = 2f;
    public float knockbackForce = 10f;

    void Update()
    {
        switch (state)
        {
            case State.Normal:
                Movement();


                
                
                break;
            case State.Attacking:
                if (InputManager.Instance.Dash && canDash && isJumpAttacking)
                {
                    StartCoroutine(Dash());
                }
                if (isHitted && isDashing)
                {
                    StopCoroutine(Dash());
                    StartCoroutine(Hitted());
                }
                break;
            case State.Attacked:
                
                break;
            case State.Dead: 
                
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }

    private void Movement()
    {

        if (InputManager.Instance.DirX > 0.001f && !isDashing && !isWallSliding)
        {
            if (!isFacingRight)
            {
                Flip();
                isFacingRight = true;
            }

            rb2d.velocity = new Vector2(InputManager.Instance.DirX * playerSpeed, rb2d.velocity.y);
        }
        else if (InputManager.Instance.DirX < -0.001f && !isDashing && !isWallSliding)
        {
            if (isFacingRight)
            {
                Flip();
                isFacingRight = false;
            }
            rb2d.velocity = new Vector2(InputManager.Instance.DirX * playerSpeed, rb2d.velocity.y);
        }

        if (InputManager.Instance.JumpInput && extraJumps > 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
            extraJumps--;
        }
        else if (InputManager.Instance.JumpInput && extraJumps == 0 && IsGrounded())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
        }

        if (InputManager.Instance.Dash && canDash)
        {
            StartCoroutine(Dash());
        }

        if (jumpAttackHit)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower * 0.6f);
            jumpAttackHit = false;
        }
        if (isHitted)
        {
            StartCoroutine(Hitted());
        }
        if (IsGrounded())
        {
            extraJumps = extraJumpValue;
        }
        WallSlide();
        InvisibleWallFall();
    }


    

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && InputManager.Instance.DirX != 0)
        {

            isWallSliding = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void InvisibleWallFall()
    {
        if (IsInvisibleWalled() && !IsGrounded() && InputManager.Instance.DirX != 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }
    private IEnumerator Hitted()
    {
        state = State.Attacked;
        rb2d.velocity = Vector3.zero;
        if (!isFacingRight)
        {
            rb2d.AddForce(new Vector2(knockbackForce*Time.deltaTime, 0f), ForceMode2D.Impulse);
        }
        else
        {
            rb2d.AddForce(new Vector2(-knockbackForce * Time.deltaTime, 0f), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.7f);
        isHitted = false;
        state = State.Normal;
    }
    protected IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0f;
        rb2d.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        rb2d.velocity = new Vector2(0.05f, 0f);
        trail.emitting = false;
        rb2d.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
