using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor.Timeline;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
    AudioSource source;

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out source))
        {
            Debug.LogError("Play audio has no audiosource component");
        }
    }

    public void PlayAudioSource()
    {
        if (!source.isPlaying)
        {
            Debug.Log($"Audio {source.clip.name} should play");
            source.Play();
        }
    }

    public void StopAudioSource()
    {
        if (source.isPlaying)
        {
            Debug.Log($"Audio {source.clip.name} should stop");
            source.Stop();
        }
    }

}
