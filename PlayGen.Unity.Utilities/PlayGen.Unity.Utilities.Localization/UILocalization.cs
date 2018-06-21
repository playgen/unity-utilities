using UnityEngine;

namespace PlayGen.Unity.Utilities.Localization
{
	public abstract class UILocalization : MonoBehaviour
	{
		#region LocalizationTesting
		[HideInInspector]
		public string LanguageOverride;
		#endregion

		protected virtual void OnEnable()
		{
			Set();
		}

		public abstract void Set();
	}
}
