using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[DisallowMultipleComponent]
public class BestFit : MonoBehaviour
{
	public static event Action ResolutionChange = delegate { };
	private Vector2 _previousResolution;
	public int MinFontSize = 1;
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

public static class BestFitExtensions {
	public static void BestFit(this GameObject go, bool setBestFitValues = true)
	{
		var childGo = new List<GameObject>();
		foreach (Transform child in go.transform)
		{
			childGo.Add(child.gameObject);
		}
		BestFit(childGo, setBestFitValues);
	}

	public static void BestFit(this List<Text> textObjects, bool setBestFitValues = true)
	{
		BestFit(textObjects.ToArray(), setBestFitValues);
	}

	public static void BestFit(this IEnumerable<Text> textObjects, bool setBestFitValues = true)
	{
		BestFit(textObjects.ToArray(), setBestFitValues);
	}

	public static void BestFit(this Text[] textObjects, bool setBestFitValues = true)
	{
		BestFit(textObjects.Select(text => text.gameObject).ToArray(), setBestFitValues);
	}

	public static void BestFit(this List<GameObject> gameObjects, bool setBestFitValues = true)
	{
		BestFit(gameObjects.ToArray(), setBestFitValues);
	}

	public static void BestFit(this IEnumerable<GameObject> gameObjects, bool setBestFitValues = true)
	{
		BestFit(gameObjects.ToArray(), setBestFitValues);
	}

	public static void BestFit(this GameObject[] gameObjects, bool setBestFitValues = true)
	{
		var smallestFontSize = 0;
		foreach (var go in gameObjects) {
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)go.transform);

			var mono = setBestFitValues ? go.GetComponentInParent<BestFit>() : null;
			var textObj = go.GetComponentsInChildren<Text>();
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
			var textObj = go.GetComponentsInChildren<Text>();
			foreach (var text in textObj)
			{
				text.fontSize = smallestFontSize;
			}
			var dropObj = go.GetComponentsInChildren<Dropdown>();
			foreach (var drop in dropObj)
			{
				var dropTextObj = drop.template.GetComponentsInChildren<Text>();
				foreach (var text in dropTextObj)
				{
					text.fontSize = (int)(smallestFontSize * ((((RectTransform)text.transform.parent).anchorMax.y - ((RectTransform)text.transform.parent).anchorMin.y) * 0.5f));
				}
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
