using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "GGJ/AudioData")]
public class AudioData : ScriptableObject
{
    public float volume = 1;
    public AudioClip clip;
}
