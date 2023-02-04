using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private bool m_isInited;
    public AudioClip clip;
    public float spawnTime;

    public void Init(AudioData data) {
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.loop = false;
        m_AudioSource.volume = Random.Range(data.volume * .8f, data.volume);
        m_AudioSource.pitch = Random.Range(.9f, 1.1f);
        clip = data.clip;
        m_AudioSource.PlayOneShot(data.clip);
        spawnTime = Time.time;
        m_isInited = true;
    }

    private void Update() {
        if(m_AudioSource != null && !m_AudioSource.isPlaying && m_isInited) {
            AudioManager.Instance.audioObjects.Remove(this);
            Destroy(gameObject);
        }
    }
}
