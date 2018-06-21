# Localization
## Function 
A package to handle the storing and retrieving of text translated into multiple languages.
## Usage
### Starting CurrentLanguage Selection Process
1. If localization has been set in the past, it is loaded using PlayerPrefs with the key "Last_Saved_Language".
2. If no language was saved or was not included as an option in the application, CultureInfo.CurrentUICulture is used.
3. If CultureInfo.CurrentUICulture was null or was not included as an option in the application, CultureInfo.CurrentCulture is used.
4. If CultureInfo.CurrentCulture was null or was not included as an option in the application, the language returned by GetFromSystemLanguage(), which uses Unity's Application.systemLanguage, is used.
5. If the language returned by GetFromSystemLanguage() is null or was not included as an option in the application, the DefaultLanguage is used.

### String Selection Process
0. If in editor, not playing and LangaugeOverride has been set, LanguageOverride is efeectively CurrentLanguage in that situation.
1. If a value exists for the CurrentLanguage for the Key provided and it doesn't match the EmptyStringText, that value will be returned.
2. Otherwise, if the parent of the CurrentLanguage does not match the InvariantCulture, is included in the Language list and has a value that doesn't match the EmptyStringText, that value will be returned.
3. Otherwise, if any children of the CurrentLanguage's parent (or CurrentLnaguage if its parent matches the InvariantCulture) has a value that doesn't match the EmptyStringText, the first value found will be returned.
4. Otherwise, if a value exists for the DefaultLanguage for the Key provided and it doesn't match the EmptyStringText, that value will be returned.
5. Otherwise, the key itself will be returned.

### UILocalization
Base class for UI, such as Text and Dropdown, to localize upon being enabled. Features an abstract 'Set' method which is called within the 'OnEnable' method.

LanguageOverride and 'Localize Text' buttons can be used to test localization on that UI element whilst not in Play Mode in-editor.

### TextLocalization
Sets text to match the localized string for the provided key upon OnEnable or whenever the 'Set' method is called in code. Toggling ToUpper on results in a completely upper case variant being used.

### DropdownLocalization
Set the options for the Dropdown to be localized upon OnEnable or whenever the 'Set' method is called in code. Dropdown options are set by passing all localization keys via the 'SetOptions' method in code or by setting them in-editor.
``` c#
  string SetOptions(List<string> options)
```

### Font Localization Character Check
The 'Font Localization Character Check' option under 'PlayGen Tools' will check which characters currently being used in Localization are missing in any of the fonts in the Unity project.

#### Issues
- This also loads and checks the default Arial font and the font(s) being used by the Unity Editor on that platform.
- Checking only works when a font isn't set to be dynamic, as dynamic fonts fall back to using the default font when a character isn't available and thus pass every test. A warning is shown when this check is run and a font is still set as dynamic.

### Code calls
In order to set text programmatically, the following methods can be used:
``` c#
  string Get(string key, bool toUpper = false, string overrideLanguage = null)
  string GetAndFormat(string key, bool toUpper, params object[] args)
  string GetAndFormat(string key, bool toUpper, params string[] args)
```
In order to update the CurrentLanguage, the following methods can be used:
``` c#
  string UpdateLanguage(string language)
  string UpdateLanguage(CultureInfo cultureLang)
```
Alongside being loaded from Resources at start-up, JSON can also be passed to Localization using the following:
``` c#
  string AddLocalization(TextAsset textAsset)
  string AddLocalization(string text)
```
## Limitations
- EmptyStringText const is set to 'XXXX' in code and currently cannot be changed.
- FilePath in Resources is set to 'Localization' and currently cannot be changed.
- PlayerPrefs key is set in code and currently cannot be changed.