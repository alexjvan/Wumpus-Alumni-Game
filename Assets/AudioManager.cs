using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEditor.SceneManagement;

public class AudioManager : MonoBehaviour {

    public Sound[] backgroundSounds;
    public AudioMixerGroup BackGroundMixer;
    public static AudioManager instance;
    // Use this for initialization
    void Awake() {

        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in backgroundSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = BackGroundMixer;
        }
    }

    void Start()
    {
        Play("TitleTheme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(backgroundSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void ChangeSong()
    {
        backgroundSounds[0].source.Stop();
        Play("OverWorld");
    }

}