using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace PlayGen.Unity.Utilities.Editor.Localization
{
	public class LocalizationEditor
	{
		internal static List<CultureInfo> Languages;
		internal static List<string> Keys;

		internal static void GetKeys(bool reload = false)
		{
			if (reload || Keys == null || Keys.Count == 0)
			{
				GetLanguages(true);
				Keys = new List<string>();
				Keys = Utilities.Localization.Localization.Keys();
			}
		}

		internal static void GetLanguages(bool reload = false)
		{
			if (reload || Languages == null || Languages.Count == 0)
			{
				Languages = new List<CultureInfo>();
				Utilities.Localization.Localization.ClearAndInitialize();
				Languages = Utilities.Localization.Localization.Languages;
			}
		}

		[MenuItem("PlayGen Tools/Font Localization Character Check")]
		public static void FontLocalizationCharacterCheck()
		{
			GetKeys();
			var allFonts = Resources.FindObjectsOfTypeAll<Font>();
			var characterList = new HashSet<char>();
			foreach (var language in Languages)
			{
				foreach (var key in Keys)
				{
					var normal = Utilities.Localization.Localization.Get(key, false, language.Name);
					foreach (var c in normal)
					{
						if (!characterList.Contains(c))
						{
							characterList.Add(c);
						}
					}
					var upper = Utilities.Localization.Localization.Get(key, true, language.Name);
					foreach (var c in upper)
					{
						if (!characterList.Contains(c))
						{
							characterList.Add(c);
						}
					}
				}
			}
			Debug.Log("Unique Character Count: " + characterList.Count);
			foreach (var font in allFonts)
			{
				if (font.dynamic)
				{
					Debug.LogWarning(font.name + " is currently set to be a dynamic font. Dynamic fonts use default fonts when characters aren't available, so can't be tested. Change this setting to test this font.");
					continue;
				}
				var missingCharacters = new List<char>();
				foreach (var c in characterList)
				{
					if (!font.HasCharacter(c))
					{
						missingCharacters.Add(c);
					}
				}
				if (missingCharacters.Count == 0)
				{
					Debug.Log(font.name + " is missing no characters");
				}
				else
				{
					var debugString = font.name + " is missing the following characters:";
					foreach (var c in missingCharacters)
					{
						debugString += "\n" + c + " (Unicode Index:" + (int)c + ")"; 
					}
					Debug.LogWarning(debugString);
				}
			}
		}
	}
}
