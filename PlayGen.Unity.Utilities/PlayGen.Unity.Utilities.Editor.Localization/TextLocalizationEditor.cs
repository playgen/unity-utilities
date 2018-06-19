using PlayGen.Unity.Utilities.Localization;
using UnityEditor;
using UnityEngine;

namespace PlayGen.Unity.Utilities.Editor.Localization
{
	[CustomEditor(typeof(TextLocalization), true)]
	[CanEditMultipleObjects]
	public class TextLocalizationEditor : UILocalizationEditor
	{
		public override void OnInspectorGUI()
		{
			LocalizationEditor.GetKeys();
			/*var index = LocalizationEditor.Keys.IndexOf(((TextLocalization)_myLoc).Key);
			if (LocalizationEditor.Keys.Count == 0)
			{
				GUILayout.Label("No Keys Found", EditorStyles.boldLabel);
			}
			else
			{
				((TextLocalization)_myLoc).Key = index >= 0 ? LocalizationEditor.Keys[EditorGUILayout.Popup("Key", index, LocalizationEditor.Keys.ToArray())] : EditorGUILayout.TextField("Key", ((TextLocalization)_myLoc).Key);
				((TextLocalization)_myLoc).ToUpper = EditorGUILayout.Toggle("Upper Case", ((TextLocalization)_myLoc).ToUpper);
			}*/
			DrawDefaultInspector();
			DrawTestingGUI();
		}
	}
}