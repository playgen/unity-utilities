using UnityEngine;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Linq;

using Newtonsoft.Json;

namespace PlayGen.Unity.Utilities.Localization
{
	public static class Localization
	{
		private static Dictionary<CultureInfo, Dictionary<string, string>> _localizationDict = new Dictionary<CultureInfo, Dictionary<string, string>>();
		private const string EmptyStringText = "XXXX";
		private const string FilePath = "Localization";

		public static List<CultureInfo> Languages = new List<CultureInfo>();
		/// <summary>
		/// The Language currently in-use
		/// </summary>
		public static CultureInfo SelectedLanguage { get; set; }
		/// <summary>
		/// The Specific version of the Language currently in-use
		/// </summary>
		public static CultureInfo SpecificSelectedLanguage => SelectedLanguage.IsNeutralCulture ? CultureInfo.CreateSpecificCulture(SelectedLanguage.Name) : SelectedLanguage;
		/// <summary>
		/// Language to use if none is set or selected language does not have text for provided key
		/// </summary>
		public static CultureInfo DefaultLanguage = new CultureInfo("en-gb");
		/// <summary>
		/// Event triggered by changing of SelectedLanguage
		/// </summary>
		public static event Action LanguageChange = delegate { };

		private static void GetLocalizationDictionary(bool loadLang = true)
		{
			var jsonTextAssets = Resources.LoadAll(FilePath, typeof(TextAsset)).Cast<TextAsset>().ToArray();
			foreach (var textAsset in jsonTextAssets)
			{
				AddLocalization(textAsset);
			}

			if (loadLang)
			{
				UpdateLanguage(PlayerPrefs.GetString("Last_Saved_Language"));
				if (string.IsNullOrEmpty(SelectedLanguage?.Name))
				{
					if (!string.IsNullOrEmpty(CultureInfo.CurrentUICulture.Name))
					{
						UpdateLanguage(GetLanguage(CultureInfo.CurrentUICulture));
					}
					else if (!string.IsNullOrEmpty(CultureInfo.CurrentCulture.Name))
					{
						UpdateLanguage(GetLanguage(CultureInfo.CurrentCulture));
					}
					else if (!string.IsNullOrEmpty(GetLanguage(GetFromSystemLanguage()).Name))
					{
						UpdateLanguage(GetLanguage(GetFromSystemLanguage()));
					}
					else
					{
						UpdateLanguage(GetLanguage(DefaultLanguage));
					}
					if (SelectedLanguage != null)
					{
						PlayerPrefs.SetString("Last_Saved_Language", SelectedLanguage.Name);
					}
				}
			}
		}

		public static void AddLocalization(TextAsset textAsset)
		{
			AddLocalization(textAsset.text);
		}

		public static void AddLocalization(string text)
		{
			var json = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(text);
			var comparer = StringComparer.OrdinalIgnoreCase;
			json = json.Select(d => new Dictionary<string, string>(d, comparer)).ToList();
			AddLanguages(json);
			AddToDict(json);
			RemoveUnused();
		}

		private static void AddLanguages(List<Dictionary<string, string>> json)
		{
			var keys = json.SelectMany(j => j.Keys.Where(k => !k.Equals("key", StringComparison.OrdinalIgnoreCase))).Distinct().ToList();
			foreach (var key in keys)
			{
				var culture = new CultureInfo(key);
				if (!Languages.Contains(culture))
				{
					if (Languages.Count == 0)
					{
						DefaultLanguage = culture;
					}
					Languages.Add(culture);
				}
			}
		}

		private static void AddToDict(List<Dictionary<string, string>> json)
		{
			foreach (var l in Languages)
			{
				var comparer = StringComparer.OrdinalIgnoreCase;
				if (!_localizationDict.ContainsKey(l))
				{
					_localizationDict.Add(l, new Dictionary<string, string>(comparer));
				}
			}

			foreach (var dict in json)
			{
				if (dict.TryGetValue("key", out var key))
				{
					foreach (var l in Languages)
					{
						if (dict.TryGetValue(l.Name, out var str) && str != EmptyStringText)
						{
							if (_localizationDict[l].ContainsKey(key))
							{
								_localizationDict[l][key] = str;
							}
							else
							{
								_localizationDict[l].Add(key, str);
							}
							
						}
					}
				}
			}
		}

		private static void RemoveUnused()
		{
			_localizationDict = _localizationDict.Where(l => l.Value.Count > 0).ToDictionary(k => k.Key, v => v.Value);
			Languages = Languages.Where(l => _localizationDict.ContainsKey(l)).ToList();
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
			var newKey = key.ToUpper();
			newKey = newKey.Replace('-', '_').Trim();

			var getLang = SelectedLanguage;

			if (SelectedLanguage == null || !string.IsNullOrEmpty(overrideLanguage) && overrideLanguage != SelectedLanguage.Name)
			{
				getLang = GetLanguage(new CultureInfo(overrideLanguage));
			}
			_localizationDict[getLang].TryGetValue(newKey, out var txt);
			if (txt == null || txt == EmptyStringText)
			{
				if (txt == null)
				{
					Debug.LogWarning("Could not find string with key '" + key + "' in Language " + getLang);
				}
				if (!Equals(getLang.Parent, CultureInfo.InvariantCulture) && Languages.Contains(getLang.Parent))
				{
					_localizationDict[getLang.Parent].TryGetValue(newKey, out txt);
				}
			}
			if (txt == null || txt == EmptyStringText)
			{
				var parentLang = !Equals(getLang.Parent, CultureInfo.InvariantCulture) ? getLang.Parent : getLang;
				foreach (var lang in Languages)
				{
					if (Equals(lang.Parent, parentLang) && !Equals(lang, getLang))
					{
						_localizationDict[lang].TryGetValue(newKey, out txt);
						if (txt != null && txt != EmptyStringText)
						{
							break;
						}
					}
				}
			}
			if (Languages.Contains(DefaultLanguage) && (txt == null || txt == EmptyStringText))
			{
				_localizationDict[DefaultLanguage].TryGetValue(newKey, out txt);
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
			return _localizationDict[SelectedLanguage].ContainsKey(newKey);
		}

		public static CultureInfo GetFromSystemLanguage()
		{
			return CultureInfo.GetCultures(CultureTypes.NeutralCultures).FirstOrDefault(r => r.EnglishName == Application.systemLanguage.ToString());
		}

		private static CultureInfo GetLanguage(CultureInfo language)
		{
			var culture = language;
			if (!Languages.Contains(culture))
			{
				if (language != null && !Equals(language.Parent, CultureInfo.InvariantCulture))
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
		/// Update the SelectedLanguage using the Language name
		/// </summary>
		public static void UpdateLanguage(string language)
		{
			if (string.IsNullOrEmpty(language) || int.TryParse(language, out _))
			{
				return;
			}

			var cultureLang = new CultureInfo(language);
			UpdateLanguage(cultureLang);
		}

		/// <summary>
		/// Update the SelectedLanguage
		/// </summary>
		public static void UpdateLanguage(CultureInfo cultureLang)
		{
			if (Languages.Count == 0)
			{
				GetLocalizationDictionary(false);
			}
			var language = new CultureInfo(cultureLang.Name);
			if (!Equals(language, SelectedLanguage) && !string.IsNullOrEmpty(language.Name))
			{
				if (!Languages.Contains(language))
				{
					if (!Equals(language.Parent, CultureInfo.InvariantCulture) && Languages.Contains(language.Parent))
					{
						language = language.Parent;
					}
				}
				if (!Languages.Contains(language))
				{
					var parentLang = !Equals(language.Parent, CultureInfo.InvariantCulture) ? language.Parent : language;
					foreach (var lang in Languages)
					{
						if (Equals(lang.Parent, parentLang))
						{
							language = lang;
							break;
						}
					}
				}
				if (Languages.Contains(language))
				{
					SelectedLanguage = language;
					if (Application.isPlaying)
					{
						PlayerPrefs.SetString("Last_Saved_Language", language.Name);
						((UILocalization[])UnityEngine.Object.FindObjectsOfType(typeof(UILocalization))).ToList().ForEach(l => l.Set());
						LanguageChange();
						Debug.Log(SelectedLanguage);
					}
				}
			}
		}
	}
}