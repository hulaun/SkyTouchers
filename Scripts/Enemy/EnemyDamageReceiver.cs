using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageReceiver : DamageReceiver
{
    public bool isHitted = false;
    public bool isDied = false;

    public override void TakeDamage(float damage)
    {
        health -= damage;
        isHitted = true;

        if(health <= 0)
        {
            isDied = true;
        }
            
    }
    public void Spawned() => health = 50;
}
