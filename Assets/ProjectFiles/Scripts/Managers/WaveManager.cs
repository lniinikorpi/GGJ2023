using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    public WaveData waveData;
    public GameObject enemyPrefab;
    [HideInInspector]
    public bool isSpawning = false;
    private Timer m_waveTimer;

    private void Update() {
        if (GameManager.Instance.isPause) {
            return;
        }
        if (isSpawning) {
            if(m_waveTimer != null && !m_waveTimer.Update()) {
                isSpawning = false;
                GlobalEventSender.SendWaveTimerEnd(new EventArgs());
            }
        }
    }

    private void Start() {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies() {
        float time = 0;
        while (time < 1) {
            while (GameManager.Instance.isPause) {
                yield return null;
            }
            yield return null;
            time += Time.deltaTime;
        }
        m_waveTimer = new Timer(waveData.waveLength);
        isSpawning = true;
        while (isSpawning) {
            while (GameManager.Instance.isPause) {
                yield return null;
            }
            float waitTime = Random.Range(waveData.spawnInterval - (waveData.spawnInterval * .5f * m_waveTimer.timeElapsedNormalized), waveData.spawnInterval * 1.5f);
            int enemiesToSpawn = Random.Range(waveData.maxEnemiesPerSpawn - 2 , Mathf.RoundToInt(waveData.maxEnemiesPerSpawn + 2 * m_waveTimer.timeElapsedNormalized));
            enemiesToSpawn = Mathf.Clamp(enemiesToSpawn, 1, GridManager.Instance.rows.Count);
            List<Row> rows = new List<Row>(GridManager.Instance.rows);
            rows.Shuffle();
            for (int i = 0; i < enemiesToSpawn; i++) {
                SpawnEnemy(rows[i]);
            }
            time = 0;
            while (time < waitTime) {
                yield return null;
                time += Time.deltaTime;
            }

        }

    }

    public void SpawnEnemy(Row row) {
        List<EnemyData> enemies = new List<EnemyData>();
        for (int i = 0; i < waveData.enemies.Count; i++) {
            for (int y = 0; y < waveData.enemies[i].possibility; y++) {
                enemies.Add(waveData.enemies[i].data);
            }
        }
        Vector2 spawnPos = row.enemySpawn.position;
        Enemy enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity).GetComponent<Enemy>();
        enemy.Init(enemies[Random.Range(0, enemies.Count)], row);
    }
}
