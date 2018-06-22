using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace PlayGen.Unity.Utilities.Video
{
	[RequireComponent(typeof(VideoPlayer))]
	[RequireComponent(typeof(RawImage))]
	public class VideoPlayerUI : MonoBehaviour
	{
		[SerializeField]
		protected Button _play;
		[SerializeField]
		protected Button _pause;
		[SerializeField]
		protected Slider _position;
		[SerializeField]
		protected Text _timer;
		protected VideoPlayer _player;
		protected RawImage _image;

		protected virtual void OnEnable()
		{
			if (!_player)
			{
				GetVideoPlayer();
				//test performance
				if (_player.playOnAwake)
				{
					Stop();
					PlayCurrent();
					return;
				} 
			}
			if (!_image)
			{
				_image = GetComponent<RawImage>();
				_image.texture = null;
			}
			if (_play)
			{
				_play.gameObject.SetActive(!_player.isPlaying);
			}
			if (_pause)
			{
				_pause.gameObject.SetActive(_player.isPlaying);
			}
		}

		protected virtual void GetVideoPlayer()
		{
			_player = GetComponent<VideoPlayer>();
			_player.waitForFirstFrame = true;
			_player.renderMode = VideoRenderMode.APIOnly;
		}

		public virtual void SetClip(VideoClip clip)
		{
			if (!_player)
			{
				GetVideoPlayer();
			}
			_player.source = VideoSource.VideoClip;
			_player.clip = clip;
		}

		public virtual void SetURL(string url)
		{
			if (!_player)
			{
				GetVideoPlayer();
			}
			_player.source = VideoSource.Url;
			_player.url = url;
		}

		public virtual void PlayDefault()
		{
			Play();
		}

		public virtual void PlayCurrent()
		{
			Play(_player.isLooping, _player.playbackSpeed);
		}

		public virtual void Play(bool loop = false, float playbackSpeed = 1f)
		{
			gameObject.SetActive(true);
			StartCoroutine(PlayVideo(loop, playbackSpeed));
		}

		protected virtual IEnumerator PlayVideo(bool loop = false, float playbackSpeed = 1f)
		{
			if (_position)
			{
				_position.value = 0;
			}
			if (_play)
			{
				_play.gameObject.SetActive(false);
			}
			if (_pause)
			{
				_pause.gameObject.SetActive(true);
			}
			if (!_player)
			{
				GetVideoPlayer();
			}
			if (!_image)
			{
				_image = GetComponent<RawImage>();
				_image.texture = null;
			}
			if (GetComponent<AudioSource>())
			{
				_player.SetTargetAudioSource(0, GetComponent<AudioSource>());
				_player.controlledAudioTrackCount = 1;
			}
			_player.playOnAwake = false;
			_player.isLooping = loop;
			_player.playbackSpeed = Mathf.Min(playbackSpeed, 3);
			_image.enabled = false;
			_player.Prepare();
			while (!_player.isPrepared)
			{
				yield return new WaitForSeconds(0.2f);
			}
			_image.texture = _player.texture;
			_player.Play();
			_image.enabled = true;
			var previousValue = 0f;
			var clipLength = _player.source == VideoSource.VideoClip ? _player.clip.length : _player.frameCount / _player.frameRate;
			var length = TimeSpan.FromSeconds(clipLength);
			while ((_player.source == VideoSource.VideoClip ? _player.clip != null : !string.IsNullOrEmpty(_player.url)) && clipLength > _player.time)
			{
				if (_position)
				{
					if (Mathf.Approximately(_position.value, previousValue))
					{
						_position.value = (float)(_player.time / clipLength);
					}
					else
					{
						_player.time = _position.value * clipLength;
					}
					previousValue = _position.value;
				}
				if (_timer)
				{
					var time = TimeSpan.FromSeconds(_player.time);
					_timer.text = (time.Hours > 0 ? time.Hours + ":" + time.Minutes.ToString("00") + ":" : time.Minutes + ":") + time.Seconds.ToString("00");
					_timer.text += " / " + (length.Hours > 0 ? length.Hours + ":" + length.Minutes.ToString("00") + ":" : length.Minutes + ":") + length.Seconds.ToString("00");
				}
				if (!gameObject.activeInHierarchy)
				{
					_player.clip = null;
					_player.url = null;
					_player.Stop();
				}
				yield return new WaitForSeconds(0.1f / _player.playbackSpeed);
			}
			if (_player.isLooping)
			{
				Play();
			}
			else
			{
				Stop();
			}
		}

		public virtual void Continue()
		{
			Continue(_player.isLooping, _player.playbackSpeed);
		}

		public virtual void Continue(bool loop, float playbackSpeed = 1f)
		{
			if (_player)
			{
				if (Mathf.Approximately((float)_player.time, 0))
				{
					Play(loop, playbackSpeed);
				}
				else
				{
					_player.isLooping = loop;
					_player.playbackSpeed = Mathf.Min(playbackSpeed, 3);
					_player.Play();
					if (_play)
					{
						_play.gameObject.SetActive(false);
					}
					if (_pause)
					{
						_pause.gameObject.SetActive(true);
					}
				}
			}
		}

		public virtual void Pause()
		{
			if (_player)
			{
				_player.Pause();
				if (_play)
				{
					_play.gameObject.SetActive(true);
				}
				if (_pause)
				{
					_pause.gameObject.SetActive(false);
				}
			}
		}

		public virtual void Stop()
		{
			if (_image)
			{
				_image.texture = null;
			}
			if (_player)
			{
				_player.Stop();
				_player.time = 0;
			}
			gameObject.SetActive(false);
		}

		protected virtual void OnDestroy()
		{
			Stop();
		}
	}
}
