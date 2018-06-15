using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace PlayGen.Unity.Utilities.AudioManagement
{
	public abstract class GenericMusicManager<T> : MonoBehaviour where T : MusicTrack
	{
		/// <summary>
		/// List of Music Tracks
		/// </summary>
		[Tooltip("List of Music Tracks")]
		[SerializeField]
		protected List<T> _music = new List<T>();
		/// <summary>
		/// Audio Source which will play the music.
		/// </summary>
		protected AudioSource _audio;
		/// <summary>
		/// Index of track currently being played
		/// </summary>
		protected int _currentTrack;
		/// <summary>
		/// Base volume of the music
		/// </summary>
		protected float _volume;
		/// <summary>
		/// Singleton
		/// </summary>
		protected static GenericMusicManager<T> _instance;
		/// <summary>
		/// Track currently being played
		/// </summary>
		public static T CurrentTrack => _instance._music[_instance._currentTrack];
		/// <summary>
		/// Base volume of the music
		/// </summary>
		public static float CurrentVolume => _instance._volume;

		public static event Action TrackChange = delegate { };

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
			_audio = GetComponent<AudioSource>();
			if (!_audio)
			{
				_audio = gameObject.AddComponent<AudioSource>();
			}
			_volume = PlayerPrefs.GetFloat("Music Volume", 1);
			PlayTrack();
		}

		protected virtual void Update()
		{
			if (!_audio.isPlaying && _music[_currentTrack].Clip.loadState != AudioDataLoadState.Loading)
			{
				NextTrack();
			}
		}

		/// <summary>
		/// Play the previous track in the list
		/// </summary>
		public static void PreviousTrack()
		{
			_instance.PlayPreviousTrack();
		}

		/// <summary>
		/// Play the previous track in the list
		/// </summary>
		protected virtual void PlayPreviousTrack()
		{
			_audio.Stop();
			_currentTrack--;
			if (_currentTrack < 0)
			{
				_currentTrack = _music.Count - 1;
			}
			PlayTrack();
		}

		/// <summary>
		/// Play the next track in the list
		/// </summary>
		public static void NextTrack()
		{
			_instance.PlayNextTrack();
		}

		/// <summary>
		/// Play the next track in the list
		/// </summary>
		protected virtual void PlayNextTrack()
		{
			_audio.Stop();
			_currentTrack++;
			if (_currentTrack >= _music.Count)
			{
				_currentTrack = 0;
			}
			PlayTrack();
		}

		/// <summary>
		/// Play the track with this name in the list of MusicTracks
		/// </summary>
		public static void PlayTrack(string trackName)
		{
			var track = _instance._music.IndexOf(_instance._music.FirstOrDefault(t => t.TrackName == trackName));
			if (track != -1)
			{
				PlayTrack(track);
			}
			else
			{
				Debug.LogWarning("Invalid Track Name: " + trackName);
			}
		}

		/// <summary>
		/// Play the track with this AudioClip in the list of MusicTracks
		/// </summary>
		public static void PlayTrack(AudioClip clip)
		{
			var track = _instance._music.IndexOf(_instance._music.FirstOrDefault(t => t.Clip == clip));
			if (track != -1)
			{
				PlayTrack(track);
			}
			else
			{
				Debug.LogWarning("Invalid Track Clip: " + clip.name);
			}
		}

		/// <summary>
		/// Play the track at this position in the list of MusicTracks
		/// </summary>
		public static void PlayTrack(int trackNumber)
		{
			if (trackNumber >= _instance._music.Count || trackNumber < 0)
			{
				Debug.LogWarning("Invalid Track Number: " + trackNumber);
			}
			else
			{
				_instance._currentTrack = trackNumber;
				_instance.PlayTrack();
			}
		}

		/// <summary>
		/// Play the track whose index in the list matches currentTrack
		/// </summary>
		protected virtual void PlayTrack()
		{
			_audio.clip = _music[_currentTrack].Clip;
			_audio.volume = _volume * _music[_currentTrack].Volume;
			_audio.priority = _music[_currentTrack].Priority;
			_audio.Play();
			TrackChange();
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
			PlayerPrefs.SetFloat("Music Volume", newValue);
			_volume = newValue;
			_audio.volume = _volume * _music[_currentTrack].Volume;
		}
	}
}