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
        private Button _play;
        [SerializeField]
        private Button _pause;
        [SerializeField]
        private Slider _position;
        [SerializeField]
        private Text _timer;
        private VideoPlayer _player;
        private RawImage _image;

        private void OnEnable()
        {
            if (!_player)
            {
                _player = GetComponent<VideoPlayer>();
            }
            if (!_image)
            {
                _image = GetComponent<RawImage>();
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

        public void Play()
        {
            gameObject.SetActive(true);
            StartCoroutine(PlayVideo());
        }

        private IEnumerator PlayVideo()
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
                _player = GetComponent<VideoPlayer>();
            }
            if (!_image)
            {
                _image = GetComponent<RawImage>();
            }
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
            var length = TimeSpan.FromSeconds(_player.clip.length);
            while (_player.clip != null && _player.clip.length > _player.time)
            {
                if (Mathf.Approximately(_position.value, previousValue))
                {
                    _position.value = (float)(_player.time / _player.clip.length);
                }
                else
                {
                    _player.time = _position.value * _player.clip.length;
                }
                if (_timer)
                {
                    var time = TimeSpan.FromSeconds(_player.time);
                    _timer.text = (time.Hours > 0 ? time.Hours + ":" + time.Minutes.ToString("00") + ":" : time.Minutes + ":") + time.Seconds.ToString("00");
                    _timer.text += " / " + (length.Hours > 0 ? length.Hours + ":" + length.Minutes.ToString("00") + ":" : length.Minutes + ":") + length.Seconds.ToString("00");
                }
                previousValue = _position.value;
                if (!gameObject.activeInHierarchy)
                {
                    _player.clip = null;
                    _player.Stop();
                }
                yield return new WaitForSeconds(0.1f);
            }
            Stop();
        }

        public void Continue()
        {
            if (_player)
            {
                if (_player.time == 0)
                {
                    Play();
                }
                else
                {
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

        public void Pause()
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

        public void Stop()
        {
            if (_image)
            {
                _image.texture = null;
                _player.time = 0;
                gameObject.SetActive(false);
            }
        }
    }
}
