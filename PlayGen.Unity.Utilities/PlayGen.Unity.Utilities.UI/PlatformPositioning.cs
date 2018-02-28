using System;
using UnityEngine;

namespace PlayGen.Unity.Utilities.UI
{
    public class PlatformPositioning : MonoBehaviour
    {
        [Serializable]
        private class Anchor
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
        private Anchor _standalonePositioning;
        [SerializeField]
        private Anchor _mobilePositioning;

        private void OnEnable()
        {
            SetPosition();
        }

        public void SetPosition(bool forced = false, bool isForcedMobile = false)
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
        private void SetToCurrent()
        {
            _mobilePositioning = new Anchor(((RectTransform)transform).anchorMin, ((RectTransform)transform).anchorMax);
            _standalonePositioning = new Anchor(((RectTransform)transform).anchorMin, ((RectTransform)transform).anchorMax);
        }
    }
}