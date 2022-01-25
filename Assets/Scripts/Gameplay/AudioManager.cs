using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class AudioManager : SingletonP<AudioManager>
{
    public AudioClip[] audioList;
    Dictionary<string, AudioClip> audioMap;

    private void Start() {
        DontDestroyOnLoad(transform.gameObject);
    }
    protected override void Awake()
    {
        base.Awake();

        audioMap = new Dictionary<string, AudioClip>();

        foreach (AudioClip a in audioList) {
            audioMap.Add(a.name, a);
        }
    }

    public AudioClip GetAudioClip(string key) {
        return audioMap[key];
    }
}
