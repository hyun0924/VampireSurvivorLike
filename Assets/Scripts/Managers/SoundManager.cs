using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.AudioSource.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = Util.GetOrCreateGameObject("@Root_Sound");
        GameObject.DontDestroyOnLoad(root);

        for (int i = 0; i < _audioSources.Length; i++)
        {
            GameObject go = new GameObject { name = Enum.GetName(typeof(Define.AudioSource), i) };
            go.transform.SetParent(root.transform);
            _audioSources[i] = go.GetOrAddComponent<AudioSource>();
        }
    }

    public bool IsPlaying(Define.AudioSource type) { return _audioSources[(int)type].isPlaying; }

    public void Play(AudioClip audioClip, Define.AudioSource type = Define.AudioSource.SFX, float volume = 1)
    {
        AudioSource audioSource = _audioSources[(int)type];
        switch (type)
        {
            case Define.AudioSource.BGM:
                {
                    if (audioSource.isPlaying) audioSource.Stop();

                    audioSource.loop = true;
                    audioSource.clip = audioClip;
                    audioSource.volume = volume;
                    audioSource.Play();
                }
                break;
            case Define.AudioSource.SFX:
                {
                    audioSource.loop = false;
                    audioSource.volume = volume;
                    audioSource.PlayOneShot(audioClip);
                }
                break;
        }
    }

    public void Play(string name, Define.AudioSource type = Define.AudioSource.SFX, float volume = 1)
    {
        AudioClip audioClip = GetOrAddAudioClip(name);
        Play(audioClip, type, volume);
    }

    public void Play(Define.Audio audioType, Define.AudioSource type = Define.AudioSource.SFX, float volume = 1)
    {
        string name = Enum.GetName(typeof(Define.Audio), audioType);
        Play(name, type, volume);
    }

    private AudioClip GetOrAddAudioClip(string name)
    {
        if (!name.Contains("Audio/"))
            name = "Audio/" + name;

        AudioClip audioClip = null;
        if (_audioClips.ContainsKey(name))
            audioClip = _audioClips[name];
        else
        {
            audioClip = Managers.Resource.Load<AudioClip>(name);
            _audioClips.Add(name, audioClip);
        }

        return audioClip;
    }

    public void SetVolume(float volume, Define.AudioSource type)
    {
        _audioSources[(int)type].volume = volume;
    }

    public void Clear()
    {
        foreach (var source in _audioSources)
        {
            source.Stop();
            source.clip = null;
        }
        _audioClips = null;
    }
}