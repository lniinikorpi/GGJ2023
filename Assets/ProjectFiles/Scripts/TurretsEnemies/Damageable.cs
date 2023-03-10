using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable {
    public bool isAlive { get => currentHealth > 0;}
    public float currentHealth { get; set; }
    public float maxHealth { get; set; }

    public virtual IEnumerator Die() {
        yield return null;
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage) {
        if (!isAlive) {
            return;
        }
        currentHealth -= damage;
        if (isAlive) {
            return;
        }
        currentHealth = 0;
        StartCoroutine(Die());
    }
}
