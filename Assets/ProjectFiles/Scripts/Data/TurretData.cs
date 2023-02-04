using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade {
    public int cost;
    public float attackSpeedBuff;
    public float damageBuff;
    public int healthBuff;
}

[CreateAssetMenu(fileName ="TurretData", menuName = "GGJ/TurretData")]
public class TurretData : AttackUnitData {
    public GameObject projectile;
    public float projectileSpeed;
    public int price;
    public Vector2 projectileOffset;

    public List<Upgrade> upgrades;
    public GameObject projectileOnHitEffect;
    public AudioData shootData;
    public AudioData spawnData;
    public AudioData projectileOnHitData;
    public AudioData upgradeData;
    public AudioData onHitAudio;
}
