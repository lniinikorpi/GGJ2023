using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : Damageable
{
    private Timer m_attackTimer;
    private TurretData m_data;
    [HideInInspector]
    public Row row;
    private bool m_isInited;
    public Transform m_levelImageParent;
    [SerializeField]
    private GameObject m_levelImagePrefab;
    public Button m_upgradeButton;
    private int m_currentLevel = 0;
    private float m_damage;
    private Animator m_anim;
    public int Price { get => m_data.price; }

    void Update()
    {
        if (!m_isInited || GameManager.Instance.isPause) {
            return;
        }
        if(m_data.attackSpeed <= 0) {
            return; 
        }
        if(m_attackTimer == null || m_attackTimer.Update()) {
            return;
        }
        StartCoroutine(Attack());
    }

    public void Init(TurretData data, Row row) {
        m_data = data;
        Instantiate(data.graphicObject, transform);
        this.row = row;
        maxHealth = m_data.health;
        currentHealth = m_data.health;
        m_attackTimer = new Timer(1 / m_data.attackSpeed);
        m_attackTimer.Start();
        m_upgradeButton.onClick.AddListener(UpgradeTurret);
        m_damage = m_data.damage;
        m_upgradeButton.gameObject.SetActive(false);
        m_anim = GetComponentInChildren<Animator>();
        GridManager.Instance.turrets.Add(this);
        if(m_data.spawnData != null) {
            AudioManager.Instance.PlayAudio(m_data.spawnData);
        }
        m_isInited = true;
        GlobalEventSender.SendTurretSpawnedEvent(new EventArgs() {
            turret = this,
            row = row
        });
    }

    private IEnumerator Attack() {
        m_attackTimer.Reset(Random.Range(m_attackTimer.duration * .9f, m_attackTimer.duration * 1.1f));
        if (!GridManager.Instance.IsEnemiesInRow(row, transform.position.x)) {
            yield break;
        }
        m_anim.SetTrigger("Attack");
        yield return new WaitForSeconds(m_data.attackDelay);
        if (m_data.shootData != null) {
            AudioManager.Instance.PlayAudio(m_data.shootData);
        }
        Instantiate(GameManager.Instance.projectileBase, transform.position, Quaternion.identity).GetComponent<Projectile>().Init(m_data, m_damage, m_data.projectileSpeed, row);
    }

    private void UpgradeTurret() {
        GameManager.Instance.AddCurrency(-m_data.upgrades[m_currentLevel].cost);
        Upgrade upgrade = m_data.upgrades[m_currentLevel];
        m_attackTimer.duration *= upgrade.attackSpeedBuff / 100;
        m_damage *= 1f + upgrade.damageBuff / 100;
        m_currentLevel++;
        GameManager.Instance.AddCurrency(0);
        Instantiate(m_levelImagePrefab, m_levelImageParent);
    }

    public override IEnumerator Die() {
        yield return new WaitForSeconds(m_data.dieDelay);
        GridManager.Instance.turrets.Remove(this);
        if(GridManager.Instance.selectedTurret == this) {
            GridManager.Instance.UnSelectTurret();
        }
        m_anim.SetTrigger("Die");
        StartCoroutine(base.Die());
    }

    private void OnCurrencyChanged(EventArgs args) {
        m_upgradeButton.interactable = m_currentLevel < m_data.upgrades.Count && args.intValue >= m_data.upgrades[m_currentLevel].cost;
    }

    private void OnEnable() {
        GlobalEventSender.CurrencyChangedEvent += OnCurrencyChanged;
    }

    private void OnDisable() {
        GlobalEventSender.CurrencyChangedEvent -= OnCurrencyChanged;
    }
}
