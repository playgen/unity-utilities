using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.BestFit
{
	[RequireComponent(typeof(Canvas))]
	[DisallowMultipleComponent]
	public class BestFit : MonoBehaviour
	{
        /// <summary>
        /// Event triggered by resolution width or height changing
        /// </summary>
		public static event Action ResolutionChange = delegate { };
		private Vector2 _previousResolution;
        /// <summary>
        /// The smallest font size text should be set to
        /// </summary>
        [Tooltip("The smallest font size text should be set to when using this extension")]
		public int MinFontSize = 1;
        /// <summary>
        /// The largest font size text should be set to
        /// </summary>
        [Tooltip("The largest font size text should be set to when using this extension")]
        public int MaxFontSize = 25;

		private void Awake()
		{
			_previousResolution = new Vector2(Screen.width, Screen.height);
		}

		private void LateUpdate()
		{
			if (!Mathf.Approximately(_previousResolution.x, Screen.width) || !Mathf.Approximately(_previousResolution.y, Screen.height))
			{
				ResolutionChange();
				_previousResolution = new Vector2(Screen.width, Screen.height);
			}
		}
	}

	public static class BestFitExtensions
	{
        /// <summary>
        /// Set all text on this GameObject and children of this GameObject to be the same size
        /// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
        /// </summary>
		public static void BestFit(this GameObject go, bool setBestFitValues = true)
		{
			var childGo = new List<GameObject>();
			foreach (Transform child in go.transform)
			{
				childGo.Add(child.gameObject);
			}
			BestFit(childGo, setBestFitValues);
		}

        /// <summary>
        /// Set all text in this list to be the same size
        /// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
        /// </summary>
		public static void BestFit(this List<Text> textObjects, bool setBestFitValues = true)
		{
			BestFit(textObjects.ToArray(), setBestFitValues);
		}

        /// <summary>
        /// Set all text in this IEnumerable to be the same size
        /// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
        /// </summary>
		public static void BestFit(this IEnumerable<Text> textObjects, bool setBestFitValues = true)
		{
			BestFit(textObjects.ToArray(), setBestFitValues);
		}

        /// <summary>
        /// Set all text in this array to be the same size
        /// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
        /// </summary>
		public static void BestFit(this Text[] textObjects, bool setBestFitValues = true)
		{
			BestFit(textObjects.Select(text => text.gameObject).ToArray(), setBestFitValues);
		}

        /// <summary>
        /// Set all text in this list of GameObjects to be the same size
        /// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
        /// </summary>
		public static void BestFit(this List<GameObject> gameObjects, bool setBestFitValues = true)
		{
			BestFit(gameObjects.ToArray(), setBestFitValues);
		}

        /// <summary>
        /// Set all text in this IEnumerable of GameObjects to be the same size
        /// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
        /// </summary>
		public static void BestFit(this IEnumerable<GameObject> gameObjects, bool setBestFitValues = true)
		{
			BestFit(gameObjects.ToArray(), setBestFitValues);
		}

        /// <summary>
        /// Set all text in this array of GameObjects to be the same size
        /// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
        /// </summary>
		public static void BestFit(this GameObject[] gameObjects, bool setBestFitValues = true)
		{
			var smallestFontSize = 0;
			foreach (var go in gameObjects)
			{
                var dropObj = go.GetComponentsInChildren<Dropdown>(true);
                foreach (var drop in dropObj)
                {
                    drop.template.gameObject.SetActive(true);
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)go.transform);

				var mono = setBestFitValues ? go.GetComponentInParent<BestFit>() : null;
				var textObj = go.GetComponentsInChildren<Text>(true);
				foreach (var text in textObj)
				{
					var dropdown = text.GetComponentInParent<Dropdown>();
					var newSize = dropdown ? GetBestFitSize(text, mono, setBestFitValues, dropdown.options.Select(o => o.text).ToList()) : GetBestFitSize(text, mono, setBestFitValues);
					if (newSize < smallestFontSize || smallestFontSize == 0)
					{
						smallestFontSize = newSize;
					}
				}
			}
			foreach (var go in gameObjects)
			{
				var textObj = go.GetComponentsInChildren<Text>(true);
				foreach (var text in textObj)
				{
					text.fontSize = smallestFontSize;
				}
				var dropObj = go.GetComponentsInChildren<Dropdown>(true);
				foreach (var drop in dropObj)
				{
                    
                    drop.template.gameObject.SetActive(false);
                }
			}
		}

		private static int GetBestFitSize(Text text, BestFit mono, bool setBestFitValue, List<string> newStrings = null)
		{
			var smallestFontSize = 0;
			var currentText = text.text;
			if (newStrings == null)
			{
				newStrings = new List<string> { currentText };
			}
			if (!newStrings.Contains(currentText))
			{
				newStrings.Add(currentText);
			}
			foreach (var s in newStrings)
			{
				text.text = s;
				text.resizeTextForBestFit = true;
				if (setBestFitValue)
				{
					text.resizeTextMinSize = mono ? mono.MinFontSize : 1;
					text.resizeTextMaxSize = mono ? mono.MaxFontSize : 25;
					text.fontSize = text.resizeTextMaxSize;
				}
				text.cachedTextGenerator.Invalidate();
				text.cachedTextGenerator.Populate(text.text, text.GetGenerationSettings(text.rectTransform.rect.size));
				text.resizeTextForBestFit = false;
				var newSize = text.cachedTextGenerator.fontSizeUsedForBestFit;
				var newSizeRescale = text.rectTransform.rect.size.x / text.cachedTextGenerator.rectExtents.size.x;
				if (text.rectTransform.rect.size.y / text.cachedTextGenerator.rectExtents.size.y < newSizeRescale)
				{
					newSizeRescale = text.rectTransform.rect.size.y / text.cachedTextGenerator.rectExtents.size.y;
				}
				newSize = Mathf.FloorToInt(newSize * newSizeRescale);
				if (newSize < smallestFontSize || smallestFontSize == 0)
				{
					smallestFontSize = newSize;
				}
			}
			text.text = currentText;
			return smallestFontSize;
		}
	}
}
