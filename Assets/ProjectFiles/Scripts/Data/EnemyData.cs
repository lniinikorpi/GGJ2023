using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "GGJ/EnemyData")]
public class EnemyData : AttackUnitData
{
    public float movementSpeed = 1.5f;
    public int currencyEarned = 10;
    public AudioData attackAudio;
}
