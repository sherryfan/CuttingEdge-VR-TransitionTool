﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CESoundManager : MonoBehaviour
{

    public static CESoundManager Singleton;

    // [SerializeField]
    // private AudioSource AmbienceSource, VoiceSource, SFXSource;
    [SerializeField]
    private float transitionTime = 5f;
    [SerializeField]
    private AudioSource ambience1, ambience2;
    public enum TransitionStyle
    {
        Dissolve, EarlyIn
    }
    public TransitionStyle transitionStyle;

    IEnumerator fadeInCoroutine(AudioSource audioSource, float time)
    {
        audioSource.volume = 0f;
        audioSource.Play();
        float elapse = 0;
        while (elapse <= time)
        {
            elapse += Time.deltaTime;
            var newVol = Mathf.Clamp01(elapse / time);
            audioSource.volume = newVol;
            yield return null;
        }
    }
    public void FadeIn(AudioSource audioSource, float time)
    {
        if (audioSource == null)
            return;
        // if (!audioSource.isPlaying)
        // 	return;
        StartCoroutine(fadeInCoroutine(audioSource, time));
    }
    IEnumerator fadeOutCoroutine(AudioSource audioSource, float time)
    {
        float elapse = 0, startVol = audioSource.volume;
        while (elapse <= time)
        {
            elapse += Time.deltaTime;
            var newVol = startVol * Mathf.Clamp01(1 - elapse / time);
            audioSource.volume = newVol;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = 1f;
    }
    public void FadeOut(AudioSource audioSource, float time)
    {
        if (audioSource == null)
            return;
        if (!audioSource.isPlaying)
            return;
        StartCoroutine(fadeOutCoroutine(audioSource, time));
    }

    public void StartTransition()
    {
        switch (transitionStyle)
        {
            case TransitionStyle.Dissolve:
                FadeOut(ambience1, transitionTime);
                FadeIn(ambience2, transitionTime);
                break;
            case TransitionStyle.EarlyIn:
                break;

        }
    }

    public void StopAmbience(){
        ambience2.Stop();
    }


        // IEnumerator playOneShotWithDelayCoroutine(AudioSource audioSource, AudioClip audioClip, 
        // 	float delay, float volumeScale, bool destroy = false) {
        // 	yield return new WaitForSeconds(delay);
        // 	audioSource.PlayOneShot(audioClip, volumeScale);
        // 	if (destroy) {
        // 		yield return new WaitForSeconds(audioClip.length);
        // 		Destroy(audioSource);
        // 	}
        // }
        // public void PlayOneShot(AudioSource audioSource, AudioClip audioClip, float delay = 0f, 
        // 	float volumeScale = 1f) {
        // 	if (audioSource == null || audioClip == null) {
        // 		return;
        // 	}
        // 	StartCoroutine(playOneShotWithDelayCoroutine(audioSource, audioClip, delay, volumeScale));
        // }
        // public void PlayOneShotWithTempAudioSource(AudioClip audioClip, float delay = 0f, 
        // 	float volumeScale = 1f) {
        // 	if (audioClip == null) {
        // 		return;
        // 	}
        // 	AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        // 	StartCoroutine(playOneShotWithDelayCoroutine(audioSource, audioClip, delay, volumeScale, destroy: true));
        // }
        // public void StopBGM() {
        // 	AmbienceSource.clip = null;
        // 	AmbienceSource.Stop();
        // }
        // public void PlayBGM(AudioClip clip, float fadeOut = 0.0f) {
        // 	StartCoroutine(playBGMCoroutine(clip, fadeOut));
        // }
        // IEnumerator playBGMCoroutine(AudioClip clip, float fadeOut) {
        // 	FadeOut(AmbienceSource, fadeOut);
        // 	AudioSource audioSource = AmbienceSource;
        // 	float elapse = 0, startVol = audioSource.volume;
        // 	while (elapse <= fadeOut) {
        // 		elapse += Time.deltaTime;
        // 		var newVol = startVol * Mathf.Clamp01(1 - elapse / fadeOut);
        // 		audioSource.volume = newVol;
        // 		yield return null;
        // 	}
        // 	AmbienceSource.volume = 1f;
        // 	AmbienceSource.clip = clip;
        // 	AmbienceSource.loop = true;
        // 	AmbienceSource.Play();
        // }
        // IEnumerator playVoiceCoroutine(AudioClip clip, float delay, float volumeScale) {
        // 	yield return new WaitForSeconds(delay);
        // 	voiceQ.Enqueue(clip);
        // 	volumeQ.Enqueue(volumeScale);
        // }
        // public void PlayVoice(AudioClip clip, float delay = 0f, float volumeScale = 1f) {
        // 	if (clip == null) {
        // 		return;
        // 	}
        // 	StartCoroutine(playVoiceCoroutine(clip, delay, volumeScale));
        // }
        // public void ClearVoiceQueueAndStop() {
        // 	voiceQ = new Queue<AudioClip>();
        // 	volumeQ = new Queue<float>();
        // 	VoiceSource.Stop();
        // 	VoiceSource.clip = null;
        // }
        // public bool IsVoiceQueueCleared() {
        // 	return (voiceQ.Count == 0) && (!VoiceSource.isPlaying);
        // }
        void Awake()
        {
            //Check if instance already exists
            if (Singleton == null)

                //if not, set instance to this
                Singleton = this;

            //If instance already exists and it's not this:
            else if (Singleton != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

        }
        void Start()
        {
            // BGMSource = GetComponent<AudioSource>();
        }
        Queue<AudioClip> voiceQ = new Queue<AudioClip>();
        Queue<float> volumeQ = new Queue<float>();
        // Update is called once per frame
        bool isPlayingVoice = false;
        void Update()
        {
            // if (VoiceSource != null && !VoiceSource.isPlaying && voiceQ.Count > 0) {
            // 	var a = voiceQ.Dequeue();
            // 	var v = volumeQ.Dequeue();
            // 	VoiceSource.clip = a;
            // 	VoiceSource.PlayOneShot(a, v);
            // }
            // if (VoiceSource != null && VoiceSource.isPlaying) {
            // 	AmbienceSource.volume = 0.3f;
            // }
            // else {
            // 	AmbienceSource.volume = 1.0f;
            // }
        }
    }
