using System.Linq;
using UnityEditor;

namespace PlayGen.Unity.Utilities.Editor.Localization
{
	public class LocalizationPostprocessor : AssetPostprocessor
	{
		public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			var assets = importedAssets.ToList();
			assets.AddRange(deletedAssets);
			assets.AddRange(movedAssets);
			assets.AddRange(movedFromAssetPaths);

			if (assets.Any(a => a.Contains("/Resources/Localization")))
			{
				LocalizationEditor.GetKeys(true);
			}
		}
	}
}
