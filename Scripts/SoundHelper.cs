using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHelper : MonoBehaviour {
	public static SoundHelper Singleton;
	[SerializeField]
	AudioSource BGMSource, VoiceSource;
	Dictionary<string, AudioSource> audioSourceDic = new Dictionary<string, AudioSource>();
	public AudioSource GetAudioSourceByName(string name) {
		if (!audioSourceDic.ContainsKey(name)) {
			audioSourceDic.Add(name, gameObject.AddComponent<AudioSource>());
		}
		return audioSourceDic[name];
	}
	IEnumerator fadeOutCoroutine(AudioSource audioSource, float time) {
		float elapse = 0, startVol = audioSource.volume;
		while (elapse <= time) {
			elapse += Time.deltaTime;
			var newVol = startVol * Mathf.Clamp01(1 - elapse / time);
			audioSource.volume = newVol;
			yield return null;
		}
		audioSource.Stop();
		audioSource.volume = 1f;
	}
	public void FadeOut(AudioSource audioSource, float time) {
		if (audioSource == null) 
			return;
		if (!audioSource.isPlaying)
			return;
		StartCoroutine(fadeOutCoroutine(audioSource, time));
	}
	IEnumerator playOneShotWithDelayCoroutine(AudioSource audioSource, AudioClip audioClip, 
		float delay, float volumeScale, bool destroy = false) {
		yield return new WaitForSeconds(delay);
		audioSource.PlayOneShot(audioClip, volumeScale);
		if (destroy) {
			yield return new WaitForSeconds(audioClip.length);
			Destroy(audioSource);
		}
	}
	public void PlayOneShot(AudioSource audioSource, AudioClip audioClip, float delay = 0f, 
		float volumeScale = 1f) {
		if (audioSource == null || audioClip == null) {
			return;
		}
		StartCoroutine(playOneShotWithDelayCoroutine(audioSource, audioClip, delay, volumeScale));
	}
	public void PlayOneShotWithTempAudioSource(AudioClip audioClip, float delay = 0f, 
		float volumeScale = 1f) {
		if (audioClip == null) {
			return;
		}
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		StartCoroutine(playOneShotWithDelayCoroutine(audioSource, audioClip, delay, volumeScale, destroy: true));
	}
	public void StopBGM() {
		BGMSource.clip = null;
		BGMSource.Stop();
	}
	public void PlayBGM(AudioClip clip, float fadeOut = 0.0f) {
		StartCoroutine(playBGMCoroutine(clip, fadeOut));
	}
	IEnumerator playBGMCoroutine(AudioClip clip, float fadeOut) {
		FadeOut(BGMSource, fadeOut);
		AudioSource audioSource = BGMSource;
		float elapse = 0, startVol = audioSource.volume;
		while (elapse <= fadeOut) {
			elapse += Time.deltaTime;
			var newVol = startVol * Mathf.Clamp01(1 - elapse / fadeOut);
			audioSource.volume = newVol;
			yield return null;
		}
		BGMSource.volume = 1f;
		BGMSource.clip = clip;
		BGMSource.loop = true;
		BGMSource.Play();
	}
	IEnumerator playVoiceCoroutine(AudioClip clip, float delay, float volumeScale) {
		yield return new WaitForSeconds(delay);
		voiceQ.Enqueue(clip);
		volumeQ.Enqueue(volumeScale);
	}
	public void PlayVoice(AudioClip clip, float delay = 0f, float volumeScale = 1f) {
		if (clip == null) {
			return;
		}
		StartCoroutine(playVoiceCoroutine(clip, delay, volumeScale));
	}
	public void ClearVoiceQueueAndStop() {
		voiceQ = new Queue<AudioClip>();
		volumeQ = new Queue<float>();
		VoiceSource.Stop();
		VoiceSource.clip = null;
	}
	public bool IsVoiceQueueCleared() {
		return (voiceQ.Count == 0) && (!VoiceSource.isPlaying);
	}
	void Awake() {
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
	void Start() {
		// BGMSource = GetComponent<AudioSource>();
	}
	Queue<AudioClip> voiceQ = new Queue<AudioClip>();
	Queue<float> volumeQ = new Queue<float>();
	// Update is called once per frame
	bool isPlayingVoice = false;
	void Update () {
		if (VoiceSource != null && !VoiceSource.isPlaying && voiceQ.Count > 0) {
			var a = voiceQ.Dequeue();
			var v = volumeQ.Dequeue();
			VoiceSource.clip = a;
			VoiceSource.PlayOneShot(a, v);
		}
		if (VoiceSource.isPlaying) {
			BGMSource.volume = 0.3f;
		}
		else {
			BGMSource.volume = 1.0f;
		}
	}
}
