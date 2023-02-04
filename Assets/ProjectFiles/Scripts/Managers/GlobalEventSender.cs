using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEventSender
{
    public delegate void CurrencyEvent(EventArgs args);

    public static event CurrencyEvent CurrencyChangedEvent;

    public static void SendCurrencyChanged(EventArgs args) {
        CurrencyChangedEvent?.Invoke(args);
    }

    public delegate void TurretEvent(EventArgs args);
    public static event TurretEvent TurretSpawnedEvent;

    public static void SendTurretSpawnedEvent(EventArgs args) {
        TurretSpawnedEvent?.Invoke(args);
    }

    public delegate void GameEvent(EventArgs args);
    public static event GameEvent PauseEvent;
    public static event GameEvent ContinueEvent;
    public static event GameEvent GameEndEvent;
    public static event GameEvent WaveTimerEndEvent;

    public static void SendPause(EventArgs args) {
        PauseEvent?.Invoke(args);
    }
    public static void SendContinue(EventArgs args) {
        ContinueEvent?.Invoke(args);
    }
    public static void SendGameEnd(EventArgs args) {
        GameEndEvent?.Invoke(args);
    }

    public static void SendWaveTimerEnd(EventArgs args) {
        WaveTimerEndEvent?.Invoke(args);
    }
}
