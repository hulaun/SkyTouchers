using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerControler : MonoBehaviour
{
    [Header("Unity types")]
    protected static Rigidbody2D rb2d;
    protected static SpriteRenderer sR;
    protected static BoxCollider2D bc2d;
    protected PlayerDamageReceiver playerDmgRec;
    [SerializeField] protected LayerMask ground;
    [SerializeField] protected TrailRenderer trail;
    [SerializeField] protected LayerMask wallLayer;

    [Header("Condition")]
    protected static bool canDash = true;
    protected static bool canAttack = true;
    protected static bool isDashing = false;
    protected static bool isFacingRight = true;
    protected static bool isWallSliding;
    public static bool isJumpAttacking=false;
    public static bool jumpAttackHit;
    public static bool isHitted;
    public static bool isDead;

    


    protected enum State
    {
        Normal,
        Attacking,
        Attacked,
        Dead
    }
    protected static State state;


    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        playerDmgRec= GetComponentInChildren<PlayerDamageReceiver>();
    }

    protected bool IsGrounded()
    {

        return Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.down, 0.1f, ground);
    }

    protected bool IsWalled()
    {
        if (isFacingRight)
        {
            return Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.right, 0.1f, wallLayer);
        }
        else
        {
            return Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.left, 0.1f, wallLayer);
        }
    }

    protected bool IsInvisibleWalled()
    {
        if (isFacingRight)
        {
            return Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.right, 0.1f, wallLayer);
        }
        else
        {
            return Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.left, 0.1f, wallLayer);
        }
    }



}
