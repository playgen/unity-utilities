using UnityEngine;

namespace PlayGen.Unity.Utilities.Extensions
{
    public static class GameObjectExtensions
    {
        public static void Active(this GameObject go, bool active)
        {
            if (go.activeInHierarchy != active)
            {
                go.SetActive(active);
            }
        }
    }
}
