using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.BestFit
{
	[DisallowMultipleComponent]
	public class BestFit : MonoBehaviour
	{
		/// <summary>
		/// Event triggered by resolution width or height changing
		/// </summary>
		public static event Action ResolutionChange = delegate { };
		protected static Vector2 _previousResolution = Vector2.zero;
		/// <summary>
		/// The smallest font size text should be set to. Set to 1 if no BestFit object is above the text object in the hierarchy.
		/// </summary>
		[Tooltip("The smallest font size text should be set to when using this extension")]
		public int MinFontSize = 1;
		/// <summary>
		/// The largest font size text should be set to. Set to 300 if no BestFit object is above the text object in the hierarchy.
		/// </summary>
		[Tooltip("The largest font size text should be set to when using this extension")]
		public int MaxFontSize = 300;

		protected virtual void Awake()
		{
			if (_previousResolution == Vector2.zero)
			{
				_previousResolution = new Vector2(Screen.width, Screen.height);
			}
		}

		protected virtual void LateUpdate()
		{
			if (!Mathf.Approximately(_previousResolution.x, Screen.width) || !Mathf.Approximately(_previousResolution.y, Screen.height))
			{
				_previousResolution = new Vector2(Screen.width, Screen.height);
				ResolutionChange();
			}
		}
	}

	public static class BestFitExtensions
	{
		/// <summary>
		/// Set all text on this Component and children of this Transform to be the same size
		/// If includeInactive is set to false, inactive GameObjects and text components will not be resized or used in resizing calculations
		/// </summary>
		public static int BestFit(this Component obj, bool includeInactive = true, List<string> newStrings = null)
		{
			return BestFit(obj?.gameObject, includeInactive, newStrings);
		}

		/// <summary>
		/// Set all text in this list of Components to be the same size
		/// If includeInactive is set to false, inactive GameObjects and text components will not be resized or used in resizing calculations
		/// </summary>
		public static int BestFit(this List<Component> objects, bool includeInactive = true, List<string> newStrings = null)
		{
			return BestFit(objects?.ToArray(), includeInactive, newStrings);
		}

		/// <summary>
		/// Set all text in this IEnumerable  of Components to be the same size
		/// If includeInactive is set to false, inactive GameObjects and text components will not be resized or used in resizing calculations
		/// </summary>
		public static int BestFit(this IEnumerable<Component> objects, bool includeInactive = true, List<string> newStrings = null)
		{
			return BestFit(objects?.ToArray(), includeInactive, newStrings);
		}

		/// <summary>
		/// Set all text in this array to of Components be the same size
		/// If includeInactive is set to false, inactive GameObjects and text components will not be resized or used in resizing calculations
		/// </summary>
		public static int BestFit(this Component[] objects, bool includeInactive = true, List<string> newStrings = null)
		{
			//requires where as will produce errors if component is null
			return BestFit(objects?.Where(obj => obj != null).Select(obj => obj.gameObject), includeInactive, newStrings);
		}

		/// <summary>
		/// Set all text on this GameObject and children of this GameObject to be the same size
		/// If includeInactive is set to false, inactive GameObjects and text components will not be resized or used in resizing calculations
		/// </summary>
		public static int BestFit(this GameObject go, bool includeInactive = true, List<string> newStrings = null)
		{
			return BestFit(new[] { go }, includeInactive, newStrings);
		}

		/// <summary>
		/// Set all text in this list of GameObjects to be the same size
		/// If includeInactive is set to false, inactive GameObjects and text components will not be resized or used in resizing calculations
		/// </summary>
		public static int BestFit(this List<GameObject> gameObjects, bool includeInactive = true, List<string> newStrings = null)
		{
			return BestFit(gameObjects?.ToArray(), includeInactive, newStrings);
		}

		/// <summary>
		/// Set all text in this IEnumerable of GameObjects to be the same size
		/// If includeInactive is set to false, inactive GameObjects and text components will not be resized or used in resizing calculations
		/// </summary>
		public static int BestFit(this IEnumerable<GameObject> gameObjects, bool includeInactive = true, List<string> newStrings = null)
		{
			return BestFit(gameObjects?.ToArray(), includeInactive, newStrings);
		}

		/// <summary>
		/// Set all text in this array of GameObjects to be the same size
		/// If includeInactive is set to false, inactive GameObjects and text components will not be resized or used in resizing calculations
		/// </summary>
		public static int BestFit(this GameObject[] gameObjects, bool includeInactive = true, List<string> newStrings = null)
		{
			//remove null gameobjects
			gameObjects = gameObjects?.Where(go => go != null).Distinct().ToArray();
			var previousSmallSize = 0;
			var smallestFontSize = 0;
			var checkCount = 0;
			if (!includeInactive)
			{
				gameObjects = gameObjects?.Where(go => go.activeSelf).ToArray();
			}
			//return zero if there's no objects to perform best fit on
			if (gameObjects == null || gameObjects.Length == 0)
			{
				return 0;
			}
			while ((previousSmallSize == 0 || previousSmallSize != smallestFontSize) && checkCount < 10)
			{
				previousSmallSize = smallestFontSize;

				//trigger layout to rebuild so that text areas are more accurate
				var layoutGroups = gameObjects.SelectMany(g => g.GetComponentsInParent<LayoutGroup>(includeInactive).Select(l => (RectTransform)l.transform)).ToList();
				layoutGroups = layoutGroups.Concat(gameObjects.Select(g => (RectTransform)g.transform).ToList()).ToList();
				layoutGroups = layoutGroups.Concat(gameObjects.SelectMany(g => g.GetComponentsInChildren<LayoutGroup>(includeInactive).Select(l => (RectTransform)l.transform)).ToList()).Distinct().ToList();
				layoutGroups.Reverse();
				foreach (var lg in layoutGroups)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(lg);
				}

				//trigger aspect ratio fitter resize so that text areas are more accurate
				var aspectRatioFitters = gameObjects.SelectMany(g => g.GetComponentsInParent<AspectRatioFitter>(includeInactive)).ToList();
				aspectRatioFitters = aspectRatioFitters.Concat(gameObjects.Select(g => g.GetComponent<AspectRatioFitter>()).Where(a => a != null).ToList()).ToList();
				aspectRatioFitters = aspectRatioFitters.Concat(gameObjects.SelectMany(g => g.GetComponentsInChildren<AspectRatioFitter>(includeInactive)).ToList()).Distinct().ToList();
				aspectRatioFitters.Reverse();
				foreach (var arf in aspectRatioFitters)
				{
					//same logic that AspectRatioFitter uses when resizing
					switch (arf.aspectMode)
					{
						case AspectRatioFitter.AspectMode.HeightControlsWidth:
							((RectTransform)arf.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ((RectTransform)arf.transform).rect.height * arf.aspectRatio);
							break;
						case AspectRatioFitter.AspectMode.WidthControlsHeight:
							((RectTransform)arf.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((RectTransform)arf.transform).rect.width / arf.aspectRatio);
							break;
						case AspectRatioFitter.AspectMode.None:
							break;
						case AspectRatioFitter.AspectMode.FitInParent:
							break;
						case AspectRatioFitter.AspectMode.EnvelopeParent:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}

				foreach (var go in gameObjects)
				{
					var dropObj = go.GetComponentsInChildren<Dropdown>(includeInactive);
					foreach (var drop in dropObj)
					{
						drop.template.gameObject.SetActive(true);
					}
					var textObj = go.GetComponentsInChildren<Text>(includeInactive);
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
						var newSize = GetBestFitSize(text, newStrings);
						if (newSize != 0 && (newSize < smallestFontSize || smallestFontSize == 0))
						{
							smallestFontSize = newSize;
						}
					}
				}
				foreach (var go in gameObjects)
				{
					var textObj = go.GetComponentsInChildren<Text>(includeInactive);
					foreach (var text in textObj)
					{
						text.fontSize = smallestFontSize;
					}
					var dropObj = go.GetComponentsInChildren<Dropdown>(includeInactive);
					foreach (var drop in dropObj)
					{
						drop.template.gameObject.SetActive(false);
					}
				}
				checkCount++;
			}
			return smallestFontSize;
		}

		private static int GetBestFitSize(Text text, List<string> newStrings)
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
			var bestFitBehaviour = text.GetComponentInParent<BestFit>();
			foreach (var s in newStrings)
			{
				text.text = s;
				text.resizeTextForBestFit = true;
				text.resizeTextMinSize = bestFitBehaviour ? bestFitBehaviour.MinFontSize : 1;
				text.resizeTextMaxSize = (bestFitBehaviour ? bestFitBehaviour.MaxFontSize : 300) + 1;
				text.fontSize = text.resizeTextMaxSize;
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
				var canvasScaler = text.GetComponentInParent<CanvasScaler>();
				while (canvasScaler != null && canvasScaler.transform.parent != null && canvasScaler.transform.parent.GetComponentInParent<CanvasScaler>())
				{
					canvasScaler = canvasScaler.transform.parent.GetComponentInParent<CanvasScaler>();
				}
				var canvas = text.GetComponentInParent<Canvas>();
				while (canvas != null && canvas.transform.parent != null && canvas.transform.parent.GetComponentInParent<Canvas>())
				{
					canvas = canvas.transform.parent.GetComponentInParent<Canvas>();
				}
				if (canvas && canvasScaler)
				{
					var logWidth = Mathf.Log(Screen.width / canvasScaler.referenceResolution.x, 2);
					var logHeight = Mathf.Log(Screen.height / canvasScaler.referenceResolution.y, 2);
					var logWeightedAverage = Mathf.Lerp(logWidth, logHeight, canvasScaler.matchWidthOrHeight);
					var logScaleFactor = Mathf.Pow(2, logWeightedAverage);
					if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera)
					{
						logScaleFactor *= canvas.worldCamera.orthographicSize / (Screen.height * 0.5f);
					}
					if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && !canvas.worldCamera) || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera && canvas.worldCamera.orthographic))
					{
						newSizeRescale *= canvas.transform.localScale.x / logScaleFactor;
					}
				}
				newSize = Mathf.FloorToInt(newSize * newSizeRescale);
				if (newSize < smallestFontSize || smallestFontSize == 0)
				{
					smallestFontSize = newSize;
				}
			}
			text.text = currentText;
			return Mathf.Min(smallestFontSize, bestFitBehaviour ? bestFitBehaviour.MaxFontSize : 300);
		}
	}
}
