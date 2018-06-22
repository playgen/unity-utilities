using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.Extensions
{
	public static class TransformExtensions
	{
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

		public static GameObject Parent(this Transform transform)
		{
			return transform.parent == null ? null : transform.parent.gameObject;
		}

		public static T FindComponentInChildren<T>(this Transform transform, string find, bool includeInactive = false)
		{
			return transform.Find(find).GetComponentInChildren<T>(includeInactive);
		}

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
