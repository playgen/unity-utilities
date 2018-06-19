using UnityEngine;

namespace PlayGen.Unity.Utilities.Extensions
{
    public static class RectTransformExtensions
    {
        public static RectTransform RectTransform(this Transform transform)
        {
            return (RectTransform)transform;
        }

        public static RectTransform RectTransform(this GameObject gameObject)
        {
            return gameObject.transform.RectTransform();
        }

        public static RectTransform RectTransform(this MonoBehaviour mono)
        {
            return mono.transform.RectTransform();
        }

        public static RectTransform FindRect(this Transform transform, string find)
        {
            return transform.Find(find).RectTransform();
        }
    }
}
