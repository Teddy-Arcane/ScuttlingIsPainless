using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    public AudioMixer mixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[0];
        }
    }

    public void PlaySound(string name)
    {
        var sound = Array.Find(sounds, sound => sound.name == name);
        sound.source.Play();
    }

    public void StopSound(string name)
    {
        var sound = Array.Find(sounds, sound => sound.name == name);
        sound.source.Stop();
    }

    public Sound GetSound(string name)
    {
        var sound = Array.Find(sounds, sound => sound.name == name);
        return sound;
    }

    public IEnumerator StartFade(string name, float duration, float targetVolume)
    {
        var audioSource = GetSound(name)?.source;
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}