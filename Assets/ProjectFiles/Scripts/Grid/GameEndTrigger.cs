using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        GameManager.Instance.EndGame(false);
    }
}
