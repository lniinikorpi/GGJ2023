using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{
    [HideInInspector]
    public Row row;
    private EnemyData m_data;
    private bool m_isInited = false;
    [HideInInspector]
    public Turret nearestTurret;
    private Timer m_attackTimer;
    [SerializeField]
    private Transform m_graphicsParent;
    private Animator anim;
    private bool m_isAttacking = false;
    public void Init(EnemyData data, Row row) {
        this.row = row;
        m_data = data;
        maxHealth = m_data.health;
        currentHealth = m_data.health;
        m_attackTimer = new Timer(1 / m_data.attackSpeed);
        var obj = Instantiate(m_data.graphicObject, m_graphicsParent);
        obj.transform.position += (Vector3)m_data.graphicsOffsett;
        SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>();
        int randomSortId = Random.Range(-49, 49);
        for (int i = 0; i < renderers.Length; i++) {
            renderers[i].sortingOrder += row.id * 100 + randomSortId;
        }
        anim = GetComponentInChildren<Animator>();
        GridManager.Instance.enemies.Add(this);
        m_isInited = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isInited || GameManager.Instance.isPause) {
            return;
        }
        if(nearestTurret != null) {
            if (transform.position.x - nearestTurret.transform.position.x < .5f) {
                anim.SetBool("IsWalking", false);
                if (m_attackTimer != null && !m_attackTimer.Update()) {
                    StartCoroutine(HandleAttack());
                    m_attackTimer.Reset(Random.Range(m_attackTimer.duration * .9f, m_attackTimer.duration * 1.1f));
                }
                return;
            }
        }
        else {
            nearestTurret = GridManager.Instance.GetNearestTurret(row, transform.position.x);
        }
        HandleMovement();
    }

    private void HandleMovement() {
        if (m_isAttacking) {
            return;
        }
        anim.SetBool("IsWalking", true);
        transform.Translate(Vector2.left * Time.deltaTime * m_data.movementSpeed);
    }

    private IEnumerator HandleAttack() {
        m_isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(m_data.attackDelay);
        if(nearestTurret != null) {
            nearestTurret.TakeDamage(m_data.damage);
        }
        m_isAttacking = false;
    }

    public override void Die() {
        StopAllCoroutines();
        int rewards = (int)Random.Range((float)m_data.currencyEarned * .8f, (float)m_data.currencyEarned * 1.2f);
        GameManager.Instance.AddCurrency(rewards);
        GridManager.Instance.RemoveEnemy(this);
        base.Die();
    }

    private void OnTurretSpawned(EventArgs args) {
        if(args.row != row) {
            return;
        }
        nearestTurret = GridManager.Instance.GetNearestTurret(row, transform.position.x);
    }

    private void OnEnable() {
        GlobalEventSender.TurretSpawnedEvent += OnTurretSpawned;

    }

    private void OnDisable() {
        GlobalEventSender.TurretSpawnedEvent -= OnTurretSpawned;
    }
}
