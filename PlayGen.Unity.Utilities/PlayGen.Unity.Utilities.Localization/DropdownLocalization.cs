using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.Localization
{

	[RequireComponent(typeof(Dropdown))]
	public class DropdownLocalization : MonoBehaviour
	{
		[SerializeField]
		private List<string> _options;

		private void OnEnable()
		{
			Localization.LanguageChange += OnLanguageChange;
			Set();
		}

		private void OnDisable()
		{
			Localization.LanguageChange -= OnLanguageChange;
		}

		public void SetOptions(List<string> options)
		{
			_options = options;
			Set();
		}

		private void Set()
		{
			var dropdown = GetComponent<Dropdown>();
			dropdown.ClearOptions();
			var translatedOptions = _options.Select(t => Localization.Get(t)).ToList();
			dropdown.AddOptions(translatedOptions);
		}

		private void OnLanguageChange()
		{
			Set();
		}
	}
}