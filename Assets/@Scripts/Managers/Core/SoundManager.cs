using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource _bgmSource;
    private AudioSource _sfxSource;

    public bool isMuted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayBgm(AudioClip clip) // 새로운 BGM으로 교체
    {
        if (clip == null) return;

        if (_bgmSource.isPlaying)
        {
            _bgmSource.Stop();
        }
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void PlaySfx(AudioClip clip, float pitch = 1f)
    {
        if (clip == null) return;

        _sfxSource.pitch = pitch;
        _sfxSource.PlayOneShot(clip);
    }

    public void StopBgm() // 현재 BGM 중지
    {
        _bgmSource.Stop();
        _bgmSource.clip = null;
    }

    public void StopSfx() // 모든 SFX 중지
    {
        _sfxSource.Stop();
        _sfxSource.clip = null;
    }
}
