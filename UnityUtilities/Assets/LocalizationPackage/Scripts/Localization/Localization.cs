using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;
using System;
using System.Globalization;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
public enum Language
{
	None = 0,
	[Language("en-gb", "en-gb")]
	English,
	[Language("en-us", "en-us")]
	AmericanEnglish,
	[Language("fr", "fr-fr")]
	French,
	[Language("es", "es-es")]
	Spanish,
	[Language("it", "it-it")]
	Italian,
	[Language("de", "de-de")]
	German,
	[Language("nl", "nl-nl")]
	Dutch,
	[Language("el", "el-gr")]
	Greek,
	[Language("ja", "ja-jp")]
	Japanese,
	[Language("zh-cn", "zh-cn")]
	ChineseSimplified
}

public class Localization : MonoBehaviour
{
	private static readonly Dictionary<Language, Dictionary<string, string>> LocalizationDict = new Dictionary<Language, Dictionary<string, string>>();

	public string Key;
	public bool ToUpper;

	private const string EmptyStringText = "XXXX";

	public static string FilePath = "Localization";
	public static Language SelectedLanguage { get; set; }
	public static CultureInfo SelectedCulture { get; set; }
	public static Language DefaultLanguage = Language.English;
	public static event Action LanguageChange = delegate { };

	#region LocalizationTesting
	[Header("Localization Testing")]
	[Tooltip("Use this enum to test other languages")]
	public Language LanguageOverride;
	#endregion

	private void OnEnable()
	{
		Set();
	}

	public void Set()
	{
		Text text = GetComponent<Text>();
		if (text == null)
		{
			Debug.LogError("Localization script could not find Text component attached to this gameObject: " + gameObject.name);
			return;
		}
		text.text = Get(Key, ToUpper, LanguageOverride);
	}

	private static void GetLocalizationDictionary()
	{
		TextAsset[] jsonTextAssets = Resources.LoadAll(FilePath, typeof(TextAsset)).Cast<TextAsset>().ToArray();

		foreach (Language l in Enum.GetValues(typeof(Language)))
		{
			var fieldInfo = typeof(Language).GetField(l.ToString());
			var attributes = (LanguageAttribute[])fieldInfo.GetCustomAttributes(typeof(LanguageAttribute), false);
			var languageHeader = attributes.Any() ? attributes.First().Name : l.ToString();
			Dictionary<string, string> languageStrings = new Dictionary<string, string>();
			foreach (var textAsset in jsonTextAssets)
			{
				var n = JSON.Parse(textAsset.text);
				for (int i = 0; i < n.Count; i++)
				{
					//go through the list and add the strings to the dictionary
					if (n[i][languageHeader] != null)
					{
						string key = n[i][0].ToString();
						key = key.Replace("\"", "").ToUpper();
						string value = n[i][languageHeader].ToString();
						value = value.Replace("\"", "");
						languageStrings[key] = value;
					}
				}
			}
			LocalizationDict[l] = languageStrings;
		}
		if (PlayerPrefs.HasKey("Last_Saved_Language"))
		{
			UpdateLanguage((Language)PlayerPrefs.GetInt("Last_Saved_Language"));
		}
		else
		{
			GetSystemLanguage();
			PlayerPrefs.SetInt("Last_Saved_Language", (int)SelectedLanguage);
		}
	}

	public static string Get(string key, bool toUpper = false, Language overrideLanguage = Language.None)
	{
		if (SelectedLanguage == 0)
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

		if (overrideLanguage == Language.None || overrideLanguage == SelectedLanguage)
		{
			LocalizationDict[SelectedLanguage].TryGetValue(newKey, out txt);
		}
		else
		{
			LocalizationDict[overrideLanguage].TryGetValue(newKey, out txt);
		}
		if (txt == null || txt == EmptyStringText)
		{
			if (txt == null)
			{
				Debug.LogError("Could not find string with key '" + key + "' in Language " + SelectedLanguage);
			}
			if ((overrideLanguage != Language.None && overrideLanguage != SelectedLanguage && overrideLanguage != DefaultLanguage) || SelectedLanguage != DefaultLanguage)
			{
				LocalizationDict[DefaultLanguage].TryGetValue(newKey, out txt);
			}
			if (txt == null)
			{
				txt = key;
			}
		}
		//new line character in spreadsheet is *n*
		txt = txt.Replace("\\n", "\n");
		if (toUpper)
		{
			txt = txt.ToUpper();
		}
		return txt;
	}

	public static string GetAndFormat(string key, bool toUpper, params object[] args)
	{
		return string.Format(Get(key, toUpper), args);
	}

	public static string GetAndFormat(string key, bool toUpper, params string[] args)
	{
		return GetAndFormat(key, toUpper, args.ToArray<object>());
	}

	public static bool HasKey(string key)
	{
		var newKey = key.ToUpper();
		newKey = newKey.Replace('-', '_');
		return LocalizationDict[SelectedLanguage].ContainsKey(newKey);
	}

	private static void GetSystemLanguage()
	{
		switch (Application.systemLanguage)
		{
			case SystemLanguage.English:
				if (LocalizationDict[Language.English].Count > 0)
				{
					UpdateLanguage(Language.English);
				}
				break;
			case SystemLanguage.French:
				if (LocalizationDict[Language.French].Count > 0)
				{
					UpdateLanguage(Language.French);
				}
				break;
			case SystemLanguage.Spanish:
				if (LocalizationDict[Language.Spanish].Count > 0)
				{
					UpdateLanguage(Language.Spanish);
				}
				break;
			case SystemLanguage.Italian:
				if (LocalizationDict[Language.Italian].Count > 0)
				{
					UpdateLanguage(Language.Italian);
				}
				break;
			case SystemLanguage.German:
				if (LocalizationDict[Language.German].Count > 0)
				{
					UpdateLanguage(Language.German);
				}
				break;
			case SystemLanguage.Dutch:
				if (LocalizationDict[Language.Dutch].Count > 0)
				{
					UpdateLanguage(Language.Dutch);
				}
				break;
			case SystemLanguage.Greek:
				if (LocalizationDict[Language.Greek].Count > 0)
				{
					UpdateLanguage(Language.Greek);
				}
				break;
			case SystemLanguage.Japanese:
				if (LocalizationDict[Language.Japanese].Count > 0)
				{
					UpdateLanguage(Language.Japanese);
				}
				break;
			case SystemLanguage.ChineseSimplified:
				if (LocalizationDict[Language.ChineseSimplified].Count > 0)
				{
					UpdateLanguage(Language.ChineseSimplified);
				}
				break;
		}
		if (SelectedLanguage == 0)
		{
			var firstLanguage = ((Language[])Enum.GetValues(typeof(Language))).FirstOrDefault(lang => LocalizationDict[lang].Count > 0);
			UpdateLanguage(firstLanguage != 0 ? firstLanguage : DefaultLanguage);
		}
	}

	public static List<string> AvailableLanguages()
	{
		if (LocalizationDict == null || LocalizationDict.Count == 0)
		{
			GetLocalizationDictionary();
		}
		var languages = (Language[])Enum.GetValues(typeof(Language));
		var usedLanguages = languages.Where(lang => LocalizationDict[lang].Count > 0).Select(lang => lang.ToString()).ToList();
		return usedLanguages;
	}

	public static void UpdateLanguage(int language)
	{
		if (AvailableLanguages()[language] != SelectedLanguage.ToString())
		{
			UpdateLanguage((Language)Enum.Parse(typeof(Language), AvailableLanguages()[language]));
		}
	}

	public static void UpdateLanguage(Language language)
	{
		if (language != SelectedLanguage)
		{
			SelectedLanguage = language;
			var fieldInfo = typeof(Language).GetField(language.ToString());
			var attributes = (LanguageAttribute[])fieldInfo.GetCustomAttributes(typeof(LanguageAttribute), false);
			SelectedCulture = attributes.Any() ? new CultureInfo(attributes.First().Culture) : CultureInfo.InvariantCulture;
			PlayerPrefs.SetInt("Last_Saved_Language", (int)SelectedLanguage);
			((Localization[])FindObjectsOfType(typeof(Localization))).ToList().ForEach(l => l.Set());
			LanguageChange();
			Debug.Log(SelectedLanguage);
			Debug.Log(SelectedCulture);
		}
	}
}

[AttributeUsage(AttributeTargets.Field)]
public class LanguageAttribute : Attribute
{
	public string Name { get; set; }
	public string Culture { get; set; }

	public LanguageAttribute(string name, string culture)
	{
		Name = name;
		Culture = culture;
	}
}

#if UNITY_EDITOR

[CustomEditor(typeof(Localization))]
public class LocalizationEditor : Editor
{
	private Language _lastLang;
	private Localization _myLoc;
	
	public void Awake()
	{
		_myLoc = (Localization)target;
		_lastLang = _myLoc.LanguageOverride;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if (EditorApplication.isPlaying)
		{
			if (_lastLang != _myLoc.LanguageOverride)
			{
				_myLoc.Set();
				_lastLang = _myLoc.LanguageOverride;
			}
		}
	}
}
#endif