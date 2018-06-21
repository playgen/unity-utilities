using System;
using UnityEngine;

namespace PlayGen.Unity.Utilities.UI
{
	public class PlatformPositioning : MonoBehaviour
	{
		[Serializable]
		protected struct Anchor
		{
			public Vector2 Min;
			public Vector2 Max;

			public Anchor(Vector2 min, Vector2 max)
			{
				Min = min;
				Max = max;
			}
		}
		[SerializeField]
		protected Anchor _standalonePositioning;
		[SerializeField]
		protected Anchor _mobilePositioning;

		protected virtual void OnEnable()
		{
			SetPosition();
		}

		protected virtual void SetPosition(bool forced = false, bool isForcedMobile = false)
		{
			if ((forced && isForcedMobile) || Application.isMobilePlatform)
			{
				((RectTransform)transform).anchorMin = _mobilePositioning.Min;
				((RectTransform)transform).anchorMax = _mobilePositioning.Max;
			}
			else
			{
				((RectTransform)transform).anchorMin = _standalonePositioning.Min;
				((RectTransform)transform).anchorMax = _standalonePositioning.Max;
			}
		}

		[ContextMenu("Set Values To Current")]
		protected virtual void SetToCurrent()
		{
			_mobilePositioning = new Anchor(((RectTransform)transform).anchorMin, ((RectTransform)transform).anchorMax);
			_standalonePositioning = new Anchor(((RectTransform)transform).anchorMin, ((RectTransform)transform).anchorMax);
		}
	}
}