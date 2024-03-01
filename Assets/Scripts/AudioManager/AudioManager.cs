using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager Instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;
	public AudioSource musicSource;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		//Play("Theme");
	}

	public void Play(string sound, AudioSource source = null)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}
		
		source.clip = s.clip;
		source.volume = s.volume;
		source.pitch = s.pitch;
		Debug.Log(source.clip + "Playing");
		source.Play();
	}

}
