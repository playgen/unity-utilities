using System;
using UnityEngine;

namespace PlayGen.Unity.Utilities.AudioManagement
{
	public class MusicManager : MonoBehaviour
	{
		[SerializeField]
		private MusicTrack[] _music;
		private AudioSource _audio;
		private int _currentTrack;
		private float _volume;
		private static MusicManager _instance;
		public static MusicTrack CurrentTrack => _instance._music[_instance._currentTrack];

		public static event Action TrackChange = delegate { };

		void Awake()
		{
			if (_instance)
			{
				Destroy(gameObject);
				return;
			}
			_instance = this;
			DontDestroyOnLoad(this);
		}

		void Start()
		{
			_audio = GetComponent<AudioSource>();
			if (!_audio)
			{
				_audio = gameObject.AddComponent<AudioSource>();
			}
			_volume = PlayerPrefs.GetFloat("Music Volume", 1);
			PlayTrack();
		}

		void Update()
		{
			if (!_audio.isPlaying && _music[_currentTrack].Clip.loadState != AudioDataLoadState.Loading)
			{
				NextTrack();
			}
		}

		public static void NextTrack()
		{
			_instance._audio.Stop();
			_instance._currentTrack++;
			if (_instance._currentTrack >= _instance._music.Length)
			{
				_instance._currentTrack = 0;
			}
			_instance.PlayTrack();
		}

		void PlayTrack()
		{
			_audio.clip = _music[_currentTrack].Clip;
			_audio.volume = _volume * _music[_currentTrack].MaxVolume;
			_audio.priority = _music[_currentTrack].Priority;
			_audio.Play();
			TrackChange();
		}

		public void UpdateVolume(float newValue)
		{
			PlayerPrefs.SetFloat("Music Volume", newValue);
			_volume = newValue;
		}
	}
}