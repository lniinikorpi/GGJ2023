using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Turret spawnedTurret;
    public Row row;

    private void Awake() {
        row = GetComponentInParent<Row>();
    }

    public void HighLightCell() {

    }
}
