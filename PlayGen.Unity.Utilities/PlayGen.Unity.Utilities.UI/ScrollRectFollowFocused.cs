using System.Collections.Generic;
using System.Linq;
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

			var diffY = target.anchoredPosition.y % target.sizeDelta.y;
			var posY = (target.anchoredPosition.y - diffY) / (_scrollRect.content.sizeDelta.y - (diffY * 2));
			if (posY > 1)
			{
				posY = 1;
			}
			if (posY < 0)
			{
				posY = 0;
			}
			_scrollRect.verticalNormalizedPosition = posY;

			var diffX = target.anchoredPosition.x % target.sizeDelta.x;
			var posX = (target.anchoredPosition.x - diffX) / (_scrollRect.content.sizeDelta.x - (diffX * 2));
			if (posX > 1)
			{
				posX = 1;
			}
			if (posX < 0)
			{
				posX = 0;
			}
			_scrollRect.horizontalNormalizedPosition = posX;
		}
	}

}
