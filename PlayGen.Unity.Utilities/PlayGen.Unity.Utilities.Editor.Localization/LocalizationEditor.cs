using PlayGen.Unity.Utilities.Localization;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayGen.Unity.Utilities.Editor.Localization
{
	[CustomEditor(typeof(UILocalization), true)]
	[CanEditMultipleObjects]
	public class LocalizationEditor : UnityEditor.Editor
	{
		private UILocalization _myLoc;

		public void Awake()
		{
			_myLoc = (UILocalization)target;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
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
