using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace PlayGen.Unity.Utilities.Text
{
	public static class ForceOneLineExtensions
	{
		/// <summary>
		/// Forces text to remain on one line
		/// </summary>
		public static Component ForceOneLine(this Component obj)
		{
			ForceOneLine(obj?.gameObject);
			return obj;
		}

		/// <summary>
		/// Forces text to remain on one line
		/// </summary>
		public static List<Component> ForceOneLine(this List<Component> objects)
		{
			ForceOneLine(objects?.ToArray());
			return objects;
		}

		/// <summary>
		/// Forces text to remain on one line
		/// </summary>
		public static IEnumerable<Component> ForceOneLine(this IEnumerable<Component> objects)
		{
			ForceOneLine(objects?.ToArray());
			return objects;
		}

		/// <summary>
		/// Forces text to remain on one line
		/// </summary>
		public static Component[] ForceOneLine(this Component[] objects)
		{
			//requires where as will produce errors if component is null
			ForceOneLine(objects?.Where(obj => obj != null).Select(obj => obj.gameObject));
			return objects;
		}

		/// <summary>
		/// Forces text to remain on one line
		/// </summary>
		public static GameObject ForceOneLine(this GameObject go)
		{
			ForceOneLine(new[] { go });
			return go;
		}

		/// <summary>
		/// Forces text to remain on one line
		/// </summary>
		public static List<GameObject> ForceOneLine(this List<GameObject> gameObjects)
		{
			ForceOneLine(gameObjects?.ToArray());
			return gameObjects;
		}

		/// <summary>
		/// Forces text to remain on one line
		/// </summary>
		public static IEnumerable<GameObject> ForceOneLine(this IEnumerable<GameObject> gameObjects)
		{
			ForceOneLine(gameObjects?.ToArray());
			return gameObjects;
		}

		/// <summary>
		/// Forces text to remain on one line
		/// </summary>
		public static GameObject[] ForceOneLine(this GameObject[] gameObjects)
		{
			//remove null gameobjects
			var objects = gameObjects?.Where(go => go != null).Distinct().ToArray();
			if (objects == null || objects.Length == 0)
			{
				return gameObjects;
			}
			foreach (var go in objects)
			{
				var textObj = go.GetComponentsInChildren<UnityEngine.UI.Text>(true);
				foreach (var text in textObj)
				{
					text.text = ForceOneLine(text.text);
				}
			}
			return gameObjects;
		}

		/// <summary>
		/// Forces text to remain on one line
		/// </summary>
		public static string ForceOneLine(this string str)
		{
			return str.Replace(" ", "\u00a0");
		}
	}
}
