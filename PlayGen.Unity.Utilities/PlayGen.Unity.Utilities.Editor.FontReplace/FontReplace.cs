using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PlayGen.Unity.Utilities.Editor.FontReplace
{
	public class FontReplace : EditorWindow
	{
		private readonly List<Font> _gameFonts = new List<Font>();
		private List<string> _names = new List<string>();

		[MenuItem("PlayGen Tools/Replace Fonts")]
		public static void ShowWindow()
		{
			GetWindow(typeof(FontReplace));
		}

		private void OnGUI()
		{
			GUILayout.Label("Replace selected fonts", EditorStyles.boldLabel);
			if (GUILayout.Button("Refresh") || _gameFonts.Count == 0)
			{
				GUILayout.Space(10f);
				_names = FontNames();
			}
			for (var i = 0; i < _names.Count; i++)
			{
				//list all the fonts
				if (_gameFonts.Count == 0 || _gameFonts.Count < i + 1)
					_gameFonts.Add(new Font());
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label(_names[i]);
				if (GUILayout.Button("Replace With"))
				{
					ReplaceFont(_names[i], _gameFonts[i]);
				}
				_gameFonts[i] = (Font)EditorGUILayout.ObjectField(_gameFonts[i], typeof(Font), true);
				//create a button to replace the selected font with the font dragged in

				EditorGUILayout.EndHorizontal();
			}
		}

		private void ReplaceFont(string fontName, Font font)
		{
			//check the font has been set to a valid font
			if (font == null || font.fontNames.Length == 0)
				return;
			//now get all text types in the scene and change the ones with a matching font fontName to the new font
			var allTexts = Resources.FindObjectsOfTypeAll<UnityEngine.UI.Text>();
			foreach (var t in allTexts)
			{
				if (t.font.ToString() == fontName)
				{
					t.font = font;
				}
			}
		}

		private List<string> FontNames()
		{
			//return a string of font names
			var fontNames = new List<string>();
			var allTexts = Resources.FindObjectsOfTypeAll<UnityEngine.UI.Text>();

			foreach (var t in allTexts)
			{
				if (!fontNames.Contains(t.font.name))
				{
					fontNames.Add(t.font.name);
				}
			}
			return fontNames;
		}
	}
}
