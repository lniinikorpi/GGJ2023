using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public bool isAlive { get; }
    public float currentHealth { get; set; }
    public float maxHealth { get; set; }

    public void TakeDamage(float damage);
    public IEnumerator Die();
}
