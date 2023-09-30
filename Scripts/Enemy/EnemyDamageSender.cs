using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageSender : DamageSender
{
    public float attackRange = 1f;
    private Collider2D playercol;
    private EnemyDamageReceiver receiver;


    [SerializeField] private Transform attackPoint;
    [SerializeField] LayerMask playerLayers;

    private void Start()
    {
        damage = 2;
        receiver = GetComponent<EnemyDamageReceiver>();

    }
    private void Update()
    {
        if(EnemyBehavior.isAttacking)
        {
            StartCoroutine(Attack());
        }

    }
    public IEnumerator Attack()
    {
        EnemyBehavior.isAttacking = false;
        yield return new WaitForSeconds(0.3f);
        playercol = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayers);
        if (playercol != null)
        {
            playercol.GetComponentInChildren<PlayerDamageReceiver>().TakeDamage(DoDamage());
        }
        yield return new WaitForSeconds(0.8f);
        playercol = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayers);
        if (playercol != null)
        {
            playercol.GetComponentInChildren<PlayerDamageReceiver>().TakeDamage(DoDamage());
        }
        
    }

}
