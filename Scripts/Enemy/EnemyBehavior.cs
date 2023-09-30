using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBehavior : MonoBehaviour
{
    public float spawnTime = 15.0f;
    public static float currentTime = 0;
    public float followDistance = 10.0f;
    public float spawnDistance = 12.0f;
    public float attackDistance = 1.0f;
    public float knockbackForce = 10f;
    public float attackSpeed=0.6f;
    public bool isSpawned;
    public bool isFacingRight = false;
    public static bool isAttacking = false;
    public bool canAttack;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform spawnAndFollowPoint;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;
    private Vector2 playerPosition;
    private Animator animator;
    private Rigidbody2D rb2d;
    private EnemyDamageReceiver enemydmgrec;
    private EnemyDamageSender enemydmgsen;
    private SpriteRenderer sR;

    string currentAni;
    const string SPAWN = "spawn";
    const string LYING = "lying";
    const string IDLE = "idle";
    const string RUN = "run";
    const string ATTACK = "attack";
    const string HIT = "hit";
    const string DIE = "die";

    public enum State
    {
        Spawn,
        Normal,
        Attacking,
        Attacked,
        Died
    }
    public State state;
    private void Awake()
    {
        canAttack = true;
        isSpawned = false;
        state=State.Spawn;
    }
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemydmgrec = GetComponentInChildren<EnemyDamageReceiver>();
        enemydmgsen = GetComponentInChildren<EnemyDamageSender>();
        sR = GetComponent<SpriteRenderer>();
        ChangeAnimationState(LYING);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Spawn:
                if (Physics2D.OverlapCircle(spawnAndFollowPoint.position, spawnDistance, playerLayer))
                {
                    StartCoroutine(Spawning());
                    isSpawned=true;
                    canAttack = true;
                }

                break;
            case State.Normal:
                playerPosition = playerTransform.position;

                if (transform.position.x - playerPosition.x > 0 && isFacingRight)
                {
                    Flip();
                    isFacingRight = false;
                }
                if (transform.position.x - playerPosition.x < 0 && !isFacingRight)
                {
                    Flip();
                    isFacingRight = true;
                }
                // If the player is within the follow distance, start following them.
                if (!Physics2D.OverlapCircle(attackPoint.position, attackDistance, playerLayer) && Physics2D.OverlapCircle(spawnAndFollowPoint.position, followDistance, playerLayer))
                {
                    if (isFacingRight)
                    {
                        rb2d.velocity = new Vector2(2.0f,transform.position.y);
                    }else
                    {
                        rb2d.velocity = new Vector2(-2.0f,transform.position.y);
                    }
                    ChangeAnimationState(RUN);
                }
                else if (Physics2D.OverlapCircle(attackPoint.position, attackDistance, playerLayer) && canAttack)
                {
                    StartCoroutine(Attack());
                }
                else
                {
                    ChangeAnimationState(IDLE);
                }

                if (enemydmgrec.isHitted)
                {
                    StartCoroutine(Hitted());
                }
                if (enemydmgrec.isDied)
                {
                    StopAllCoroutines();
                    StartCoroutine(Died());
                }
                break;
            case State.Attacking:
                if (enemydmgrec.isHitted)
                {
                    StopAllCoroutines();
                    StartCoroutine(Hitted());
                    canAttack= true;
                    isAttacking = false;
                }
                break;
            case State.Attacked:
                break;
            case State.Died:
                isSpawned = false;
                currentTime += Time.deltaTime;
                if(currentTime >= spawnTime)
                {
                    state = State.Spawn;
                    sR.enabled=true;
                    enemydmgrec.isDied = false;
                    enemydmgrec.Spawned();
                    ChangeAnimationState(LYING);
                    currentTime = 0;
                }
                break;  
        }
        
    }
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
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
    private IEnumerator Attack()
    {
        canAttack = false;
        state = State.Attacking;
        rb2d.velocity = Vector3.zero;
        ChangeAnimationState(ATTACK);
        isAttacking= true;
        yield return new WaitForSeconds(1.4f);
        state = State.Normal;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }
    private IEnumerator Hitted()
    {
        state = State.Attacked;
        enemydmgrec.isHitted = false;
        rb2d.velocity = Vector3.zero;
        ChangeAnimationState(HIT);
        if (!isFacingRight)
        {
            rb2d.AddForce(new Vector2(knockbackForce, 0f), ForceMode2D.Impulse);
        }
        else
        {
            rb2d.AddForce(new Vector2(-knockbackForce, 0f), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.7f);
        state = State.Normal;
    }
    private IEnumerator Spawning()
    {
        ChangeAnimationState(SPAWN);
        yield return new WaitForSeconds(1.1f);
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        state = State.Normal;
    }
    private IEnumerator Died()
    {

        state = State.Died;
        rb2d.bodyType = RigidbodyType2D.Static;
        ChangeAnimationState(DIE);
        yield return new WaitForSeconds(0.7f);
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null || spawnAndFollowPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(spawnAndFollowPoint.position, spawnDistance);
        Gizmos.DrawWireSphere(spawnAndFollowPoint.position, followDistance);
        Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }
}
