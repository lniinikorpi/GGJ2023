using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private GameObject m_audioObject;
    public List<AudioObject> audioObjects = new List<AudioObject>();

    public void PlayAudio(AudioData data) {
        if(audioObjects.Count > 10 && audioObjects.Where(x => x.clip == data.clip && Time.time - x.spawnTime <= .1f).Count() > 3) {
            return;
        }
        AudioObject obj = Instantiate(m_audioObject).GetComponent<AudioObject>();
        obj.Init(data);
        audioObjects.Add(obj);
    }
}
