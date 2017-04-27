using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;
using System;
using System.Globalization;
using System.Linq;

namespace PlayGen.Unity.Utilities.Localization
{
    [RequireComponent(typeof(Text))]
	public class Localization : MonoBehaviour
	{
		private static readonly Dictionary<CultureInfo, Dictionary<string, string>> LocalizationDict = new Dictionary<CultureInfo, Dictionary<string, string>>();

        /// <summary>
        /// Localization Key for this text object
        /// </summary>
        [Tooltip("Localization Key for this text object")]
		public string Key;
        /// <summary>
        /// Should the text be converted to be upper case?
        /// </summary>
        [Tooltip("Should the text be converted to be upper case?")]
        public bool ToUpper;

		private const string EmptyStringText = "XXXX";
        private const string FilePath = "Localization";

        public static List<CultureInfo> Languages = new List<CultureInfo>();
        /// <summary>
        /// The Language currently in-use
        /// </summary>
		public static CultureInfo SelectedLanguage { get; set; }
        /// <summary>
        /// Language to use if none is set or selected language does not have text for provided key
        /// </summary>
		public static CultureInfo DefaultLanguage = new CultureInfo("en-gb");
        /// <summary>
        /// Event triggered by changing of SelectedLanguage
        /// </summary>
		public static event Action LanguageChange = delegate { };

		#region LocalizationTesting
		[Header("Localization Testing")]
		[Tooltip("Use this enum to test other languages")]
		public string LanguageOverride;
		#endregion

		private void OnEnable()
		{
			Set();
		}

        /// <summary>
        /// Set the text on this object to match the localized string for the provided key
        /// </summary>
		public void Set()
		{
			Text text = GetComponent<Text>();
			if (!text)
			{
				Debug.LogError("Localization script could not find Text component attached to this gameObject: " + gameObject.name);
				return;
			}
			text.text = Get(Key, ToUpper, LanguageOverride);
		}

		private static void GetLocalizationDictionary()
		{
			TextAsset[] jsonTextAssets = Resources.LoadAll(FilePath, typeof(TextAsset)).Cast<TextAsset>().ToArray();

            foreach (var textAsset in jsonTextAssets)
            {
	            AddLocalization(textAsset);
            }

			UpdateLanguage(PlayerPrefs.GetString("Last_Saved_Language"));
			if (SelectedLanguage == null || string.IsNullOrEmpty(SelectedLanguage.Name))
			{
				UpdateLanguage(GetLanguage(CultureInfo.CurrentCulture));
				PlayerPrefs.SetString("Last_Saved_Language", SelectedLanguage.Name);
			}
		}

		public static void AddLocalization(TextAsset textAsset)
		{
			AddLocalization(textAsset.text);
		}

		public static void AddLocalization(string text)
		{
			AddLanguages(text);
			AddToDict(text);
			RemoveUnused();
		}

		private static void AddLanguages(string text)
		{
			var n = JSON.Parse(text);
			var keys = n[0].AsObject.Keys;
			for (int i = 1; i < keys.Count; i++)
			{
				var culture = new CultureInfo(keys[i]);
				if (!Languages.Contains(culture))
				{
					Languages.Add(culture);
				}
			}
		}

		private static void AddToDict(string text)
		{
			foreach (var l in Languages)
			{
				Dictionary<string, string> languageStrings = new Dictionary<string, string>();
				var n = JSON.Parse(text);
				for (int i = 0; i < n.Count; i++)
				{
					//go through the list and add the strings to the dictionary
					if (n[i][l.Name.ToLower()] != null)
					{
						string key = n[i][0].ToString();
						key = key.Replace("\"", "").ToUpper();
						string value = n[i][l.Name.ToLower()].ToString();
						value = value.Replace("\"", "");
						if (value != EmptyStringText)
						{
							languageStrings[key] = value;
						}
					}
				}
				if (languageStrings.Count > 0)
				{
					if (LocalizationDict.ContainsKey(l))
					{
						foreach (var s in languageStrings)
						{
							if (!LocalizationDict[l].ContainsKey(s.Key))
							{
								LocalizationDict[l][s.Key] = s.Value;
							}
						}
					}
					else
					{
						LocalizationDict[l] = languageStrings;
					}
				}
			}
		}

		private static void RemoveUnused()
		{
			List<CultureInfo> langToRemove = new List<CultureInfo>();
			foreach (var l in Languages)
			{
				if (!LocalizationDict.ContainsKey(l))
				{
					langToRemove.Add(l);
				}
			}
			foreach (var l in langToRemove)
			{
				Languages.Remove(l);
			}
		}

        /// <summary>
        /// Get the localized string for the provided key
        /// </summary>
		public static string Get(string key, bool toUpper = false, string overrideLanguage = null)
		{
			if (SelectedLanguage == null)
			{
				GetLocalizationDictionary();
			}
			if (string.IsNullOrEmpty(key))
			{
				return string.Empty;
			}
			string txt;
			var newKey = key.ToUpper();
			newKey = newKey.Replace('-', '_');

            var getLang = SelectedLanguage;

            if (!string.IsNullOrEmpty(overrideLanguage) && overrideLanguage != SelectedLanguage.Name)
			{
				getLang = GetLanguage(new CultureInfo(overrideLanguage));
			}
			LocalizationDict[getLang].TryGetValue(newKey, out txt);
			if (txt == null || txt == EmptyStringText)
			{
				if (txt == null)
				{
					Debug.LogWarning("Could not find string with key '" + key + "' in Language " + getLang);
				}
				if (!Equals(getLang.Parent, CultureInfo.InvariantCulture) && Languages.Contains(getLang.Parent))
				{
					LocalizationDict[getLang.Parent].TryGetValue(newKey, out txt);
				}
			}
			if (txt == null || txt == EmptyStringText)
			{
				var parentLang = !Equals(getLang.Parent, CultureInfo.InvariantCulture) ? getLang.Parent : getLang;
				foreach (var lang in Languages)
				{
					if (Equals(lang.Parent, parentLang) && !Equals(lang, getLang))
					{
						LocalizationDict[lang].TryGetValue(newKey, out txt);
						if (txt != null && txt != EmptyStringText)
						{
							break;
						}
					}
				}
			}
			if (txt == null || txt == EmptyStringText)
			{
				LocalizationDict[DefaultLanguage].TryGetValue(newKey, out txt);
			}
			if (txt == null || txt == EmptyStringText)
			{
				txt = key;
			}
			//new line character in spreadsheet is *n*
			txt = txt.Replace("\\n", "\n");
			if (toUpper)
			{
				txt = txt.ToUpper();
			}
			return txt;
		}

        /// <summary>
        /// Get the localized string for the provided key and format it using the args provided
        /// </summary>
		public static string GetAndFormat(string key, bool toUpper, params object[] args)
		{
			return string.Format(Get(key, toUpper), args);
		}

        /// <summary>
        /// Get the localized string for the provided key and format it using the args provided
        /// </summary>
		public static string GetAndFormat(string key, bool toUpper, params string[] args)
		{
			return GetAndFormat(key, toUpper, args.ToArray<object>());
		}

        /// <summary>
        /// Check if the SelectedLanguage contains the provided key
        /// </summary>
		public static bool HasKey(string key)
		{
			var newKey = key.ToUpper();
			newKey = newKey.Replace('-', '_');
			return LocalizationDict[SelectedLanguage].ContainsKey(newKey);
		}

		private static CultureInfo GetLanguage(CultureInfo language)
		{
			var culture = language;
			if (!Languages.Contains(culture))
			{
				if (!Equals(language.Parent, CultureInfo.InvariantCulture))
				{
					culture = language.Parent;
				}
				if (!Languages.Contains(culture))
				{
					culture = Languages.Where(c => Equals(c.Parent, culture)).ToList().Count > 0 ? Languages.First(c => Equals(c.Parent, culture)) : DefaultLanguage;
				}
			}
			return culture;
		}

        /// <summary>
        /// Update the SelectedLanguage using the numerical value of the Language within the enum
        /// </summary>
		public static void UpdateLanguage(string language)
		{
            var intTest = 0;
            if (string.IsNullOrEmpty(language) || int.TryParse(language, out intTest))
            {
                return;
            }

            var cultureLang = new CultureInfo(language);

            if (!Equals(cultureLang, SelectedLanguage) && !string.IsNullOrEmpty(cultureLang.Name) && Languages.Contains(cultureLang))
			{
				UpdateLanguage(cultureLang);
			}
		}

        /// <summary>
        /// Update the SelectedLanguage
        /// </summary>
		public static void UpdateLanguage(CultureInfo language)
		{
			if (language != SelectedLanguage)
			{
				SelectedLanguage = language;
				PlayerPrefs.SetString("Last_Saved_Language", language.Name);
				((Localization[])FindObjectsOfType(typeof(Localization))).ToList().ForEach(l => l.Set());
				LanguageChange();
				Debug.Log(SelectedLanguage);
			}
		}
	}
}