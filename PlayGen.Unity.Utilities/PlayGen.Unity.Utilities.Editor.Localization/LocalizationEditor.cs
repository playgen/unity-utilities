using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace PlayGen.Unity.Utilities.Editor.Localization
{
	[CustomEditor(typeof(Utilities.Localization.Localization))]
	public class LocalizationEditor : UnityEditor.Editor
	{
		private Utilities.Localization.Localization _myLoc;

		public void Awake()
		{
			_myLoc = (Utilities.Localization.Localization)target;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
            if (GUILayout.Button("Localize Text"))
            {
                if (!EditorApplication.isPlaying)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
                _myLoc.gameObject.SetActive(false);
                _myLoc.Set();
                _myLoc.gameObject.SetActive(true);

            }
        }
	}	
}
