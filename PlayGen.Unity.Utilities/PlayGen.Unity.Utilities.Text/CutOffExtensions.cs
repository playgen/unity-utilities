using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace PlayGen.Unity.Utilities.Text
{
	public static class CutOffExtensions
	{
		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off due to length
		/// </summary>
		public static Component CutOff(this Component obj, uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(obj?.gameObject, maxLength, cutOffAfter, cutOffAppendment);
			return obj;
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off due to length
		/// </summary>
		public static List<Component> CutOff(this List<Component> objects, uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(objects?.ToArray(), maxLength, cutOffAfter, cutOffAppendment);
			return objects;
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off due to length
		/// </summary>
		public static IEnumerable<Component> CutOff(this IEnumerable<Component> objects, uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(objects?.ToArray(), maxLength, cutOffAfter, cutOffAppendment);
			return objects;
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off due to length
		/// </summary>
		public static Component[] CutOff(this Component[] objects, uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			//requires where as will produce errors if component is null
			CutOff(objects?.Where(obj => obj != null).Select(obj => obj.gameObject), maxLength, cutOffAfter, cutOffAppendment);
			return objects;
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off due to length
		/// </summary>
		public static GameObject CutOff(this GameObject go, uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(new[] { go }, maxLength, cutOffAfter, cutOffAppendment);
			return go;
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off due to length
		/// </summary>
		public static List<GameObject> CutOff(this List<GameObject> gameObjects, uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(gameObjects?.ToArray(), maxLength, cutOffAfter, cutOffAppendment);
			return gameObjects;
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off due to length
		/// </summary>
		public static IEnumerable<GameObject> CutOff(this IEnumerable<GameObject> gameObjects, uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(gameObjects?.ToArray(), maxLength, cutOffAfter, cutOffAppendment);
			return gameObjects;
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off due to length
		/// </summary>
		public static GameObject[] CutOff(this GameObject[] gameObjects, uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
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
					text.text = CutOff(text.text, maxLength, cutOffAfter, cutOffAppendment);
				}
			}
			return gameObjects;
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off due to length
		/// </summary>
		public static string CutOff(this string stringToCut, uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			var builder = new StringBuilder();
			if (cutOffAfter == null)
			{
				cutOffAfter = new char[0];
			}
			var cutOffPoint = maxLength == 0 ? stringToCut.Length : (int)Mathf.Min(stringToCut.Length, maxLength);
			for (var i = 0; i < cutOffPoint; i++)
			{
				if (!cutOffAfter.Contains(stringToCut[i]))
				{
					builder.Append(stringToCut[i]);
				}
				else
				{
					break;
				}
			}

			if (stringToCut.Length > cutOffPoint && builder.ToString().Length == cutOffPoint)
			{
				builder.Append(cutOffAppendment);
			}

			return builder.ToString();
		}
	}
}
