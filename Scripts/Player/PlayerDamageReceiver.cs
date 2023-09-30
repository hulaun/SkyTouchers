using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageReceiver : DamageReceiver
{
    [SerializeField] private HealthBar healthBar;

    public override void TakeDamage(float damage)
    {
        health-=damage;
        PlayerControler.isHitted = true;
        if (health <= 0)
        {
            PlayerControler.isDead = true;
        }
    }

    private void Awake()
    {
        maxHealth = 100;
        health = 100;
    }
    private void Start()
    {
        healthBar.SetMaxHealth(health);
    }
    private void Update()
    {
        healthBar.SetHealth(health);
    }
}
