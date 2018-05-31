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

	    public delegate void OnSet();
        /// <summary>
        /// Event fired every time the Set method is called
        /// </summary>
	    public event OnSet SetEvent;

		private void OnEnable()
		{
			Set();
		}

	    public virtual void Set()
	    {
            SetEvent?.Invoke();
	    }
	}
}
