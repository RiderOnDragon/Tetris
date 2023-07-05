using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Sound[] _sounds;
    public void PlayOneShot(string clipName)
    {
        var clip = Array.Find(_sounds, sound => sound.ClipName == clipName);

        if (clip == null)
            throw new Exception("The name of the clip is incorrect");

        _audioSource.PlayOneShot(clip.Clip);
    }

    [Serializable]
    private class Sound
    {
        [SerializeField] private string _clipName;
        [SerializeField] private AudioClip _clip;

        public string ClipName => _clipName;
        public AudioClip Clip => _clip;
    }
}
