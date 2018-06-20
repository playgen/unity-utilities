using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace PlayGen.Unity.Utilities.Text
{
	public class CutOff : MonoBehaviour
	{

	}

	public static class CutOffExtensions
	{
		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off before the end
		/// </summary>
		public static void CutOff(this Component obj, int maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(obj?.gameObject, maxLength, cutOffAfter, cutOffAppendment);
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off before the end
		/// </summary>
		public static void CutOff(this List<Component> objects, int maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(objects?.ToArray(), maxLength, cutOffAfter, cutOffAppendment);
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off before the end
		/// </summary>
		public static void CutOff(this IEnumerable<Component> objects, int maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(objects?.ToArray(), maxLength, cutOffAfter, cutOffAppendment);
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off before the end
		/// </summary>
		public static void CutOff(this Component[] objects, int maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			//requires where as will produce errors if component is null
			CutOff(objects?.Where(obj => obj != null).Select(obj => obj.gameObject), maxLength, cutOffAfter, cutOffAppendment);
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off before the end
		/// </summary>
		public static void CutOff(this GameObject go, int maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(new[] { go }, maxLength, cutOffAfter, cutOffAppendment);
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off before the end
		/// </summary>
		public static void CutOff(this List<GameObject> gameObjects, int maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(gameObjects?.ToArray(), maxLength, cutOffAfter, cutOffAppendment);
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off before the end
		/// </summary>
		public static void CutOff(this IEnumerable<GameObject> gameObjects, int maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			CutOff(gameObjects?.ToArray(), maxLength, cutOffAfter, cutOffAppendment);
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off before the end
		/// </summary>
		public static void CutOff(this GameObject[] gameObjects, int maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			foreach (var go in gameObjects)
			{
				var textObj = go.GetComponentsInChildren<UnityEngine.UI.Text>(true);
				foreach (var text in textObj)
				{
					text.text = CutOff(text.text, maxLength, cutOffAfter, cutOffAppendment);
				}
			}
		}

		/// <summary>
		/// Cut off string after max length of characters or when a character in the array occurs
		/// Default maxLength of 0 is taken as there being no max length
		/// cutOffAppendment is added to the end of the string if it was cut off before the end
		/// </summary>
		public static string CutOff(this string stringToCut, int maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "")
		{
			var builder = new StringBuilder();
			if (cutOffAfter == null)
			{
				cutOffAfter = new char[0];
			}
			var cutOffPoint = maxLength == 0 ? stringToCut.Length : Mathf.Min(stringToCut.Length, maxLength);
			for (var i = 0; i < cutOffPoint; i++)
			{
				if (!cutOffAfter.Contains(stringToCut[i]) && !cutOffAfter.Contains(stringToCut[i]))
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
