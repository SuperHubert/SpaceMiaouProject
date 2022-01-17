using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds = new List<Sound>();
    public static float volumeMultiplier;

    public static AudioManager Instance;

    private void Awake()
    {
        #region Singleton Don't Destroy On Load
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string id)
    {
        var s = Array.Find<Sound>(sounds.ToArray(), sound => sound.name == id);
        s?.source.Play();
    }

    public void Play(int id,bool dontCutPrevious = false)
    {
        if (id >= sounds.Count || id < 0) return;
        if(sounds[id].source.isPlaying && dontCutPrevious) return;
        sounds[id].source.Play();
    }

    public void Stop(int id, bool fade = false)
    {
        if (id >= sounds.Count || id < 0) return;
        if (fade)
        {
            sounds[id].source.DOFade(0f, 3);
        }
        else
        {
            sounds[id].source.Stop();
        }
    }
    
    public void StopAllSounds()
    {
        foreach (var sound in sounds.Where(sound => sound.source.isPlaying))
        {
            sound.source.Stop();
        }
    }

    public void ChangeVolume(float input)
    {
        if (input > 1) input = 1;
        if (input < 0) input = 0;
        volumeMultiplier = input;

        foreach (var sound in sounds)
        {
            sound.source.volume = sound.volume * volumeMultiplier;
        }
    }

}
