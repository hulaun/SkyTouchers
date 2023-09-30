using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerDamageSender : DamageSender
{

    public float attackRange = 0.5f;
    public float jumpAttackRange = 0.5f;
    
    [Header("Others")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform jumpAttackPoint;
    [SerializeField] LayerMask enemyLayers;

    private void Start()
    {
        damage = 2;
    }
    private void Update()
    {
        if (InputManager.Instance.PlayerMeleeAttack && !PlayerControler.isJumpAttacking)
        {
            Attack();
        }
        if (InputManager.Instance.PlayerMeleeAttack && PlayerControler.isJumpAttacking)
        {
            StartCoroutine(JumpWhenJumpAttack());
        }
        if (PlayerControler.isHitted&&PlayerControler.isJumpAttacking)
        {
            StopCoroutine(JumpWhenJumpAttack());
        }
    }
    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponentInChildren<EnemyDamageReceiver>().TakeDamage(DoDamage());
        }
        
    }

    private IEnumerator JumpWhenJumpAttack()
    {
        yield return new WaitForSeconds(0.25f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(jumpAttackPoint.position, jumpAttackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            PlayerControler.jumpAttackHit = true;
            Debug.Log("jumphit");
            damage *= 1.5f;
            enemy.GetComponentInChildren<EnemyDamageReceiver>().TakeDamage(DoDamage());
            damage = 2;
        }
        PlayerControler.isJumpAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null || jumpAttackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(jumpAttackPoint.position, jumpAttackRange);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
