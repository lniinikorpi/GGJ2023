using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUnitData : ScriptableObject
{
    public float attackSpeed;
    public float damage;
    public float health;
    public GameObject graphicObject;
    public Sprite icon;
    public float attackDelay = .4f;
    public Vector2 graphicsOffsett = Vector2.zero;
}
