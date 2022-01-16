using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds = new List<Sound>();

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

    public void Play(int id)
    {
        if (id >= sounds.Count || id < 0) return;
        //if(sounds[id].source.isPlaying) return;
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

}
