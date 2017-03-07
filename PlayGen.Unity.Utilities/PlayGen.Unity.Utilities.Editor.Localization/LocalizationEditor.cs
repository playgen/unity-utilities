using PlayGen.Unity.Utilities.Localization;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

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
            if (GUILayout.Button("Localize Text"))
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                _myLoc.gameObject.SetActive(false);
                _myLoc.Set();
                _myLoc.gameObject.SetActive(true);

            }
        }
	}	
}
