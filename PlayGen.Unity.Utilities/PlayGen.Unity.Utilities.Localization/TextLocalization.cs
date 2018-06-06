using UnityEngine;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.Localization
{
	[RequireComponent(typeof(Text))]
	public class TextLocalization : UILocalization
	{
		/// <summary>
		/// Localization Key for this text object
		/// </summary>
		[Tooltip("Localization Key for this text object")]
		public string Key;
		/// <summary>
		/// Should the text be converted to be upper case?
		/// </summary>
		[Tooltip("Should the text be converted to be upper case?")]
		public bool ToUpper;

		private Text _text;

		/// <summary>
		/// Set the text on this object to match the localized string for the provided key
		/// </summary>
		public override void Set()
		{
			if (!_text)
			{
				_text = GetComponent<Text>();
			}
			if (!_text)
			{
				Debug.LogError("Localization script could not find Text component attached to this gameObject: " + gameObject.name);
				return;
			}
			_text.text = Localization.Get(Key, ToUpper, LanguageOverride);
		}
	}
}
