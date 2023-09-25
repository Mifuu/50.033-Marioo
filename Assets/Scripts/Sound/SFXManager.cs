using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public static float sfxVolume = 1;
    private static float sfxVolumeFac = 1;

    public Sound[] sounds;
    private List<AudioSource> audioSources;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;

        audioSources = new List<AudioSource>();
    }

    public void Start()
    {
        SetSFXVolume(sfxVolume);
    }

    public void PlaySFX(string name, GameObject target)
    {
        // find sound
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // if can't find sound with the same name
        if (s == null)
        {
            // if 
            if (name.Equals("")) return;
            Debug.Log("SFXManager");
        }

        // ensuring source
        AudioSource source = target.GetComponent<AudioSource>();
        if (source == null)
        {
            source = target.AddComponent<AudioSource>();
            source.volume = sfxVolume * sfxVolumeFac;
            audioSources.Add(source);

            source.maxDistance = 30f;
            source.minDistance = 0f;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.spatialBlend = 1f;
        }

        if (source.enabled) source.PlayOneShot(s.clip, s.volume);
    }

    public void SetSFXVolume(float vol)
    {
        sfxVolume = vol;
        foreach (AudioSource a in audioSources)
        {
            if (a == null)
            {
                audioSources.Remove(a);
                continue;
            }
            else
            {
                a.volume = sfxVolume * sfxVolumeFac;
            }
        }
    }

    public static void TryPlaySFX(string name, GameObject target)
    {
        if (instance != null) instance.PlaySFX(name, target);
    }

    public static void TryPlaySFX(string[] names, GameObject target)
    {
        int i = UnityEngine.Random.Range(0, names.Length);
        if (instance != null) instance.PlaySFX(names[i], target);
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 4f)]
    public float volume = 1;

    [HideInInspector] public AudioSource source;
}
