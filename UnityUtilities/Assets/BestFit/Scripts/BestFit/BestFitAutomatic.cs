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
		if (_bestFitChildren.Count == 0)
		{
			var children = transform.GetComponentsInChildren<Text>();
			_bestFitChildren = children.Where(child => child.resizeTextForBestFit).ToList();
		}
		OnChange();
		BestFit.ResolutionChange += OnChange;
	}

	private void OnDisable()
	{
		BestFit.ResolutionChange -= OnChange;
	}

	private void OnChange()
	{
		_bestFitChildren.BestFit(false);
	}
}
