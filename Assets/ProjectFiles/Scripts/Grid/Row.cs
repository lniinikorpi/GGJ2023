using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [SerializeField] private List<Cell> m_cells = new List<Cell>();
    public Transform enemySpawn;
    public int id;

    private void Awake() {
        id = transform.GetSiblingIndex();
    }
}
