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

		/// <summary>
		/// Set the text on this object to match the localized string for the provided key
		/// </summary>
		public override void Set()
		{
			Text text = GetComponent<Text>();
			if (!text)
			{
				Debug.LogError("Localization script could not find Text component attached to this gameObject: " + gameObject.name);
				return;
			}
			text.text = Localization.Get(Key, ToUpper, LanguageOverride);
		}
	}
}
