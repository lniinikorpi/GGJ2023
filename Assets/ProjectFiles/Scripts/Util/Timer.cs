using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float duration;
    public float timeRemaining;
    public float timeElapsed { get => duration - timeRemaining; }
    public float timeElapsedNormalized { get => timeElapsed / duration; }
    public bool isPlaying { get => timeRemaining > 0 && !m_isPaused; }
    private bool m_isPaused;
    public float timeLeftNormalized
    {
        get => timeRemaining / duration;
    }

    public Timer()
    {
        duration = 0;
        Reset();
        Start();
    }

    public Timer(float duration)
    { 
        this.duration = duration;
        Reset();
        Start();
    }

    public bool Update()
    {
        if (!isPlaying)
        {
            return false;
        }

        timeRemaining -= Time.deltaTime;
        if (!isPlaying)
        {
            Stop();
            return false;
        }
        return true;
    }

    public void Start() {
        m_isPaused = false;
    }

    public void Pause() {
        m_isPaused = true;
    }

    public void Stop()
    {
        timeRemaining = 0;
    }

    public void Reset()
    {
        timeRemaining = duration;
    }

    public void Reset(float value)
    {
        duration = value;
        Reset();
    }
}
