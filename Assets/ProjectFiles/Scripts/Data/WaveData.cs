using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveEnemy {
    public EnemyData data;
    public int possibility;
}

[CreateAssetMenu(fileName = "WaveData", menuName = "GGJ/WaveData")]
public class WaveData : ScriptableObject
{
    public int waveLength = 60;
    public float spawnInterval = 1;
    public int maxEnemiesPerSpawn = 1;
    public int startMoney = 150;
    public List<WaveEnemy> enemies = new List<WaveEnemy>();
}
