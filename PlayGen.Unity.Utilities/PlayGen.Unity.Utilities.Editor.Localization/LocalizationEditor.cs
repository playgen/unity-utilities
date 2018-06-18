using System.Collections.Generic;
using System.Linq;

namespace PlayGen.Unity.Utilities.Editor.Localization
{
	public class LocalizationEditor
	{
		internal static List<string> Languages;

		internal static void GetLanguages()
		{
			if (Languages == null || Languages.Count == 0)
			{
				Languages = new List<string>();
				Utilities.Localization.Localization.Initialize();
				Languages = Utilities.Localization.Localization.Languages.Select(l => l.Name).ToList();
			}
		}
	}
}
