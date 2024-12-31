using Sirenix.Serialization;
using System;
using UnityEngine;

public interface IDamageable 
{
    [SerializeField]
    public float maxHealth { get; set; }
    [SerializeField]
    public float currentHealth { get; set;  }
    public void onGetDamage(float damage);
    public void onDead();
     
}
