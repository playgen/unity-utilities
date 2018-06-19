using System.Linq;

using PlayGen.Unity.Utilities.Localization;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.Editor.Localization
{
	[CustomEditor(typeof(UILocalization), true)]
	[CanEditMultipleObjects]
	public class UILocalizationEditor : UnityEditor.Editor
	{
		protected UILocalization _myLoc;

		protected virtual void Awake()
		{
			_myLoc = (UILocalization)target;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			DrawTestingGUI();
		}

		public void DrawTestingGUI()
		{
			if (!EditorApplication.isPlaying)
			{
				LocalizationEditor.GetLanguages();
				if (LocalizationEditor.Keys.Count != 0 && LocalizationEditor.Languages.Count != 0)
				{
					GUILayout.Space(10);
					GUILayout.Label("Localization Testing", EditorStyles.boldLabel);
					var index = Mathf.Max(0, LocalizationEditor.Languages.FindIndex(lang => _myLoc.LanguageOverride == lang.Name));

					EditorGUI.BeginChangeCheck();
					var overrideLang = LocalizationEditor.Languages[EditorGUILayout.Popup("Language Override", index, LocalizationEditor.Languages.Select(lang => lang.EnglishName).ToArray())].Name;
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(_myLoc, "Localization Override Change");
						_myLoc.LanguageOverride = overrideLang;
					}
					if (GUILayout.Button("Localize Text"))
					{
						Undo.RecordObject(_myLoc.GetComponent<Text>(), "Localization Override Change");
						_myLoc.gameObject.SetActive(false);
						_myLoc.Set();
						_myLoc.gameObject.SetActive(true);
					}
				}
			}
		}
	}
}
