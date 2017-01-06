Updated 06-01-2017 
By Felix Wentworth

Package contains a localization script as well as a excel spreadsheet to be filled out. Located at Resources -> StringFile.xlsz.
For any languages that are not required within the game they must still be given a value, otherwise the conversion from excel spreadsheet to JSON will fail. 
To handle missing strings being shown in foreign langueages being shown, the localization script contains 2 const values that should be edited:

EMPTY_STRING_TEXT - The text that is entered into the spreadsheet to mark a missing translation (default is set to "XXXX")
DEFAULT_LANGUAGE_INDEX - The index of the language that should be used by defualt (default is set to 1 (English))

The localization script works for unity UI Text Components and will set the language based off of a Key value

Supported Languages and their indexes:
	English,				Language Index = 1
	French,					Language Index = 3
	Spanish,				Language Index = 4
	Italian,				Language Index = 5
	German,					Language Index = 6
	Dutch,					Language Index = 7
	Greek,					Language Index = 8
	Japanese,				Language Index = 9
	Chinese (Simplified)	Language Index = 10
	
Language index 2 is taken by american english, functionality for this can be created but will need to get the devices region to find whether they are in america or not

The localization script can be used in 2 ways, attaching directly to a Text object and setting the Key to match the key value in the excel spreadsheet and it will set the text on Start()
The localization script can also be called using Localization.Get(string key) passing in the key value and getting the string returned

There are 2 external packages that are used in this localization package:

SimpleJSON - http://wiki.unity3d.com/index.php/SimpleJSON
ExcelToJSONConverter - https://github.com/Benzino/ExcelToJsonConverter

To update the json file in the Resources folder use Tools -> Excel To Json Converter

The Test Scene added is simply a Text object that is setting its text value using the Localization script, the TextToBeOverriden object is used to test that a language override works, if one object has a language override set, all text will be set to that language 		