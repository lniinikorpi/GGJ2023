using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade {
    public int cost;
    public float attackSpeedBuff;
    public float damageBuff;
}

[CreateAssetMenu(fileName ="TurretData", menuName = "GGJ/TurretData")]
public class TurretData : AttackUnitData {
    public GameObject projectile;
    public float projectileSpeed;
    public int price;

    public List<Upgrade> upgrades;
    public GameObject projectileOnHitEffect;
}
