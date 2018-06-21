using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.Localization
{
	[RequireComponent(typeof(Dropdown))]
	public class DropdownLocalization : UILocalization
	{
		/// <summary>
		/// The localization keys for this dropdown
		/// </summary>
		[Tooltip("The localization keys for this dropdown")]
		[SerializeField]
		protected List<string> _options;

		/// <summary>
		/// Set the dropdown option localization keys
		/// </summary>
		public virtual void SetOptions(List<string> options)
		{
			_options = options;
			Set();
		}

		public override void Set()
		{
			if (_options != null)
			{
				var dropdown = GetComponent<Dropdown>();
				dropdown.ClearOptions();
				var translatedOptions = _options.Select(t => Localization.Get(t)).ToList();
				dropdown.AddOptions(translatedOptions);
			}
		}
	}
}