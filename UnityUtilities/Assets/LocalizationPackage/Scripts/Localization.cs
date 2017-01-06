using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;

public class Localization : MonoBehaviour {

    static Dictionary<string, string> localizationDict = new Dictionary<string, string>();
    static Dictionary<string, string> defaultLocalizationDict = new Dictionary<string, string>();

    private const string EMPTY_STRING_TEXT = "XXXX";
    private const int DEFAULT_LANGUAGE_INDEX = 1;

    public string Key;
    public bool toUpper;
    public string filePath = "StringLocalizations";

    #region LocalizationTesting

    public enum Languages {
        None = 0,
        English = 1,
        /*American English = 2,*/
        French = 3,
        Spanish = 4,
        Italian = 5,
        German = 6,
        Dutch = 7,
        Greek = 8,
        Japanese = 9,
        Chinese_Simplified = 10
    }
    [Space(10)]
    [Header("Localisation Testing")]
    [Tooltip("Use this enum to test other languages")]
    public Languages languageOverride;
    public static int languageIndex = 0;
    #endregion

    void Awake()
    {
        //if this element has language override, override all languages
        if (languageOverride != Languages.None && languageIndex == 0)
        {
            languageIndex = (int)languageOverride;
        }
    }

    private void ConvertJsonToDict(ref Dictionary<string, string> dict, int index)
    {
        TextAsset jsonTextAsset = Resources.Load(filePath) as TextAsset;

        var N = JSON.Parse(jsonTextAsset.text);

        for (int i = 0; N[i] != null; i++)
        {
            //go through the list and add the strings to the dictionary
            string _key = N[i][0].ToString();
            _key = _key.Replace("\"", "");
            string _value = N[i][index].ToString();
            _value = _value.Replace("\"", "");

            //update our keys with our values
            dict[_key] = _value;
        }
    }

    void Start()
    {
        if (languageIndex == 0)
            languageIndex = GetLanguageIndex();
        if (languageIndex != DEFAULT_LANGUAGE_INDEX)
        {
            //only populate the default localization dictionary if it is different to the current language
            ConvertJsonToDict(ref defaultLocalizationDict, DEFAULT_LANGUAGE_INDEX);
        }
        ConvertJsonToDict(ref localizationDict, languageIndex);

        //set our text
        Text _text = this.GetComponent<Text>();

        if (_text == null)
            Debug.LogError("Localization script could not find Text component attached to this gameObject: " + this.gameObject.name);

        _text.text = Get(Key);

        if (_text.text == "")
        {
            Debug.LogError("Could not find string with key: " + Key);
        }
        if (toUpper)
            _text.text = _text.text.ToUpper();
    }

    public static string Get(string key)
    {
        string txt = "";
        key = key.ToUpper();

        localizationDict.TryGetValue(key, out txt);
        if (txt == EMPTY_STRING_TEXT)
        {
            //if the text received is the same as the pre defined string that symbolises a null string
            defaultLocalizationDict.TryGetValue(key, out txt);
        }
        //typing \n in excel spreadsheet will format to \\n so we put it back here
        txt = txt.Replace("\\n*", "\n");
        
        return txt;
    }

    private int GetLanguageIndex()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                return 1;
            //american english takes language index 2
            case SystemLanguage.French:
                return 3;
            case SystemLanguage.Spanish:
                return 4;
            case SystemLanguage.Italian:
                return 5;
            case SystemLanguage.German:
                return 6;
            case SystemLanguage.Dutch:
                return 7;
            case SystemLanguage.Greek:
                return 8;
            case SystemLanguage.Japanese:
                return 9;
            case SystemLanguage.ChineseSimplified:
                return 10;

        }
        //default set to english
        return 1;
    }
    public void RebuildJsonWithNewLanguage(SystemLanguage newLanguage)
    {
        switch(newLanguage)
        {
            case SystemLanguage.English:
                languageIndex = 1;
                break;
            //american english takes language index 2
            case SystemLanguage.French:
                languageIndex = 3;
                break;
            case SystemLanguage.Spanish:
                languageIndex = 4;
                break;
            case SystemLanguage.Italian:
                languageIndex = 5;
                break;
            case SystemLanguage.German:
                languageIndex = 6;
                break;
            case SystemLanguage.Dutch:
                languageIndex = 7;
                break;
            case SystemLanguage.Greek:
                languageIndex = 8;
                break;
            case SystemLanguage.Japanese:
                languageIndex = 9;
                break;
            case SystemLanguage.ChineseSimplified:
                languageIndex = 10;
                break;

        }
        ConvertJsonToDict(ref localizationDict, languageIndex);
    }
}
