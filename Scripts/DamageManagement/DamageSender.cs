using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class DamageSender : MonoBehaviour
{
    [SerializeField] protected float damage = 1;

    public float DoDamage() => damage;
}

