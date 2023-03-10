using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject turretPrefab;
    public GameObject projectileBase;
    public List<TurretData> availableTurrest = new List<TurretData>();
    public int currency = 0;
    public bool isPause;
    public AudioData victoryData;
    public AudioData loseData;
    private void Start() {
        AddCurrency(WaveManager.Instance.waveData.startMoney);
    }

    public void AddCurrency(int amount) {
        currency += amount;
        GlobalEventSender.SendCurrencyChanged(new EventArgs() {
            intValue = currency
        });
    }

    public void EndGame(bool playerWon) {
        AudioManager.Instance.FadeAudioOut();
        if (playerWon) {
            AudioManager.Instance.PlayAudio(victoryData);
            StateManager.completedLevels = SceneLoader.Instance.GetSceneIndex();
        }
        else {
            AudioManager.Instance.PlayAudio(loseData);
            GlobalEventSender.SendPause(new EventArgs());
        }
        GlobalEventSender.SendGameEnd(new EventArgs() {
            boolValue = playerWon
        });
    }

    private void OnGamePause(EventArgs args) {
        isPause = true;
    }

    private void OnGameContinue(EventArgs args) {
        isPause = false;
    }

    private void OnEnable() {
        GlobalEventSender.ContinueEvent += OnGameContinue;
        GlobalEventSender.PauseEvent += OnGamePause;
    }

    private void OnDisable() {
        GlobalEventSender.ContinueEvent += OnGameContinue;
        GlobalEventSender.PauseEvent += OnGamePause;
    }
}
