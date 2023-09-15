using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource[] audioSources;
    AudioSource currentSource;

    [SerializeField] AudioClip levelMusic;

    void Awake()
    {
        int numAudioPlayers = FindObjectsOfType(typeof(AudioPlayer)).Length;

        if (numAudioPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        audioSources = GetComponentsInChildren<AudioSource>();
        currentSource = audioSources[0];
    }

    private void Start()
    {
        PlayAudioLoop(levelMusic, 0.2f, "Channel1(Music)");
    }

    public bool IsCurrentlyPlaying(string channel)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.name == channel)
            {
                currentSource = source;
            }
        }

        return currentSource.isPlaying;
    }

    public void PlaySFXClipOnce(AudioClip clip, float volume)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.name.Contains("SFX") && !source.isPlaying)
            {
                currentSource = source;
                break;
            }
        }
        currentSource.loop = false;
        currentSource.volume = volume;
        currentSource.PlayOneShot(clip);

        Debug.Log(currentSource.name + " is playing " + clip.name);
    }

    public void PlayClipOnce(AudioClip clip, float volume, string channel)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.name == channel)
            {
                currentSource = source;
            }
        }
        currentSource.loop = false;
        currentSource.volume = volume;
        currentSource.PlayOneShot(clip);
    }

    public void PlayAudioLoop(AudioClip clip, float volume, string channel)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.name == channel)
            {
                currentSource = source;
            }
        }
        currentSource.volume = volume;
        currentSource.clip = clip;
        currentSource.loop = true;
        currentSource.PlayDelayed(1f);
    }

    public void StopAudio(string channel)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.name == channel)
            {
                currentSource = source;
            }
        }
        currentSource.loop = false;
        currentSource.Stop();
    }
}
