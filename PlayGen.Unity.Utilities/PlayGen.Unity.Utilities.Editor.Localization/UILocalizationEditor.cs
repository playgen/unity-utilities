using System.Collections.Generic;
using System.Linq;

using PlayGen.Unity.Utilities.Localization;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayGen.Unity.Utilities.Editor.Localization
{
	[CustomEditor(typeof(UILocalization), true)]
	[CanEditMultipleObjects]
	public class UILocalizationEditor : UnityEditor.Editor
	{
		private UILocalization _myLoc;

		public void Awake()
		{
			_myLoc = (UILocalization)target;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			LocalizationEditor.GetLanguages();
			var index = Mathf.Max(0, LocalizationEditor.Languages.IndexOf(_myLoc.LanguageOverride));
			_myLoc.LanguageOverride = LocalizationEditor.Languages[EditorGUILayout.Popup("Language Override", index, LocalizationEditor.Languages.ToArray())];
			if (GUILayout.Button("Localize Text"))
			{
				if (!EditorApplication.isPlaying)
				{
					EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
				}
				_myLoc.gameObject.SetActive(false);
				_myLoc.Set();
				_myLoc.gameObject.SetActive(true);
			}
		}
	}
}
