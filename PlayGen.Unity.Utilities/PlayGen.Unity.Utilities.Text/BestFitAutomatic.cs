using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace PlayGen.Unity.Utilities.Text
{
	/// <summary>
	/// Add this class as a component of an object that you want children with best fit to all be the same size
	/// </summary>
	public class BestFitAutomatic : BestFit
	{
		protected List<UnityEngine.UI.Text> _bestFitChildren = new List<UnityEngine.UI.Text>();
		/// <summary>
		/// Should inactive GameObjects and text components be resized and used in resizing calculations?
		/// </summary>
		[Tooltip("Should inactive GameObjects and text components be resized and used in resizing calculations?")]
		[SerializeField]
		protected bool _includeInactive = true;
		/// <summary>
		/// A list of strings that will also be tested on every text component as well as the current text
		/// </summary>
		[Tooltip("A list of strings that will also be tested on every text component as well as the current text")]
		[SerializeField]
		protected List<string> _newStrings = new List<string>();

		protected virtual void OnEnable()
		{
			OnChange();
			ResolutionChange += OnChange;
		}

		protected virtual void OnDisable()
		{
			ResolutionChange -= OnChange;
		}

		public virtual void OnChange()
		{
			OnChange(_includeInactive, _newStrings);
		}

		public virtual void OnChange(bool includeInactive, List<string> newStrings = null)
		{
			_bestFitChildren = _bestFitChildren.Where(t => t != null).ToList();
			var children = transform.GetComponentsInChildren<UnityEngine.UI.Text>().ToList();
			_bestFitChildren.AddRange(children);
			_bestFitChildren = _bestFitChildren.Distinct().ToList();
			_bestFitChildren.BestFit(includeInactive, newStrings);
		}
	}
}
