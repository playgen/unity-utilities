using System.Collections.Generic;
using UnityEngine;

namespace PlayGen.Unity.Utilities.AudioManagement
{
	public abstract class GenericSfxManager<T> : MonoBehaviour where T : SfxClip
	{
		/// <summary>
		/// List of sound effect clips
		/// </summary>
		[Tooltip("List of sound effect clips")]
		[SerializeField]
		protected List<T> _audioClips;
		/// <summary>
		/// Dictionary of clip names and their audio sources
		/// </summary>
		protected readonly Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();
		/// <summary>
		/// Base volume of the sound effects
		/// </summary>
		protected float _volume;
		/// <summary>
		/// Singleton
		/// </summary>
		protected static GenericSfxManager<T> _instance;

		protected virtual void Awake()
		{
			if (_instance)
			{
				Destroy(gameObject);
				return;
			}
			_instance = this;
			DontDestroyOnLoad(this);
		}

		protected virtual void Start()
		{
			_volume = PlayerPrefs.GetFloat("Sound Volume", 1);
			foreach (var audioClip in _audioClips)
			{
				if (!_audioSources.ContainsKey(audioClip.Name))
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
				else
				{
					Debug.LogWarning("Clip with name " + audioClip.Name + " not set up due to duplicate clip name");
				}
			}
		}

		/// <summary>
		/// Play the sound effect with the provided name
		/// </summary>
		public static void Play(string sourceName)
		{
			_instance.PlaySound(sourceName);
		}

		/// <summary>
		/// Play the sound effect with the provided name
		/// </summary>
		protected virtual void PlaySound(string sourceName)
		{
			if (_audioSources.TryGetValue(sourceName, out var audioSource))
			{
				audioSource.Stop();
				audioSource.Play();
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + sourceName);
			}
		}

		/// <summary>
		/// Stop the sound effect with the provided name
		/// </summary>
		public static void Stop(string sourceName)
		{
			_instance.StopSound(sourceName);
		}

		/// <summary>
		/// Stop the sound effect with the provided name
		/// </summary>
		protected virtual void StopSound(string sourceName)
		{
			if (_audioSources.TryGetValue(sourceName, out var audioSource))
			{
				if (audioSource.isPlaying)
				{
					audioSource.Stop();
				}
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + sourceName);
			}
		}

		/// <summary>
		/// Pause the sound effect with the provided name
		/// </summary>
		public static void Pause(string sourceName)
		{
			_instance.PauseSound(sourceName);
		}

		/// <summary>
		/// Pause the sound effect with the provided name
		/// </summary>
		protected virtual void PauseSound(string sourceName)
		{
			if (_audioSources.TryGetValue(sourceName, out var audioSource))
			{
				if (audioSource.isPlaying)
				{
					audioSource.Pause();
				}
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + sourceName);
			}
		}

		/// <summary>
		/// Resume the sound effect with the provided name
		/// </summary>
		public static void Resume(string sourceName)
		{
			_instance.ResumeSound(sourceName);
		}

		/// <summary>
		/// Resume the sound effect with the provided name
		/// </summary>
		protected virtual void ResumeSound(string sourceName)
		{
			if (_audioSources.TryGetValue(sourceName, out var audioSource))
			{
				audioSource.Play();
			}
			else
			{
				Debug.LogWarning("Invalid Clip Name: " + sourceName);
			}
		}

		/// <summary>
		/// Is the sound effect with the provided name playing?
		/// </summary>
		public static bool IsPlaying(string sourceName)
		{
			return _instance.IsSoundPlaying(sourceName);
		}

		/// <summary>
		/// Is the sound effect with the provided name playing?
		/// </summary>
		protected virtual bool IsSoundPlaying(string sourceName)
		{
			if (_audioSources.TryGetValue(sourceName, out var audioSource))
			{
				return audioSource.isPlaying;
			}
			Debug.LogWarning("Invalid Clip Name: " + sourceName);
			return false;
		}

		/// <summary>
		/// Update the base volume value
		/// </summary>
		public static void UpdateVolume(float newValue)
		{
			_instance.ChangeVolume(newValue);
		}

		/// <summary>
		/// Update the base volume value
		/// </summary>
		protected virtual void ChangeVolume(float newValue)
		{
			PlayerPrefs.SetFloat("Sound Volume", newValue);
			_volume = newValue;
			_audioClips.ForEach(a => a.AudioSource.volume = a.Volume * _volume);
		}
	}
}
