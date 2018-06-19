using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.Loading
{
	public class LoadingSpinner : MonoBehaviour
	{
		protected GameObject _container;
		protected Image _spinner;
		protected Text _text;
		/// <summary>
		/// The speed that the spinner rotates at
		/// </summary>
		[Tooltip("The speed that the spinner rotates at")]
		[SerializeField]
		protected float _spinSpeed = 1;
		/// <summary>
		/// The direction of spinner rotation
		/// </summary>
		[Tooltip("The direction of spinner rotation")]
		[SerializeField]
		protected bool _spinClockwise;
		protected bool _animate;
		protected float _stopDelay;

		public bool IsActive => _container.gameObject.activeSelf;

		protected virtual void Awake()
		{
			Loading.LoadingSpinner = this;
			_container = transform.GetChild(0)?.gameObject;
			_spinner = _container?.GetComponentsInChildren<Image>(true).First(i => i.gameObject != _container.gameObject);
			_text = GetComponentInChildren<Text>(true);
		}

		protected virtual void Update()
		{
			if (_container.gameObject.activeSelf)
			{
				if (_animate || _stopDelay > 0)
				{
					_spinner.transform.Rotate(0, 0, (_spinClockwise ? -1 : 1) * _spinSpeed * Time.smoothDeltaTime);
					_stopDelay -= Time.smoothDeltaTime;
				}
				else
				{
					_container.gameObject.SetActive(false);
					_stopDelay = 0;
				}
			}
		}

		public virtual void Set(bool clockwise, float speed)
		{
			_spinClockwise = clockwise;
			_spinSpeed = speed;
		}

		public virtual void StartSpinner(string text)
		{
			_container.gameObject.SetActive(true);
			if (_text)
			{
				_text.text = text;
			}
			_animate = true;
		}

		public virtual void StopSpinner(string text, float stopDelay)
		{
			if (_text)
			{
				_text.text = text;
			}
			_animate = false;
			_stopDelay = stopDelay;
		}
	}
}