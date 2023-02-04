using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private GameObject m_audioObject;
    public List<AudioObject> audioObjects = new List<AudioObject>();
    private AudioSource bgMusic;
    public float bgMusicVolume = .3f;

    private void Start() {
        bgMusic = GameObject.FindGameObjectWithTag("BGMusic").GetComponent<AudioSource>();
        FadeAudioIn();
    }

    public void PlayAudio(AudioData data) {
        if(data== null) return;
        if(audioObjects.Count > 10 && audioObjects.Where(x => x.clip == data.clip && Time.time - x.spawnTime <= .2f).Count() > 0) {
            return;
        }
        AudioObject obj = Instantiate(m_audioObject).GetComponent<AudioObject>();
        obj.Init(data);
        audioObjects.Add(obj);
    }

    public void FadeAudioIn() {
        StartCoroutine(FadeAudio(1));
    }

    public void FadeAudioOut() {
        StartCoroutine(FadeAudio(-1));
    }

    private IEnumerator FadeAudio(int direction) {

        if(direction == 1) {
            while(bgMusic.volume < bgMusicVolume) {
                bgMusic.volume += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime * direction * 1);
            }
        }
        else if(direction == -1) {
            while (bgMusic.volume > 0) {
                bgMusic.volume += Time.deltaTime * direction;
                yield return new WaitForSeconds(Time.deltaTime * direction * 1);
            }
        }
    }
}
