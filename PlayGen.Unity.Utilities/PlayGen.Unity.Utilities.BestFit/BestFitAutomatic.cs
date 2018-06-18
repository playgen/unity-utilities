using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.BestFit
{
	/// <summary>
	/// Add this class as a component of an object that you want children with best fit to all be the same size
	/// </summary>
	public class BestFitAutomatic : MonoBehaviour
	{
		private List<Text> _bestFitChildren = new List<Text>();

		private void OnEnable()
		{
			OnChange();
			BestFit.ResolutionChange += OnChange;
		}

		private void OnDisable()
		{
			BestFit.ResolutionChange -= OnChange;
		}

		private void OnChange()
		{
			_bestFitChildren = _bestFitChildren.Where(t => t != null).ToList();
			var children = transform.GetComponentsInChildren<Text>().Where(child => child.resizeTextForBestFit).ToList();
			_bestFitChildren.AddRange(children);
			_bestFitChildren = _bestFitChildren.Distinct().ToList();
			_bestFitChildren.BestFit(false, false);
		}
	}
}
