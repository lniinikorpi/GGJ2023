using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float m_movementSpeed;
    private Row m_row;
    private float m_damage;
    private bool m_donedDamage;
    private Timer m_timer;
    private GameObject onHitObject;
    private TurretData data;

    public void Init(TurretData data, float damage, float projectileSpeed, Row row) {
        this.data = data;
        m_movementSpeed = projectileSpeed;
        m_damage = damage;
        m_row = row;
        Instantiate(data.projectile, transform);
        m_timer = new Timer(15);
        onHitObject = data.projectileOnHitEffect;
        GetComponentInChildren<SpriteRenderer>().sortingOrder += row.id * 100 + 49;
    }
    void Update()
    {
        if (GameManager.Instance.isPause) {
            return;
        }
        transform.Translate(Vector2.right * Time.deltaTime * m_movementSpeed);
        if (!m_timer.Update()) {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy == null || enemy.row != m_row) {
            return;
        }
        if (!m_donedDamage) {
            enemy.TakeDamage(m_damage);
            m_donedDamage = true;
        }
        DestroyProjectile();
    }

    private void DestroyProjectile() {
        if (onHitObject != null) {
            Instantiate(onHitObject, transform.position, Quaternion.identity); 
        }
        if(data.projectileOnHitEffect != null) {
            AudioManager.Instance.PlayAudio(data.projectileOnHitData);
        }

        Destroy(gameObject);
    }
}
