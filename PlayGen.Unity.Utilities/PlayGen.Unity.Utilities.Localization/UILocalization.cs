using UnityEngine;

namespace PlayGen.Unity.Utilities.Localization
{
	public abstract class UILocalization : MonoBehaviour
	{
		#region LocalizationTesting
		[Header("Localization Testing")]
		[Tooltip("Use this enum to test other languages")]
		public string LanguageOverride;
		#endregion

		private void OnEnable()
		{
			Set();
		}

		public abstract void Set();
	}
}
