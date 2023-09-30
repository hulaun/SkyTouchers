using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimation : PlayerControler
{
    [Header("Unity types")]
    private Animator animator;
    [Header("C# types")]
    public float attackingTime = 0.15f;
    public float attackingSpeed = 0.4f;
    private float jumpAttackingTime = 0.6f;
    private float deathTime = 1f;


    [Header("Animation")]
    string currentAni;
    const string PLAYER_IDLE = "Player_Idle";
    const string PLAYER_RUN = "Player_Run";
    const string PLAYER_JUMP = "Player_Jump";
    const string PLAYER_FALL = "Player_Fall";
    const string PLAYER_DASH = "Player_Dash";
    const string PLAYER_WALLSLIDE = "Player_WallSlide";

    const string PLAYER_ATTACK = "Player_Attack";
    const string PLAYER_JUMPATTACK = "Player_JumpAttack";
    
    const string PLAYER_HIT= "Player_Hit";
    const string PLAYER_DEATH= "Player_Death";
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    } 

    void Update()
    {
        switch (state)
        {
            case State.Normal:
                NormalAnimation();
                AttackingAnimation();
                if (isHitted)
                {
                    Debug.Log("Normal");
                    if (isJumpAttacking)
                    {
                        StopCoroutine(JumpAttack());
                    }
                    if (!canAttack)
                    {
                        StopCoroutine(Attack());
                    }
                }
                break;
            case State.Attacking:
                if (isHitted)
                {
                    Debug.Log("Attacking");
                    if (isJumpAttacking)
                    {
                        StopCoroutine(JumpAttack());
                    }
                    if (!canAttack)
                    {
                        StopCoroutine(Attack());
                    }
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
    private void NormalAnimation()
    {
        if (isHitted)
        {
            ChangeAnimationState(PLAYER_HIT);
        }
        else if (InputManager.Instance.DirX != 0f && IsGrounded() && !isDashing)
        {
            ChangeAnimationState(PLAYER_RUN);
        }
        else if (rb2d.velocity.y > 0.1f)
        {
            ChangeAnimationState(PLAYER_JUMP);
        }
        else if (rb2d.velocity.y < -0.1f && !IsWalled())
        {
            ChangeAnimationState(PLAYER_FALL);
        }
        else if (isDashing)
        {
            ChangeAnimationState(PLAYER_DASH);
        }
        else if (IsWalled() && !IsGrounded())
        {
            ChangeAnimationState(PLAYER_WALLSLIDE);
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
        if (isDead)
        {
            StartCoroutine(Die());
        }
    }

    private void AttackingAnimation()
    {
        if (InputManager.Instance.PlayerMeleeAttack && (rb2d.velocity.y <= -0.1f || rb2d.velocity.y >= 0.1f))
        {
            StartCoroutine(JumpAttack());
        }
        else if (InputManager.Instance.PlayerMeleeAttack && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        state = State.Attacking;
        canAttack=false;
        rb2d.velocity = Vector3.zero;
        ChangeAnimationState(PLAYER_ATTACK);
        yield return new WaitForSeconds(attackingTime);
        state = State.Normal;
        yield return new WaitForSeconds(attackingSpeed);
        canAttack=true;
    }
    private IEnumerator Die()
    {
        state = State.Dead;
        ChangeAnimationState(PLAYER_DEATH);
        rb2d.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(deathTime);
        sR.enabled = false;
    }
    private IEnumerator JumpAttack()
    {
        state = State.Attacking;
        ChangeAnimationState(PLAYER_JUMPATTACK);
        isJumpAttacking = true;
        yield return new WaitForSeconds(jumpAttackingTime);
        state = State.Normal;
    }

    private void ChangeAnimationState(string newAni)
    {
        if (newAni == currentAni)
        {
            return;
        }
        animator.Play(newAni);
        currentAni = newAni;
    }
}
