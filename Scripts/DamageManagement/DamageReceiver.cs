using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageReceiver : MonoBehaviour
{
    protected float maxHealth;
    [SerializeField]protected float health;

    public abstract void TakeDamage(float damage);
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageSender damageSender = collision.transform.GetComponent<DamageSender>();
        if(damageSender != null )
        {
            TakeDamage(damageSender.DoDamage());
        }
    }
}
