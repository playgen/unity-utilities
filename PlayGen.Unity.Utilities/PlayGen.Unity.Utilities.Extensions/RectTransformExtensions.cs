using UnityEngine;
using UnityEngine.UI;

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

        public static T FindComponent<T>(this Transform transform, string find) where T : Component
        {
            var compTrans = transform.Find(find);
            return compTrans != null ? compTrans.GetComponent<T>() : null;
        }

        public static Image FindImage(this Transform transform, string find)
        {
            return transform.FindComponent<Image>(find);
        }

        public static Text FindText(this Transform transform, string find)
        {
            return transform.FindComponent<Text>(find);
        }

        public static Button FindButton(this Transform transform, string find)
        {
            return transform.FindComponent<Button>(find);
        }

        public static GameObject FindObject(this Transform transform, string find)
        {
            return transform.Find(find).gameObject;
        }

        public static T FindComponentInChildren<T>(this Transform transform, string find)
        {
            return transform.Find(find).GetComponentInChildren<T>();
        }
    }
}
