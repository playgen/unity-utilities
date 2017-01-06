using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
public enum Language
{
	None = 0,
	[Name("en-gb")]
	English,
	[Name("en-us")]
	AmericanEnglish,
	[Name("fr")]
	French,
	[Name("es")]
	Spanish,
	[Name("it")]
	Italian,
	[Name("de")]
	German,
	[Name("nl")]
	Dutch,
	[Name("el")]
	Greek,
	[Name("ja")]
	Japanese,
	[Name("zh-cn")]
	ChineseSimplified
}

public class Localization : MonoBehaviour
{
	private static readonly Dictionary<Language, Dictionary<string, string>> LocalizationDict = new Dictionary<Language, Dictionary<string, string>>();

	public string Key;
	public bool ToUpper;

	private const string EmptyStringText = "XXXX";

	public static string FilePath = "StringLocalizations";
	public static Language SelectedLanguage { get; set; }
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
		Text _text = GetComponent<Text>();
		if (_text == null)
		{
			Debug.LogError("Localization script could not find Text component attached to this gameObject: " + gameObject.name);
			return;
		}
		_text.text = Get(Key, ToUpper, LanguageOverride);
	}

	private static void GetLocalizationDictionary()
	{
		TextAsset jsonTextAsset = Resources.Load(FilePath) as TextAsset;

		var N = JSON.Parse(jsonTextAsset.text);
		foreach (Language l in Enum.GetValues(typeof(Language)))
		{
			var fieldInfo = typeof(Language).GetField(l.ToString());
			var attributes = (NameAttribute[])fieldInfo.GetCustomAttributes(typeof(NameAttribute), false);
			var languageHeader = attributes.Any() ? attributes.First().Name : l.ToString();
			Dictionary<string, string> languageStrings = new Dictionary<string, string>();
			for (int i = 0; i < N.Count; i++)
			{
				//go through the list and add the strings to the dictionary
				if (N[i][languageHeader] != null)
				{
					string key = N[i][0].ToString();
					key = key.Replace("\"", "").ToUpper();
					string value = N[i][languageHeader].ToString();
					value = value.Replace("\"", "");
					languageStrings[key] = value;
				}
			}
			LocalizationDict[l] = languageStrings;
		}
		if (PlayerPrefs.HasKey("Last_Saved_Language"))
		{
			SelectedLanguage = (Language)PlayerPrefs.GetInt("Last_Saved_Language");
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

	private static void GetSystemLanguage()
	{
		switch (Application.systemLanguage)
		{
			case SystemLanguage.English:
				SelectedLanguage = LocalizationDict[Language.English].Count > 0 ? Language.English : 0;
				break;
			case SystemLanguage.French:
				SelectedLanguage = LocalizationDict[Language.French].Count > 0 ? Language.English : 0;
				break;
			case SystemLanguage.Spanish:
				SelectedLanguage = LocalizationDict[Language.Spanish].Count > 0 ? Language.English : 0;
				break;
			case SystemLanguage.Italian:
				SelectedLanguage = LocalizationDict[Language.Italian].Count > 0 ? Language.English : 0;
				break;
			case SystemLanguage.German:
				SelectedLanguage = LocalizationDict[Language.German].Count > 0 ? Language.English : 0;
				break;
			case SystemLanguage.Dutch:
				SelectedLanguage = LocalizationDict[Language.Dutch].Count > 0 ? Language.English : 0;
				break;
			case SystemLanguage.Greek:
				SelectedLanguage = LocalizationDict[Language.Greek].Count > 0 ? Language.English : 0;
				break;
			case SystemLanguage.Japanese:
				SelectedLanguage = LocalizationDict[Language.Japanese].Count > 0 ? Language.English : 0;
				break;
			case SystemLanguage.ChineseSimplified:
				SelectedLanguage = LocalizationDict[Language.ChineseSimplified].Count > 0 ? Language.English : 0;
				break;
		}
		if (SelectedLanguage == 0)
		{
			SelectedLanguage = ((Language[])Enum.GetValues(typeof(Language))).FirstOrDefault(lang => LocalizationDict[lang].Count > 0);
			if (SelectedLanguage == 0)
			{
				SelectedLanguage = DefaultLanguage;
			}
		}
	}

	public static List<string> AvailableLanguages()
	{
		var languages = (Language[])Enum.GetValues(typeof(Language));
		var usedLanguages = languages.Where(lang => LocalizationDict[lang].Count > 0).Select(lang => lang.ToString()).ToList();
		return usedLanguages;
	}

	public static void UpdateLanguage(int language)
	{
		UpdateLanguage((Language)Enum.Parse(typeof(Language), AvailableLanguages()[language]));
	}

	public static void UpdateLanguage(Language language)
	{
		SelectedLanguage = language;
		PlayerPrefs.SetInt("Last_Saved_Language", (int)SelectedLanguage);
		((Localization[])FindObjectsOfType(typeof(Localization))).ToList().ForEach(l => l.Set());
		LanguageChange();
		Debug.Log(SelectedLanguage);
	}
}

[AttributeUsage(AttributeTargets.Field)]
public class NameAttribute : Attribute
{
	public string Name { get; set; }

	public NameAttribute(string name)
	{
		Name = name;
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
			}
		}
	}
}
#endif