using System.Collections.Generic;
using System.Globalization;

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
	}
}
