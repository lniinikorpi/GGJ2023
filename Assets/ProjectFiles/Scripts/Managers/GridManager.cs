using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : Singleton<GridManager>
{
    private Camera m_camera;
    private Vector2 m_mousePosition;
    public List<Row> rows = new List<Row>();
    public TurretData turretToPlace;
    [HideInInspector]
    public Turret selectedTurret;
    [HideInInspector]
    public List<Turret> turrets = new List<Turret>();
    [HideInInspector]
    public List<Enemy> enemies = new List<Enemy>();
    private void Start() {
        m_camera = Camera.main;
    }

    public void HandleMouseMove(Vector2 pos) {
        m_mousePosition = m_camera.ScreenToWorldPoint(new Vector3(pos.x, pos.y, -m_camera.transform.position.z));
        RaycastHit2D hit = Physics2D.Raycast(m_mousePosition, Vector2.down);
        if (!hit.transform) {
            return;
        }
        Cell cell = hit.collider.gameObject.GetComponent<Cell>();
        if(cell == null) {
            return;
        }
    }

    public void HandleMouseClick() {
        RaycastHit2D hit = Physics2D.Raycast(m_mousePosition, new Vector3(0,0,1));
        UnSelectTurret();
        if (!hit.transform) {
            return;
        }
        Cell cell = hit.collider.gameObject.GetComponent<Cell>();
        if (cell == null) {
            return;
        }
        if(cell.spawnedTurret != null) {
            SelectTurret(cell);
            return;
        }
        if(turretToPlace == null) {
            return;
        }
        SpawnTurret(cell);
    }

    public void HandleMouseSecondaryClick() {
        UnSelectTurret();
        UIManager.Instance.UnToggleAll();
    }

    public void SpawnTurret(Cell cell) {
        Turret turret = Instantiate(GameManager.Instance.turretPrefab, cell.transform).GetComponent<Turret>();
        turret.transform.localPosition = Vector3.zero;
        turret.Init(turretToPlace, cell.row);
        cell.spawnedTurret = turret;
        GameManager.Instance.AddCurrency(-turretToPlace.price);
    }

    private void SelectTurret(Cell cell) {
        UIManager.Instance.UnToggleAll();
        selectedTurret = cell.spawnedTurret;
        selectedTurret.m_upgradeButton.gameObject.SetActive(true);
        //selectedTurret.m_levelImageParent.gameObject.SetActive(true);
    }

    public void UnSelectTurret() {
        if (selectedTurret != null) {
            selectedTurret.m_upgradeButton.gameObject.SetActive(false);
            //selectedTurret.m_levelImageParent.gameObject.SetActive(false);
        }
        selectedTurret = null;
    }

    public Turret GetNearestTurret(Row row, float position) {
        List<Turret> rowTurrets = turrets.Where(turret => turret.row == row && turret.transform.position.x < position).ToList();
        if(rowTurrets.Count == 0) {
            return null;
        }
        Turret nearestTurret = rowTurrets[0];
        foreach (var turret in rowTurrets) {
            if(turret == nearestTurret) {
                continue;
            }
            if(position - turret.transform.position.x < position - nearestTurret.transform.position.x) {
                nearestTurret = turret;
            }
        }
        return nearestTurret;
    }

    public void RemoveEnemy(Enemy enemy) {
        enemies.Remove(enemy);
        if(enemies.Count > 0 || WaveManager.Instance.isSpawning) {
            return;
        }
        GameManager.Instance.EndGame(true);
    }

    public bool IsEnemiesInRow(Row row, float position) {
        return enemies.Where(enemy => enemy.row == row && enemy.transform.position.x > position).Any();
    }

    private void OnWaveTimerEnd(EventArgs args) {
        if(enemies.Count > 0) {
            return;
        }

        GameManager.Instance.EndGame(true);
    }

    private void OnEnable() {
        GlobalEventSender.WaveTimerEndEvent += OnWaveTimerEnd;
    }

    private void OnDisable() {
        GlobalEventSender.WaveTimerEndEvent -= OnWaveTimerEnd;
    }
}
