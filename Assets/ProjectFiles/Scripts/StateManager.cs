using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuState {
    None,
    Map
}

public static class StateManager
{
    private static int m_completedLevels;
    public static float volume = 1;
    public static bool isMuted;
    public static int completedLevels {
        get {
            return m_completedLevels;
        }
        set {
            if(value < m_completedLevels) {
                return;
            }
            m_completedLevels = value;
            m_completedLevels = Mathf.Clamp(m_completedLevels, 0, 3);
        }
    }
    public static MenuState state;
    public static bool isCongrated = false;
}

