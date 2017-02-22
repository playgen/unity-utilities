using PlayGen.Unity.Utilities.Localization;

using UnityEditor;

namespace PlayGen.Unity.Utilities.Editor.Localization
{
	[CustomEditor(typeof(Utilities.Localization.Localization))]
	public class LocalizationEditor : UnityEditor.Editor
	{
		private Language _lastLang;
		private Utilities.Localization.Localization _myLoc;

		public void Awake()
		{
			_myLoc = (Utilities.Localization.Localization)target;
			_lastLang = _myLoc.LanguageOverride;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			if (EditorApplication.isPlaying)
			{
				if (_lastLang != _myLoc.LanguageOverride)
				{
					_myLoc.Set();
					_lastLang = _myLoc.LanguageOverride;
				}
			}
		}
	}	
}
