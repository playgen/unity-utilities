using UnityEngine;
using System.Collections.Generic;

namespace PlayGen.Unity.Utilities.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Find child with name provided, including inactive objects
        /// </summary>
        public static Transform FindInactive(this Transform parent, string name)
        {
            var trs = parent.GetComponentsInChildren<Transform>(true);
            foreach (var t in trs)
            {
                if (t.name == name && t.parent == parent)
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// Find all children with name provided
        /// </summary>
        public static List<Transform> FindAll(this Transform parent, string name, bool includeInactive = false)
        {
            var found = new List<Transform>();
            var trs = parent.GetComponentsInChildren<Transform>(includeInactive);
            foreach (var t in trs)
            {
                if (t.name == name)
                {
                    found.Add(t);
                }
            }
            return found;
        }
    }
}
