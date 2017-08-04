using System.Collections.Generic;
using UnityEngine;

namespace PlayGen.Unity.Utilities.AudioManagement
{
	public class SfxManager : MonoBehaviour
	{
		[SerializeField] private List<SfxClip> _audioClips;

		private readonly Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();

		private float _volume;

		private static SfxManager _instance;

		private void Awake()
		{
			if (_instance)
			{
				Destroy(gameObject);
				return;
			}
			_instance = this;
			DontDestroyOnLoad(this);
		}

		private void Start()
		{
			_volume = PlayerPrefs.GetFloat("Sound Volume", 1);
			foreach (var audioClip in _audioClips)
			{
				var source = gameObject.AddComponent<AudioSource>();
				source.clip = audioClip.Clip;
				source.priority = audioClip.Priority;
				source.volume = audioClip.Volume * _volume;

				source.loop = audioClip.Loop;
				source.playOnAwake = audioClip.AutoPlay;
				if (audioClip.AutoPlay)
				{
					source.Play();
				}

				_audioSources.Add(audioClip.Name, source);
				audioClip.AudioSource = source;
			}
		}

		public static void Play(string name)
		{
			if (_instance._audioSources.ContainsKey(name))
			{
				_instance._audioSources[name].Stop();
				_instance._audioSources[name].Play();
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + name);
			}
		}

		public static void Stop(string name)
		{
			if (_instance._audioSources.ContainsKey(name))
			{
				if (_instance._audioSources[name].isPlaying)
				{
					_instance._audioSources[name].Stop();
				}
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + name);
			}
		}

		public static void Pause(string name)
		{
			if (_instance._audioSources.ContainsKey(name))
			{
				if (_instance._audioSources[name].isPlaying)
				{
					_instance._audioSources[name].Pause();
				}
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + name);
			}
		}

		public static void Resume(string name)
		{
			if (_instance._audioSources.ContainsKey(name))
			{
				_instance._audioSources[name].Play();
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + name);
			}
		}

		public static bool IsPlaying(string name)
		{
			if (_instance._audioSources.ContainsKey(name))
			{
				return _instance._audioSources[name].isPlaying;
			}
			Debug.LogWarning("Invalid Clip Name: " + name);
			return false;
		}

		public static void UpdateVolume()
		{
			_instance._volume = PlayerPrefs.GetFloat("Sound Volume", 1);
			_instance._audioClips.ForEach(a => a.AudioSource.volume = a.Volume * _instance._volume);
		}
	}
}
