using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Add this class as a component of an object that you want children with best fit to all be the same size
/// </summary>
public class BestFitAutomatic : MonoBehaviour
{
    private List<Text> _bestFitChildren = new List<Text>();

    void OnEnable()
    {
        var children = transform.GetComponentsInChildren<Text>();
        if (_bestFitChildren.Count == 0)
        {
            _bestFitChildren = children.Where(child => child.resizeTextForBestFit).ToList();
        }

        var smallestFontSize = 0;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);

        foreach (var text in _bestFitChildren)
        {
            text.resizeTextForBestFit = true;

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

        foreach (var text in _bestFitChildren)
        {
            text.fontSize = smallestFontSize+1;
        }
    }
}
