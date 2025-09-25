using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource _bgmSource;
    private AudioSource _sfxSource;
    private Coroutine _bgmFadeCoroutine;
    public float bgmVolume = 0.5f;
    public float sfxVolume = 1f;

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
        _bgmSource.volume = 0;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.volume = sfxVolume;
    }

    public void PlayBgm(AudioClip clip, float fadeDuration = 50.0f)
    {

        if (_bgmSource.clip == clip && _bgmSource.isPlaying)
        {
            return;
        }

        if (_bgmFadeCoroutine != null)
        {
            StopCoroutine(_bgmFadeCoroutine);
        }

        _bgmSource.clip = clip;
        _bgmSource.Play();
        _bgmFadeCoroutine = StartCoroutine(FadeBgmIn(bgmVolume, fadeDuration));
    }
    private IEnumerator FadeBgmIn(float targetVolume, float duration)
    {
        _bgmSource.volume = 0; // 항상 0에서 시작
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            _bgmSource.volume = Mathf.Lerp(0, targetVolume, time / duration);
            yield return null;
        }

        _bgmSource.volume = targetVolume;
    }
    public void PlaySfx(AudioClip clip, float pitch = 1f)
    {
        if (clip == null) return;

        _sfxSource.pitch = pitch;
        _sfxSource.PlayOneShot(clip);
    }

    public void StopBgm()
    {
        _bgmSource.Stop();
        _bgmSource.clip = null;
    }
    public void FadeBgm(float targetVolume, float duration)
    {
        StartCoroutine(FadeCoroutine(_bgmSource, targetVolume, duration));
    }
    private IEnumerator FadeCoroutine(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        source.volume = targetVolume;
    }
    public void StopSfx() // 모든 SFX 중지
    {
        _sfxSource.Stop();
        _sfxSource.clip = null;
    }

    public void OnBgm()
    {
        _bgmSource.mute = false;
    }

    public void OnSfx()
    {
        _sfxSource.mute = false;
    }

    public void OffBgm()
    {
        _bgmSource.mute = true;
    }

    public void OffSfx()
    {
        _sfxSource.mute = true;
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;
        _bgmSource.volume = bgmVolume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        _sfxSource.volume = sfxVolume;
    }
}
