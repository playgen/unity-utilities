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
		public static int BestFit(this GameObject go, bool setBestFitValues = true, List<string> newStrings = null)
		{
			var childGo = new List<GameObject>();
			foreach (Transform child in go.transform)
			{
				childGo.Add(child.gameObject);
			}
			return BestFit(childGo, setBestFitValues, newStrings);
		}

		/// <summary>
		/// Set all text on this Transform and children of this Transform to be the same size
		/// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
		/// </summary>
		public static int BestFit(this Transform trans, bool setBestFitValues = true, List<string> newStrings = null)
		{
			var childGo = new List<GameObject>();
			foreach (Transform child in trans)
			{
				childGo.Add(child.gameObject);
			}
			return BestFit(childGo, setBestFitValues, newStrings);
		}

		/// <summary>
		/// Set all text in this list of selectables to be the same size
		/// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
		/// </summary>
		public static int BestFit(this List<Selectable> selectableObjects, bool setBestFitValues = true, List<string> newStrings = null)
		{
			return BestFit(selectableObjects.ToArray(), setBestFitValues, newStrings);
		}

		/// <summary>
		/// Set all text in this IEnumerable  of selectables to be the same size
		/// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
		/// </summary>
		public static int BestFit(this IEnumerable<Selectable> selectableObjects, bool setBestFitValues = true, List<string> newStrings = null)
		{
			return BestFit(selectableObjects.ToArray(), setBestFitValues, newStrings);
		}

		/// <summary>
		/// Set all text in this array to of selectables be the same size
		/// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
		/// </summary>
		public static int BestFit(this Selectable[] selectableObjects, bool setBestFitValues = true, List<string> newStrings = null)
		{
			return BestFit(selectableObjects.Select(selectable => selectable.gameObject).ToArray(), setBestFitValues, newStrings);
		}

		/// <summary>
		/// Set all text in this list to be the same size
		/// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
		/// </summary>
		public static int BestFit(this List<Text> textObjects, bool setBestFitValues = true, List<string> newStrings = null)
		{
			return BestFit(textObjects.ToArray(), setBestFitValues, newStrings);
		}

		/// <summary>
		/// Set all text in this IEnumerable to be the same size
		/// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
		/// </summary>
		public static int BestFit(this IEnumerable<Text> textObjects, bool setBestFitValues = true, List<string> newStrings = null)
		{
			return BestFit(textObjects.ToArray(), setBestFitValues, newStrings);
		}

		/// <summary>
		/// Set all text in this array to be the same size
		/// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
		/// </summary>
		public static int BestFit(this Text[] textObjects, bool setBestFitValues = true, List<string> newStrings = null)
		{
			return BestFit(textObjects.Select(text => text.gameObject).ToArray(), setBestFitValues, newStrings);
		}

		/// <summary>
		/// Set all text in this list of GameObjects to be the same size
		/// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
		/// </summary>
		public static int BestFit(this List<GameObject> gameObjects, bool setBestFitValues = true, List<string> newStrings = null)
		{
			return BestFit(gameObjects.ToArray(), setBestFitValues, newStrings);
		}

		/// <summary>
		/// Set all text in this IEnumerable of GameObjects to be the same size
		/// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
		/// </summary>
		public static int BestFit(this IEnumerable<GameObject> gameObjects, bool setBestFitValues = true, List<string> newStrings = null)
		{
			return BestFit(gameObjects.ToArray(), setBestFitValues, newStrings);
		}

		/// <summary>
		/// Set all text in this array of GameObjects to be the same size
		/// If setBestFitValues is set to false, the best fit values set on object will be used instead of MinFontSize and MaxFontSize
		/// </summary>
		public static int BestFit(this GameObject[] gameObjects, bool setBestFitValues = true, List<string> newStrings = null)
		{
			var previousSmallSize = 0;
			var smallestFontSize = 0;
			var checkCount = 0;
			while ((previousSmallSize == 0 || previousSmallSize != smallestFontSize) && checkCount < 10)
			{
				previousSmallSize = smallestFontSize;

				var layoutGroups = gameObjects.SelectMany(g => g.GetComponentsInParent<LayoutGroup>(true).Select(l => (RectTransform)l.transform)).ToList();
				layoutGroups = layoutGroups.Concat(gameObjects.Select(g => (RectTransform)g.transform).ToList()).ToList();
				layoutGroups = layoutGroups.Concat(gameObjects.SelectMany(g => g.GetComponentsInChildren<LayoutGroup>(true).Select(l => (RectTransform)l.transform)).ToList()).Distinct().ToList();
				layoutGroups.Reverse();
				foreach (var lg in layoutGroups)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(lg);
				}

				var aspectRatioFitters = gameObjects.SelectMany(g => g.GetComponentsInParent<AspectRatioFitter>(true)).ToList();
				aspectRatioFitters = aspectRatioFitters.Concat(gameObjects.Select(g => g.GetComponent<AspectRatioFitter>()).Where(a => a != null).ToList()).ToList();
				aspectRatioFitters = aspectRatioFitters.Concat(gameObjects.SelectMany(g => g.GetComponentsInChildren<AspectRatioFitter>(true)).ToList()).Distinct().ToList();
				aspectRatioFitters.Reverse();
				foreach (var arf in aspectRatioFitters)
				{
					if (arf.aspectMode == AspectRatioFitter.AspectMode.HeightControlsWidth)
					{
						((RectTransform)arf.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ((RectTransform)arf.transform).rect.height * arf.aspectRatio);
					}
					else if (arf.aspectMode == AspectRatioFitter.AspectMode.WidthControlsHeight)
					{
						((RectTransform)arf.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((RectTransform)arf.transform).rect.width / arf.aspectRatio);
					}
				}

				foreach (var go in gameObjects)
				{
					var dropObj = go.GetComponentsInChildren<Dropdown>(true);
					foreach (var drop in dropObj)
					{
						drop.template.gameObject.SetActive(true);
					}
					var mono = setBestFitValues ? go.GetComponentInParent<BestFit>() : null;
					var textObj = go.GetComponentsInChildren<Text>(true);
					foreach (var text in textObj)
					{
						var dropdown = text.GetComponentInParent<Dropdown>();
						if (dropdown)
						{
							if (newStrings != null)
							{
								newStrings.AddRange(dropdown.options.Select(o => o.text).ToList());
							}
							else
							{
								newStrings = dropdown.options.Select(o => o.text).ToList();
							}
						}
						var newSize = GetBestFitSize(text, mono, setBestFitValues, newStrings);
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
				checkCount++;
			}
			return smallestFontSize;
		}

		private static int GetBestFitSize(Text text, BestFit mono, bool setBestFitValue, List<string> newStrings)
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
				//log logic copied from https://bitbucket.org/Unity-Technologies/ui/src/a3f89d5f7d145e4b6fa11cf9f2de768fea2c500f/UnityEngine.UI/UI/Core/Layout/CanvasScaler.cs?at=2017.3&fileviewer=file-view-default
				//allows calculation to be accurate from the first frame
				var logWidth = Mathf.Log(Screen.width / text.GetComponentInParent<CanvasScaler>().referenceResolution.x, 2);
				var logHeight = Mathf.Log(Screen.height / text.GetComponentInParent<CanvasScaler>().referenceResolution.y, 2);
				var logWeightedAverage = Mathf.Lerp(logWidth, logHeight, text.GetComponentInParent<CanvasScaler>().matchWidthOrHeight);
				var logScaleFactor = Mathf.Pow(2, logWeightedAverage);
				newSizeRescale *= text.GetComponentInParent<Canvas>().transform.localScale.x / logScaleFactor;
				newSize = Mathf.FloorToInt(newSize * newSizeRescale);
				if (newSize != 0 && (newSize < smallestFontSize || smallestFontSize == 0))
				{
					smallestFontSize = newSize;
				}
			}
			text.text = currentText;
			return Mathf.Min(Mathf.Max(smallestFontSize, mono ? mono.MinFontSize : 1), mono ? mono.MaxFontSize : 25);
		}
	}
}
