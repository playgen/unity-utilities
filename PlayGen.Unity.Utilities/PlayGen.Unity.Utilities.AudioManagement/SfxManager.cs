using System.Collections.Generic;
using UnityEngine;

namespace PlayGen.Unity.Utilities.AudioManagement
{
	public class SfxManager : MonoBehaviour
	{
		[SerializeField] protected List<SfxClip> _audioClips;

		protected readonly Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();

		private static SfxManager _sfxInstance;

		protected virtual void Awake()
		{
			if (_sfxInstance)
			{
				Destroy(gameObject);
				return;
			}
			_sfxInstance = this;
			DontDestroyOnLoad(this);
		}

		protected virtual void Start()
		{
			foreach (var audioClip in _audioClips)
			{
				var source = gameObject.AddComponent<AudioSource>();
				source.clip = audioClip.Clip;
				source.priority = audioClip.Priority;
				source.volume = audioClip.Volume;

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
			if (_sfxInstance._audioSources.ContainsKey(name))
			{
				_sfxInstance._audioSources[name].Stop();
				_sfxInstance._audioSources[name].Play();
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + name);
			}
		}

		public static void Stop(string name)
		{
			if (_sfxInstance._audioSources.ContainsKey(name))
			{
				if (_sfxInstance._audioSources[name].isPlaying)
				{
					_sfxInstance._audioSources[name].Stop();
				}
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + name);
			}
		}

		public static void Pause(string name)
		{
			if (_sfxInstance._audioSources.ContainsKey(name))
			{
				if (_sfxInstance._audioSources[name].isPlaying)
				{
					_sfxInstance._audioSources[name].Pause();
				}
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + name);
			}
		}

		public static void Resume(string name)
		{
			if (_sfxInstance._audioSources.ContainsKey(name))
			{
				_sfxInstance._audioSources[name].Play();
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + name);
			}
		}

		public static bool IsPlaying(string name)
		{
			if (_sfxInstance._audioSources.ContainsKey(name))
			{
				return _sfxInstance._audioSources[name].isPlaying;
			}
			Debug.LogWarning("Invalid Clip Name: " + name);
			return false;
		}
	}
}
