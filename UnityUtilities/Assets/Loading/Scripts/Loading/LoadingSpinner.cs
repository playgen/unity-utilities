using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class LoadingSpinner : MonoBehaviour
{
	private GameObject _container;
	private Image _spinner;
	private Text _text;
	[SerializeField]
	private float _spinSpeed = 1;
	[SerializeField]
	private bool _spinClockwise;
	private bool _animate;
	private float _stopDelay;

	void Awake()
	{
		Loading.LoadingSpinner = this;
		_container = transform.GetChild(0).gameObject;
		_spinner = _container.GetComponentsInChildren<Image>(true).First(i => i.gameObject != _container.gameObject);
		_text = GetComponentInChildren<Text>(true);
	}

	private void Update()
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

	public void Set(bool clockwise, float speed)
	{
		_spinClockwise = clockwise;
		_spinSpeed = speed;
	}

	public void StartSpinner(string text)
	{
		_container.gameObject.SetActive(true);
		_text.text = text;
		_animate = true;
	}

	public void StopSpinner(string text, float stopDelay)
	{
		_text.text = text;
		_animate = false;
		_stopDelay = stopDelay;
	}
}
