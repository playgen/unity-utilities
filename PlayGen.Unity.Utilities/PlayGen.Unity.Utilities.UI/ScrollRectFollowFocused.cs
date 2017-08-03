using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.UI
{
	public class ScrollRectFollowFocused : MonoBehaviour
	{
		private ScrollRect _scrollRect;
		private GameObject _lastSelected;

		private void Start()
		{
			_scrollRect = GetComponent<ScrollRect>();
		}

		private void Update()
		{
			if (_lastSelected != EventSystem.current.currentSelectedGameObject)
			{
				_lastSelected = EventSystem.current.currentSelectedGameObject;
				if (!_lastSelected.transform.IsChildOf(transform))
				{
					Destroy(gameObject);
				}
				else
				{
					SnapTo((RectTransform)_lastSelected.transform);
				}
			}
		}

		private void SnapTo(RectTransform target)
		{
			Canvas.ForceUpdateCanvases();

			var diff = target.anchoredPosition.y % target.sizeDelta.y;
			var pos = (target.anchoredPosition.y - diff) / (_scrollRect.content.sizeDelta.y - (diff * 2));
			if (pos > 1)
			{
				pos = 1;
			}
			if (pos < 0)
			{
				pos = 0;
			}
			_scrollRect.verticalNormalizedPosition = pos;
		}
	}

}
